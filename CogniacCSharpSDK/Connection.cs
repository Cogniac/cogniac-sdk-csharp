﻿using System;
using System.IO;
using System.Collections.Generic;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using System.Net;

namespace Cogniac
{
    public class Connection
    {
        private string _urlPrefix = "";
        private string _username = "";
        private string _password = "";
        private string _tenantId = "";
        private string _providedToken = "";
        private Auth _authObject = null;
        public Connection(string username = "", string password = "", string tenantId = "", string token = "", string urlPrefix = "https://api.cogniac.io/1")
        {
            _username = username;
            _password = password;
            _providedToken = token;
            _tenantId = tenantId;
            _urlPrefix = urlPrefix;            
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

        public Auth GetAuthObject()
        {
            return _authObject;
        }

        private void Authenticate()
        {
            string fullUrl = $"{_urlPrefix}/oauth/token?tenant_id={_tenantId}";
            _authObject = Auth.FromJson(ExecuteRequest(fullUrl).Content);
            IRestResponse resp = ExecuteRequest(fullUrl);
            try
            {
                _authObject = Auth.FromJson(resp.Content);
                _providedToken = _authObject.AccessToken;
            }
            catch
            {
                throw new ArgumentException(message: resp.Content);
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
            catch (Exception ex)
            {
                throw ex;
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

        public Media UploadMedia
        (
            string fileName = null,
            long mediaTimestamp = 0,
            string parentMediaId = null,
            string[] parentMediaIds = null,
            int frame = 0,
            bool forceOverwrite = true,
            string[] metaTags = null,
            bool isPublic = false,
            bool isVideo = false,
            string externalMediaId = null,
            string originalUrl = null,
            string originalLandingUrl = null,
            string license = null,
            string authorProfileUrl = null,
            string author = null,
            string title = null,
            string sourceUrl = null,
            string previewUrl = null,
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
                if (mediaTimestamp <= 0)
                {
                    if (!((fileName.ToLower().StartsWith("https://")) || (fileName.ToLower().StartsWith("http://"))))
                    {
                        mediaTimestamp = Helpers.ToUnixTime(File.GetCreationTime(fileName));
                    }
                }
                Dictionary<string, object> dict = new Dictionary<string, object>
                {
                    { "media_src", "c_sharp_sdk" },
                    { "media_fomat", mediaFormat },
                    { "media_timestamp", mediaTimestamp },
                    { "filename", fileName },
                    { "force_overwrite", forceOverwrite },
                    { "public", isPublic },
                    { "video", isVideo },
                };
                if (isVideo == true)
                {
                    dict.Add("frame", frame);
                }
                if (!String.IsNullOrEmpty(parentMediaId))
                {
                    dict.Add("parent_media_id", parentMediaId);
                }
                if (!(parentMediaIds == null))
                {
                    dict.Add("parent_media_ids", Helpers.FlattenStringArray(parentMediaIds));
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
                if (!String.IsNullOrEmpty(author))
                {
                    dict.Add("author", author);
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
                throw new ArgumentException(message: "The subject ID provided is null or empty.");
            }
        }

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

        public CaptureId AssociateMediaToSubject(string mediaId, string subjectUid, bool forceFeedback = false)
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
                    { "force_feedback", forceFeedback }
                };
                var request = new RestRequest(Method.POST);
                string data = Helpers.MapToQueryString(dict);
                IRestResponse response = ExecuteRequest($"{_urlPrefix}/subjects/{subjectUid}/media?{data}", request);
                Console.WriteLine(Serialize.ToJson(response));
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
    } // End of class
}
