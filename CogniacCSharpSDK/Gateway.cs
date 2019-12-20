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
    public class Gateway
    {
        private string _urlPrefix = "";

        /// <summary>
        /// Main class to establish a connection to a EdgeFlow gateway API.
        /// </summary>
        /// <param name="urlPrefix">EdgeFlow gateway URL Prefix</param>
        public Gateway(string urlPrefix)
        {
            _urlPrefix = urlPrefix;
         
            if (string.IsNullOrEmpty(_urlPrefix))
            {
                throw new ArgumentException(message: "urlPrefix parameter is null or empty.");
            }
        }

        private IRestResponse ExecuteRequest(string fullUrl, RestRequest request = null)
        {
            var client = new RestClient(fullUrl);
            if (request == null)
            {
                request = new RestRequest(Method.GET);
            }
            client.AddHandler("application/json", new JsonDeserializer());
            request.AddHeader("Cache-Control", "no-cache");
            return Retry.Do(() => client.Execute(request), TimeSpan.FromSeconds(5), 3);
        }

        /// <summary>
        /// Uploads media to a local EdgeFlow. Note that in the SDK, EdgeFlows 
        /// are referred to as gateways.
        /// </summary>
        /// <param name="fileName">File to upload</param>
        /// <param name="mediaTimestamp">Media time stamp</param>
        /// <param name="externalMediaId">External media ID</param>
        /// <param name="domainUnit">(E.G. serial number) for set assignment grouping</param>"
        /// <param name="postUrl">Source URL</param>
        /// <returns>Cogniac.MediaDetections object</returns>
        public MediaDetections ProcessMedia
        (
            string subjectUid,
            string fileName = null,
            double mediaTimestamp = 0,
            string externalMediaId = null,
            string domainUnit = null,
            string postUrl = null
        )
        {
            byte[] fileBytes;
            if (File.Exists(fileName))
            {
                fileBytes = Helpers.GetFileBytesContent(fileName);
            }
            else
            {
                throw new ArgumentException(message: "File does not exist: '" + fileName + "'");
            }
            if (String.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException(message: "No input file provided (fileName");
            }
            string mediaFormat = Path.GetExtension(fileName);
            mediaFormat = mediaFormat.Replace(".", string.Empty);
            Dictionary<string, object> dict = new Dictionary<string, object>
            {
            };
            if (mediaTimestamp > 0)
            {
                dict.Add("media_timestamp", mediaTimestamp);
            }
            if (!String.IsNullOrEmpty(externalMediaId))
            {
                dict.Add("external_media_id", externalMediaId);
            }
            if (!String.IsNullOrEmpty(domainUnit))
            {
                dict.Add("domain_unit", domainUnit);
            }
            if (!String.IsNullOrEmpty(postUrl))
            {
                dict.Add("post_url", postUrl);
            }
            string data = Helpers.MapToQueryString(dict);
            var request = new RestRequest(Method.POST)
            {
                AlwaysMultipartFormData = true
            };
            request.AddFile("fileData", fileBytes, fileName);
            IRestResponse response;
            if (string.IsNullOrEmpty(_urlPrefix))
            {
                throw new ArgumentException(message: "Gateway URL is null or empty");
            }
            else
            {
                response = ExecuteRequest($"{_urlPrefix}/process/{subjectUid}?{data}", request);
            }
            if ((response.StatusCode == HttpStatusCode.OK) && (response.IsSuccessful == true))
            {
                return MediaDetections.FromJson(response.Content);
            }
            else
            {
                throw new WebException(message: "Network error: " + response.Content);
            }
        }
    }
}
