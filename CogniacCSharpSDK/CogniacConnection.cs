using System;
using System.IO;
using System.Collections.Generic;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using System.Web;
using System.Net;
using System.Text;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;

namespace CogniacCSharpSDK
{
    public class CogniacConnection
    {
        private string _urlPrefix = "";
        private string _username = "";
        private string _password = "";
        private string _tenantId = "";
        private string _providedToken = "";
        private CogniacAuthObject _authObject = null;
        public CogniacConnection(string username = "", string password = "", string tenantId = "", string token = "", string urlPrefix = "https://api.cogniac.io/1")
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
                throw new ArgumentException("urlPrefix parameter is null or empty.");
            }
        }

        public CogniacAuthObject GetCogniacAuthObject()
        {
            return _authObject;
        }

        private void Authenticate()
        {
            string fullUrl = _urlPrefix + "/oauth/token?tenant_id=" + _tenantId;
            _authObject = CogniacAuthObject.FromJson(ExecuteRequest(fullUrl).Content);
            IRestResponse resp = ExecuteRequest(fullUrl);
            try
            {
                _authObject = CogniacAuthObject.FromJson(resp.Content);
                _providedToken = _authObject.AccessToken;
            }
            catch
            {
                throw new ArgumentException(resp.Content);
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
                request.AddHeader("Authorization", "Bearer " + _providedToken);
            }
            else
            {
                client.Authenticator = new HttpBasicAuthenticator(_username, _password);
            }
            client.AddHandler("application/json", new JsonDeserializer());
            request.AddHeader("Cache-Control", "no-cache");
            return Retry.Do(() => client.Execute(request), TimeSpan.FromSeconds(5), 3);
        }

        public static CogniacTenantsObject GetAllAuthorizedTenants(string username = "", string password = "", string urlPrefix = "https://api.cogniac.io/1")
        {
            var client = new RestClient(urlPrefix + "/users/current/tenants");
            var request = new RestRequest(Method.GET);
            CogniacTenantsObject returnValue = null;
            IRestResponse resp = null;
            try
            {
                client.Authenticator = new HttpBasicAuthenticator(username, password);
                client.AddHandler("application/json", new JsonDeserializer());
                request.AddHeader("Cache-Control", "no-cache");
                resp = Retry.Do(() => client.Execute(request), TimeSpan.FromSeconds(5), 3);
                returnValue = CogniacTenantsObject.FromJson(resp.Content);
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
                    CogniacTenantsObject cto;
                    cto = GetAllAuthorizedTenants(_username, _password, urlPrefix: _urlPrefix);
                    if (cto.Tenants.Length.Equals(1))
                    {
                        _tenantId = cto.Tenants[0].TenantId;
                    }
                    else
                    {
                        throw new ArgumentException("Provided user has multiple or no tenants, please use GetAllAuthorizedTenants(...) first and supply a single valid tenantId parameter.");
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
                        throw new ArgumentException("Parameter 'userName' not provided!");
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
                        throw new ArgumentException("Parameter 'password' is not provided!");
                    }
                }
            }
        }

        public CogniacCreateMediaObject UploadMedia
        (
            string fileName,
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
                if (File.Exists(fileName))
                {
                    var request = new RestRequest(Method.POST);
                    request.AlwaysMultipartFormData = true;
                    string mediaFormat = Path.GetExtension(fileName);
                    mediaFormat = mediaFormat.Replace(".", string.Empty);
                    if (mediaTimestamp <= 0)
                    {
                        mediaTimestamp = Helpers.ToUnixTime(File.GetCreationTime(fileName));
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
                    if (!String.IsNullOrEmpty(sourceUrl))
                    {
                        dict.Add("source_url", sourceUrl);
                    }
                    if (!String.IsNullOrEmpty(previewUrl))
                    {
                        dict.Add("preview_url", previewUrl);
                    }
                    string data = Helpers.MapToQueryString(dict);
                    request.AddFile("fileData", Helpers.GetFileBytesContent(fileName), fileName);
                    IRestResponse response;
                    if (string.IsNullOrEmpty(localGatewayUrl))
                    {
                        response = ExecuteRequest(_urlPrefix + "/media?" + data, request);
                    }
                    else
                    {
                        response = ExecuteRequest(localGatewayUrl + "/media?" + data, request);
                    }
                    if ((response.StatusCode == HttpStatusCode.OK) && (response.IsSuccessful == true))
                    {
                        return CogniacCreateMediaObject.FromJson(response.Content);
                    }
                    else
                    {
                        throw new WebException("Network error.");
                    }
                }
                else
                {
                    throw new ArgumentException("File does not exist: '" + fileName + "'");
                }
            }
            else
            {
                throw new ArgumentException("Access token is null or empty");
            }
        }

    }
}
