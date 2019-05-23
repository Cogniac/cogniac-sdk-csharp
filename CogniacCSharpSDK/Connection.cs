/*
    Copyright 2019 Cogniac Corporation.

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/

using System;
using System.IO;
using System.Collections.Generic;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using System.Net;
using System.Timers;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Cogniac
{
    /// <summary>
    /// Main entry point of the SDK
    /// </summary>
    public class Connection
    {
        private string _urlPrefix = "";
        private string _username = "";
        private string _password = "";
        private string _tenantId = "";
        private string _providedToken = "";
        private Auth _authObject = null;
        private bool _autoRenewToken = true;
        private long? _tokenExpiresIn = 0;
        private Timer _tokenTimer;
        private const uint _expirationOffset = 30;

        /// <summary>
        /// Main class to establish a connection to the Cogniac public API.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="tenantId">Tenant ID</param>
        /// <param name="token">Access Token</param>
        /// <param name="urlPrefix">URL Prefix</param>
        /// <param name="autoRenewToken">Auto Renew Access Token</param>
        public Connection(string username = "", string password = "", string tenantId = "", string token = "", string urlPrefix = "https://api.cogniac.io/1", bool autoRenewToken = true)
        {
            _username = username;
            _password = password;
            _providedToken = token;
            _tenantId = tenantId;
            _urlPrefix = urlPrefix;
            _autoRenewToken = autoRenewToken;

            // if token is provided, no need to validate other input data
            if (string.IsNullOrEmpty(_providedToken))
            {
                ValidateUserPass();
                ValidateTenantId();
                Authenticate();
            }
            if (string.IsNullOrEmpty(_urlPrefix))
            {
                throw new ArgumentException(message: "urlPrefix parameter is null or empty.");
            }
        }

        /// <summary>
        /// Gets an Auth object after a Connection object is created
        /// </summary>
        /// <returns>Cogniac.Auth object</returns>
        public Auth GetAuthObject()
        {
            return _authObject;
        }

        private void Authenticate()
        {
            string fullUrl = $"{_urlPrefix}/token?tenant_id={_tenantId}";
            IRestResponse resp = ExecuteRequest(fullUrl);
            try
            {
                // Populate the Auth object with the missing information
                _authObject = Auth.FromJson(resp.Content);
                _providedToken = _authObject.AccessToken;
                var decoded_token = new JwtSecurityToken(jwtEncodedString: _providedToken);
                _authObject.UserId = decoded_token.Claims.First(c => c.Type == "sub").Value;
                _authObject.TenantId = decoded_token.Claims.First(c => c.Type == "tid").Value;
                _authObject.ExpiresIn = (long)(Convert.ToDouble(decoded_token.Claims.First(c => c.Type == "exp").Value)
                    - Convert.ToDouble(decoded_token.Claims.First(c => c.Type == "iat").Value));
                _authObject.TenantName = GetTenant(_authObject.TenantId).Name;
                _authObject.UserEmail = decoded_token.Claims.First(c => c.Type == "ema").Value;
                _tokenExpiresIn = _authObject.ExpiresIn;
                if (_autoRenewToken == true)
                {
                    if (_tokenExpiresIn != null)
                    {
                        _tokenTimer = new Timer();
                        _tokenTimer.Elapsed += new ElapsedEventHandler(OnTokenTimedEvent);
                        _tokenTimer.Interval = TimeSpan.FromSeconds((double)(_tokenExpiresIn - _expirationOffset)).TotalMilliseconds;
                        _tokenTimer.Enabled = true;
                        _tokenTimer.Start();
                    }
                }
            }
            catch
            {
                throw new ArgumentException(message: resp.Content);
            }
        }

        private void OnTokenTimedEvent(object source, ElapsedEventArgs e)
        {
            string fullUrl = $"{_urlPrefix}/token?tenant_id={_tenantId}";
            var client = new RestClient(fullUrl);
            var request = new RestRequest(Method.GET);
            IRestResponse resp = null;
            try
            {
                client.Authenticator = new HttpBasicAuthenticator(_username, _password);
                client.AddHandler("application/json", new JsonDeserializer());
                request.AddHeader("Cache-Control", "no-cache");
                resp = Retry.Do(() => client.Execute(request), TimeSpan.FromSeconds(5), 3);
                _authObject = Auth.FromJson(resp.Content);
                _providedToken = _authObject.AccessToken;
                var decoded_token = new JwtSecurityToken(jwtEncodedString: _providedToken);
                _authObject.UserId = decoded_token.Claims.First(c => c.Type == "sub").Value;
                _authObject.TenantId = decoded_token.Claims.First(c => c.Type == "tid").Value;
                _authObject.ExpiresIn = (long)(Convert.ToDouble(decoded_token.Claims.First(c => c.Type == "exp").Value)
                    - Convert.ToDouble(decoded_token.Claims.First(c => c.Type == "iat").Value));
                _authObject.TenantName = GetTenant(_authObject.TenantId).Name;
                _authObject.UserEmail = decoded_token.Claims.First(c => c.Type == "ema").Value;
                _tokenExpiresIn = _authObject.ExpiresIn;
                _tokenTimer.Interval = TimeSpan.FromSeconds((double)(_tokenExpiresIn - _expirationOffset)).TotalMilliseconds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IRestResponse ExecuteRequest(string fullUrl, RestRequest request = null)
        {
            var client = new RestClient(fullUrl);
            if(request == null)
            {
                request = new RestRequest(Method.GET);
            }
            if (!string.IsNullOrEmpty(_providedToken))
            {
                request.AddHeader("Authorization", $"Bearer {_providedToken}");
            }
            else
            {
                client.Authenticator = new HttpBasicAuthenticator(_username, _password);
            }
            client.AddHandler("application/json", new JsonDeserializer());
            request.AddHeader("Cache-Control", "no-cache");
            return Retry.Do(() => client.Execute(request), TimeSpan.FromSeconds(5), 3);
        }

        /// <summary>
        /// Gets an AuthorizedTenants object given a username and password
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="urlPrefix">URL Prefix</param>
        /// <returns>Cogniac.AuthorizedTenants object</returns>
        public static AuthorizedTenants GetAllAuthorizedTenants(string username = "", string password = "", string urlPrefix = "https://api.cogniac.io/1")
        {
            var client = new RestClient($"{urlPrefix}/users/current/tenants");
            var request = new RestRequest(Method.GET);
            AuthorizedTenants returnValue = null;
            IRestResponse resp = null;
            try
            {
                client.Authenticator = new HttpBasicAuthenticator(username, password);
                client.AddHandler("application/json", new JsonDeserializer());
                request.AddHeader("Cache-Control", "no-cache");
                resp = Retry.Do(() => client.Execute(request), TimeSpan.FromSeconds(5), 3);
                returnValue = AuthorizedTenants.FromJson(resp.Content);
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        private void ValidateTenantId()
        {
            // If tenantId is provided, we use it
            if (string.IsNullOrEmpty(_tenantId))
            {
                // We don't have a tenantId, check environment variables
                if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("COG_TENANT")))
                {
                    _tenantId = Environment.GetEnvironmentVariable("COG_TENANT");
                }
                else
                {
                    // We don't have a tenantId in the environment and it is not provided
                    // Get list of tenants, if only 1, use it.
                    AuthorizedTenants at;
                    at = GetAllAuthorizedTenants(_username, _password, urlPrefix: _urlPrefix);
                    if (at.Tenants.Length.Equals(1))
                    {
                        _tenantId = at.Tenants[0].TenantId;
                    }
                    else
                    {
                        throw new ArgumentException(message: "Provided user has multiple or no tenants, please use GetAllAuthorizedTenants(...) first and supply a single valid tenantId parameter.");
                    }
                }
            }

        }
        private void ValidateUserPass()
        {
            // If token is provided, username and passwords don't matter
            if (string.IsNullOrEmpty(_providedToken))
            {
                if (string.IsNullOrEmpty(_username))
                {
                    // We don't have a tenantId, check environment variables
                    if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("COG_USER")))
                    {
                        _username = Environment.GetEnvironmentVariable("COG_USER");
                    }
                    else
                    {
                        throw new ArgumentException(message: "Parameter 'userName' not provided!");
                    }
                }
                if (string.IsNullOrEmpty(_password))
                {
                    // We don't have a tenantId, check environment variables
                    if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("COG_PASS")))
                    {
                        _password = Environment.GetEnvironmentVariable("COG_PASS");
                    }
                    else
                    {
                        throw new ArgumentException(message: "Parameter 'password' is not provided!");
                    }
                }
            }
        }

        /// <summary>
        /// Uploads media to the Cogniac system
        /// </summary>
        /// <param name="forceSet">One of "training" or "validation"</param>
        /// <param name="fileName">File to upload</param>
        /// <param name="mediaTimestamp">Media time stamp</param>
        /// <param name="forceOverwrite">Force overwrite flag</param>
        /// <param name="metaTags">List of meta tags</param>
        /// <param name="isPublic">Public flag</param>
        /// <param name="externalMediaId">External media ID</param>
        /// <param name="originalUrl">Original URL</param>
        /// <param name="originalLandingUrl">Original landing URL</param>
        /// <param name="license">License string</param>
        /// <param name="authorProfileUrl">Author profile URL</param>
        /// <param name="title">Title of media</param>
        /// <param name="sourceUrl">Source URL</param>
        /// <param name="previewUrl">Preview URL</param>
        /// <param name="domainUnit">(E.G. serial number) for set assignment grouping</param>"
        /// <param name="localGatewayUrl">Local gateway URL</param>
        /// <returns>Cogniac.Media object</returns>
        public Media UploadMedia
        (
            string forceSet = null,
            string fileName = null,
            double mediaTimestamp = 0,
            bool forceOverwrite = true,
            string[] metaTags = null,
            bool isPublic = false,
            string externalMediaId = null,
            string originalUrl = null,
            string originalLandingUrl = null,
            string license = null,
            string authorProfileUrl = null,
            string title = null,
            string sourceUrl = null,
            string previewUrl = null,
            string domainUnit = null,
            string localGatewayUrl = null
        )
        {
            if (!string.IsNullOrEmpty(_providedToken))
            {
                byte[] fileBytes;
                if (!String.IsNullOrEmpty(sourceUrl))
                {
                    fileBytes = Helpers.GetFileBytesContent(sourceUrl);
                    fileName = sourceUrl;
                }
                else
                {
                    if (File.Exists(fileName))
                    {
                        fileBytes = Helpers.GetFileBytesContent(fileName);
                    }
                    else
                    {
                        throw new ArgumentException(message: "File does not exist: '" + fileName + "'");
                    }
                }
                if (String.IsNullOrEmpty(fileName) && String.IsNullOrEmpty(sourceUrl))
                {
                    throw new ArgumentException(message: "No input file provided (fileName or sourceUrl");
                }
                string mediaFormat = Path.GetExtension(fileName);
                mediaFormat = mediaFormat.Replace(".", string.Empty);
                Dictionary<string, object> dict = new Dictionary<string, object>
                {
                    { "media_src", "c_sharp_sdk" },
                    { "media_fomat", mediaFormat },
                    { "filename", fileName },
                    { "force_overwrite", forceOverwrite },
                    { "public", isPublic },
                };
                if (mediaTimestamp > 0)
                {
                    dict.Add("media_timestamp", mediaTimestamp);
                }
                if (!String.IsNullOrEmpty(forceSet))
                {
                    if (forceSet.ToLower().Equals("training"))
                    {
                        dict.Add("force_set", "training");
                    }
                    else if (forceSet.ToLower().Equals("validation"))
                    {
                        dict.Add("force_set", "validation");
                    }
                }
                if (!(metaTags == null))
                {
                    dict.Add("meta_tags", Helpers.FlattenStringArray(metaTags));
                }
                if (!String.IsNullOrEmpty(externalMediaId))
                {
                    dict.Add("external_media_id", externalMediaId);
                }
                if (!String.IsNullOrEmpty(originalUrl))
                {
                    dict.Add("original_url", originalUrl);
                }
                if (!String.IsNullOrEmpty(originalLandingUrl))
                {
                    dict.Add("original_landing_url", originalLandingUrl);
                }
                if (!String.IsNullOrEmpty(license))
                {
                    dict.Add("license", license);
                }
                if (!String.IsNullOrEmpty(authorProfileUrl))
                {
                    dict.Add("author_profile_url", authorProfileUrl);
                }
                if (!String.IsNullOrEmpty(title))
                {
                    dict.Add("title", title);
                }
                if (!String.IsNullOrEmpty(previewUrl))
                {
                    dict.Add("preview_url", previewUrl);
                }
                if (!String.IsNullOrEmpty(sourceUrl))
                {
                    dict.Add("source_url", sourceUrl);
                }
                if (!String.IsNullOrEmpty(domainUnit))
                {
                    dict.Add("domain_unit", domainUnit);
                }
                string data = Helpers.MapToQueryString(dict);
                var request = new RestRequest(Method.POST)
                {
                    AlwaysMultipartFormData = true
                };
                request.AddFile("fileData", fileBytes, fileName);
                IRestResponse response;
                if (string.IsNullOrEmpty(localGatewayUrl))
                {
                    response = ExecuteRequest($"{_urlPrefix}/media?{data}", request);
                }
                else
                {
                    response = ExecuteRequest($"{localGatewayUrl}/media?{data}", request);
                }
                if ((response.StatusCode == HttpStatusCode.OK) && (response.IsSuccessful == true))
                {
                    return Media.FromJson(response.Content);
                }
                else
                {
                    throw new WebException(message: "Network error: " + response.Content);
                }
            }
            else
            {
                throw new ArgumentException(message: "Access token is null or empty");
            }
        }

        /// <summary>
        /// Deletes a media file form the Cogniac system given a known media ID
        /// </summary>
        /// <param name="mediaId">Media ID</param>
        /// <param name="localGatewayUrl">Local gateway URL</param>
        /// <returns>True on success</returns>
        public bool DeleteMedia(string mediaId, string localGatewayUrl = null)
        {
            string fullUrl = "";
            if (!string.IsNullOrEmpty(mediaId))
            {
                if (string.IsNullOrEmpty(localGatewayUrl))
                {
                    fullUrl = $"{_urlPrefix}/media/{mediaId}";
                }
                else
                {
                    if (localGatewayUrl.EndsWith("/"))
                    {
                        fullUrl = $"{localGatewayUrl}media/{mediaId}";
                    }
                    else
                    {
                        fullUrl = $"{localGatewayUrl}/media/{mediaId}";
                    }
                }
                var request = new RestRequest(Method.DELETE);
                var response = ExecuteRequest(fullUrl, request);
                if (response.IsSuccessful && (response.StatusCode == HttpStatusCode.NoContent))
                {
                    return true;
                }
                else
                {
                    throw new WebException(message: "Error deleting media: " + response.Content);
                }
            }
            else
            {
                throw new ArgumentException(message: "mediaId parameter is not provided.");
            }
        }

        /// <summary>
        /// Gets all subjects associated with a given tenant
        /// </summary>
        /// <param name="tenantId">Tenant ID</param>
        /// <returns>Cogniac.Subjects object</returns>
        public Subjects GetAllSubjects(string tenantId)
        {
            if (!String.IsNullOrEmpty(tenantId))
            {
                string fullUrl = $"{_urlPrefix}/tenants/{tenantId}/subjects";
                var request = new RestRequest(Method.GET);
                var response = ExecuteRequest(fullUrl, request);
                if (response.IsSuccessful && (response.StatusCode == HttpStatusCode.OK))
                {
                    return Subjects.FromJson(response.Content);
                }
                else
                {
                    throw new WebException(message: "Network error while getting all subjects: " + response.Content);
                }
            }
            else
            {
                throw new ArgumentException(message: "The tenant ID provided is null or empty.");
            }
        }

        /// <summary>
        /// Gets a subject given a subject UID
        /// </summary>
        /// <param name="subjectUid">Subject UID</param>
        /// <returns>Cogniac.Subject object</returns>
        public Subject GetSubject(string subjectUid)
        {
            if (!String.IsNullOrEmpty(subjectUid))
            {
                string fullUrl = $"{_urlPrefix}/subjects/{subjectUid}";
                var request = new RestRequest(Method.GET);
                var response = ExecuteRequest(fullUrl, request);
                if (response.IsSuccessful && (response.StatusCode == HttpStatusCode.OK))
                {
                    return Subject.FromJson(response.Content);
                }
                else
                {
                    throw new WebException(message: "Network error while getting subjects: " + response.Content);
                }
            }
            else
            {
                throw new ArgumentException(message: "The subject UID provided is null or empty.");
            }
        }

        /// <summary>
        /// Gets a list of applications given a tenant ID
        /// </summary>
        /// <param name="tenantId">Tenant ID</param>
        /// <returns>Cogniac.Applications object</returns>
        public Applications GetAllApplications(string tenantId)
        {
            if (!String.IsNullOrEmpty(tenantId))
            {
                string fullUrl = $"{_urlPrefix}/tenants/{tenantId}/applications";
                var request = new RestRequest(Method.GET);
                var response = ExecuteRequest(fullUrl, request);
                if (response.IsSuccessful && (response.StatusCode == HttpStatusCode.OK))
                {
                    return Applications.FromJson(response.Content);
                }
                else
                {
                    throw new WebException(message: "Network error while getting all applications: " + response.Content);
                }
            }
            else
            {
                throw new ArgumentException(message: "The tenant ID provided is null or empty.");
            }
        }

        /// <summary>
        /// Gets an applicatoin given an application ID
        /// </summary>
        /// <param name="applicationId">Application ID</param>
        /// <returns>Cogniac.Application object</returns>
        public Application GetApplication(string applicationId)
        {
            if (!String.IsNullOrEmpty(applicationId))
            {
                string fullUrl = $"{_urlPrefix}/applications/{applicationId}";
                var request = new RestRequest(Method.GET);
                var response = ExecuteRequest(fullUrl, request);
                if (response.IsSuccessful && (response.StatusCode == HttpStatusCode.OK))
                {
                    return Application.FromJson(response.Content);
                }
                else
                {
                    throw new WebException(message: "Network error while getting application: " + response.Content);
                }
            }
            else
            {
                throw new ArgumentException(message: "The application ID provided is null or empty.");
            }
        }

        /// <summary>
        /// Gets a tenant given a tenant ID
        /// </summary>
        /// <param name="tenantId">Tenant ID</param>
        /// <returns>Cogniac.Tenant object</returns>
        public Tenant GetTenant(string tenantId)
        {
            if (!String.IsNullOrEmpty(tenantId))
            {
                string fullUrl = $"{_urlPrefix}/tenants/{tenantId}";
                var request = new RestRequest(Method.GET);
                var response = ExecuteRequest(fullUrl, request);
                if (response.IsSuccessful && (response.StatusCode == HttpStatusCode.OK))
                {
                    return Tenant.FromJson(response.Content);
                }
                else
                {
                    throw new WebException(message: "Network error while getting tenant: " + response.Content);
                }
            }
            else
            {
                throw new ArgumentException(message: "The tenant ID provided is null or empty.");
            }
        }

        /// <summary>
        /// Associates a given media file with an exiting subject
        /// </summary>
        /// <param name="mediaId">Media ID</param>
        /// <param name="subjectUid">Subject UID</param>
        /// <param name="forceFeedback">Fore feedback flag</param>
        /// <param name="enableWaitResult">Enable wait flag</param>
        /// <param name="probability">Association probability, only valid if consensus is null</param>
        /// <param name="consensus">Consensus value</param>
        /// <returns>Cogniac.CaptureId object</returns>
        public CaptureId AssociateMediaToSubject(string mediaId, string subjectUid, bool forceFeedback = false, bool enableWaitResult = false,
            double? probability = null, bool? consensus = null)
        {
            if (string.IsNullOrEmpty(mediaId) || string.IsNullOrEmpty(subjectUid))
            {
                throw new ArgumentException(message: "Provided media ID or subject UID are is or empty.");
            }
            else
            {
                Dictionary<string, object> dict = new Dictionary<string, object>
                {
                    { "media_id", mediaId },
                    { "force_feedback", forceFeedback },
                    { "enable_wait_result", enableWaitResult }
                };
                // Perform some logic for the probability
                if (consensus == null)
                {
                    if (probability == null)
                    {
                        probability = 0.99;
                    }
                    // Insert it into the API call
                    dict.AddIfNotNull("uncal_prob", probability);
                }
                else
                {
                    dict.AddIfNotNull("consensus", consensus);
                }
                var request = new RestRequest(Method.POST);
                string data = Helpers.MapToQueryString(dict);
                IRestResponse response = ExecuteRequest($"{_urlPrefix}/subjects/{subjectUid}/media?{data}", request);
                if ((response.StatusCode == HttpStatusCode.OK) && (response.IsSuccessful == true))
                {
                    return CaptureId.FromJson(response.Content);
                }
                else
                {
                    throw new WebException(message: "Network error: " + response.Content);
                }
            }
        }

        /// <summary>
        /// Gets media given a media ID
        /// </summary>
        /// <param name="mediaId">Media ID</param>
        /// <returns>Cogniac.Media object</returns>
        public Media GetMedia(string mediaId)
        {
            if (!String.IsNullOrEmpty(mediaId))
            {
                string fullUrl = $"{_urlPrefix}/media/{mediaId}";
                var request = new RestRequest(Method.GET);
                var response = ExecuteRequest(fullUrl, request);
                if (response.IsSuccessful && (response.StatusCode == HttpStatusCode.OK))
                {
                    return Media.FromJson(response.Content);
                }
                else
                {
                    throw new WebException(message: "Network error while getting media: " + response.Content);
                }
            }
            else
            {
                throw new ArgumentException(message: "The media ID provided is null or empty.");
            }
        }

        /// <summary>
        /// Creates an application in the Cogniac system
        /// </summary>
        /// <param name="name">Application name</param>
        /// <param name="type">Application type</param>
        /// <param name="description">Description of application</param>
        /// <param name="inputSubjects">List of input subjects</param>
        /// <param name="outputSubjects">List of output subjects</param>
        /// <param name="releaseMetrics">Release metrics</param>
        /// <param name="detectionThresholds">Detection thresholds</param>
        /// <param name="detectionPostUrls">Detection post URL's</param>
        /// <param name="gatewayPostUrls">Gateway post URL's</param>
        /// <param name="active">Active flag</param>
        /// <param name="requestedFeedbackPerHour">Requested feedback per hour value</param>
        /// <param name="refreshFeedback">Refresh feedback flag</param>
        /// <param name="appManagers">List of application managers</param>
        /// <returns>Cogniac.Application object</returns>
        public Application CreateApplication
        (
            string name,
            string type,
            string description = null,
            string[] inputSubjects = null,
            string[] outputSubjects = null,
            string releaseMetrics = null,
            Dictionary<string, string> detectionThresholds = null,
            string[] detectionPostUrls = null,
            string[] gatewayPostUrls = null,
            bool active = false,
            int? requestedFeedbackPerHour = null,
            bool refreshFeedback = false,
            string[] appManagers = null
        )
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("message", nameof(name));
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentException("message", nameof(type));
            }
            Dictionary<string, object> dict = new Dictionary<string, object>
            {
                { "name", name },
                { "type", type },
            };
            dict.AddIfNotNull("description", description);
            dict.AddIfNotNull("input_subjects", Helpers.FlattenStringArray(inputSubjects));
            dict.AddIfNotNull("output_subjects", Helpers.FlattenStringArray(outputSubjects));
            dict.AddIfNotNull("release_metrics", releaseMetrics);
            dict.AddIfNotNull("detection_post_urls", Helpers.FlattenStringArray(detectionPostUrls));
            dict.AddIfNotNull("gateway_post_urls", Helpers.FlattenStringArray(gatewayPostUrls));
            dict.AddIfNotNull("active", active);
            dict.AddIfNotNull("requested_feedback_per_hour", requestedFeedbackPerHour);
            dict.AddIfNotNull("refresh_feedback", refreshFeedback);
            dict.AddIfNotNull("app_managers", Helpers.FlattenStringArray(appManagers));
            if (detectionThresholds != null)
            {
                dict.AddIfNotNull("detection_thresholds", detectionThresholds.ToJson());
            }
            string data = Helpers.MapToQueryString(dict);
            var request = new RestRequest(Method.POST)
            {
                AlwaysMultipartFormData = true
            };
            string fullUrl = $"{_urlPrefix}/applications?{data}";
            var response = ExecuteRequest(fullUrl, request);
            if (response.IsSuccessful && (response.StatusCode == HttpStatusCode.OK))
            {
                return Application.FromJson(response.Content);
            }
            else
            {
                throw new WebException(message: "Network error while creating application: " + response.Content);
            }
        }

        /// <summary>
        /// Creates a subject in the Cogniac system
        /// </summary>
        /// <param name="name">Subject name</param>
        /// <param name="description">Subject description</param>
        /// <param name="publicRead">Public read flag</param>
        /// <param name="publicWrite">Public write flag</param>
        /// <returns>Cogniac.Subject object</returns>
        public Subject CreateSubject
        (
            string name,
            string description = null,
            bool? publicRead = null,
            bool? publicWrite = null
        )
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("message", nameof(name));
            }
            Dictionary<string, object> dict = new Dictionary<string, object>
            {
                { "name", name }
            };
            dict.AddIfNotNull("description", description);
            dict.AddIfNotNull("public_read", publicRead);
            dict.AddIfNotNull("public_write", publicWrite);
            string data = Helpers.MapToQueryString(dict);
            var request = new RestRequest(Method.POST)
            {
                AlwaysMultipartFormData = true
            };
            string fullUrl = $"{_urlPrefix}/subjects?{data}";
            var response = ExecuteRequest(fullUrl, request);
            if (response.IsSuccessful && (response.StatusCode == HttpStatusCode.OK))
            {
                return Subject.FromJson(response.Content);
            }
            else
            {
                throw new WebException(message: "Network error while creating subject: " + response.Content);
            }
        }

        /// <summary>
        /// Delete a subject from the Cogniac system
        /// </summary>
        /// <param name="subjectUid">Subject UID</param>
        /// <returns>True on success</returns>
        public bool DeleteSubject(string subjectUid)
        {
            if (string.IsNullOrEmpty(subjectUid))
            {
                throw new ArgumentException("message", nameof(subjectUid));
            }
            string fullUrl = $"{_urlPrefix}/subjects/{subjectUid}";
            var request = new RestRequest(Method.DELETE);
            var response = ExecuteRequest(fullUrl, request);
            if (response.IsSuccessful && (response.StatusCode == HttpStatusCode.NoContent))
            {
                return true;
            }
            else
            {
                throw new WebException(message: "Error deleting subject: " + response.Content );
            }
        }

        /// <summary>
        /// Gets the subject media associations given a subject UID
        /// </summary>
        /// <param name="subjectUid">Subject UID</param>
        /// <returns>Cogniac.SubjectMediaAssociations object</returns>
        public SubjectMediaAssociations GetSubjectMediaAssociations(string subjectUid)
        {
            if (!String.IsNullOrEmpty(subjectUid))
            {
                string fullUrl = $"{_urlPrefix}/subjects/{subjectUid}/media";
                var request = new RestRequest(Method.GET);
                var response = ExecuteRequest(fullUrl, request);
                if (response.IsSuccessful && (response.StatusCode == HttpStatusCode.OK))
                {
                    return SubjectMediaAssociations.FromJson(response.Content);
                }
                else
                {
                    throw new WebException(message: "Network error while getting subject media associations: " + response.Content);
                }
            }
            else
            {
                throw new ArgumentException(message: "The subject UID provided is null or empty.");
            }
        }

        /// <summary>
        /// Get the subjects associated with a given media
        /// </summary>
        /// <param name="mediaId">Media ID</param>
        /// <returns>Cogniac.MediaSubjects</returns>
        public MediaSubjects GetMediaSubjects(string mediaId)
        {
            if (!String.IsNullOrEmpty(mediaId))
            {
                string fullUrl = $"{_urlPrefix}/media/{mediaId}/subjects";
                var request = new RestRequest(Method.GET);
                var response = ExecuteRequest(fullUrl, request);
                if (response.IsSuccessful && (response.StatusCode == HttpStatusCode.OK))
                {
                    return MediaSubjects.FromJson(response.Content);
                }
                else
                {
                    throw new WebException(message: "Network error while getting media subjects: " + response.Content);
                }
            }
            else
            {
                throw new ArgumentException(message: "The media ID provided is null or empty.");
            }
        }

        /// <summary>
        /// Get the detections associated with a media
        /// </summary>
        /// <param name="mediaId">Media ID</param>
        /// <param name="waitCaptureId">Wait capture ID</param>
        /// <returns>Cogniac.MediaDetections</returns>
        public MediaDetections GetMediaDetections(string mediaId, string waitCaptureId = null)
        {
            if (!String.IsNullOrEmpty(mediaId))
            {
                string fullUrl = "";
                if (string.IsNullOrEmpty(waitCaptureId))
                {
                    fullUrl = $"{_urlPrefix}/media/{mediaId}/detections";
                }
                else
                {
                    fullUrl = $"{_urlPrefix}/media/{mediaId}/detections?wait_capture_id={waitCaptureId}";
                }
                var request = new RestRequest(Method.GET);
                var response = ExecuteRequest(fullUrl, request);
                if (response.IsSuccessful && (response.StatusCode == HttpStatusCode.OK))
                {
                    return MediaDetections.FromJson(response.Content);
                }
                else
                {
                    throw new WebException(message: "Network error while getting media detections: " + response.Content);
                }
            }
            else
            {
                throw new ArgumentException(message: "The media ID provided is null or empty.");
            }
        }

        /// <summary>
        /// Update a given tenant
        /// </summary>
        /// <param name="tenantId">Tenant ID</param>
        /// <param name="name">Tenant name</param>
        /// <param name="description">Tenant description</param>
        /// <param name="azureSasTokens">Tenant Azure SAS token</param>
        /// <returns>Cogniac.Tenant</returns>
        public Tenant UpdateTenant
        (
            string tenantId,
            string name = null,
            string description = null,
            string azureSasTokens = null
        )
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentException("message", nameof(tenantId));
            }
            Dictionary<string, object> dict = new Dictionary<string, object> { };
            dict.AddIfNotNull("name", name);
            dict.AddIfNotNull("description", description);
            dict.AddIfNotNull("azure_sas_tokens", azureSasTokens);
            string data = Helpers.MapToQueryString(dict);
            var request = new RestRequest(Method.POST)
            {
                AlwaysMultipartFormData = true
            };
            string fullUrl = $"{_urlPrefix}/tenants/{tenantId}?{data}";
            var response = ExecuteRequest(fullUrl, request);
            if (response.IsSuccessful && (response.StatusCode == HttpStatusCode.OK))
            {
                return Tenant.FromJson(response.Content);
            }
            else
            {
                throw new WebException(message: "Network error while updating tenant: " + response.Content);
            }
        }
    } // End of class
}
