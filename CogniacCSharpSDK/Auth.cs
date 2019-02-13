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

using Newtonsoft.Json;

namespace Cogniac
{
    /// <summary>
    /// Cogniac.Auth class
    /// </summary>
    partial class Auth
    {
        /// <summary>
        /// User ID
        /// </summary>
        [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
        public string UserId { get; set; }

        /// <summary>
        /// Access token
        /// </summary>
        [JsonProperty("access_token", NullValueHandling = NullValueHandling.Ignore)]
        public string AccessToken { get; set; }

        /// <summary>
        /// Tenant ID
        /// </summary>
        [JsonProperty("tenant_id", NullValueHandling = NullValueHandling.Ignore)]
        public string TenantId { get; set; }

        /// <summary>
        /// Expiration time
        /// </summary>
        [JsonProperty("expires_in", NullValueHandling = NullValueHandling.Ignore)]
        public long? ExpiresIn { get; set; }

        /// <summary>
        /// Token type
        /// </summary>
        [JsonProperty("token_type", NullValueHandling = NullValueHandling.Ignore)]
        public string TokenType { get; set; }

        /// <summary>
        /// Tenant name
        /// </summary>
        [JsonProperty("tenant_name", NullValueHandling = NullValueHandling.Ignore)]
        public string TenantName { get; set; }

        /// <summary>
        /// User email address
        /// </summary>
        [JsonProperty("user_email", NullValueHandling = NullValueHandling.Ignore)]
        public string UserEmail { get; set; }
    }

    public partial class Auth
    {
        /// <summary>
        /// Build the Cogniac.Auth object from a valid JSON string
        /// </summary>
        /// <param name="json">A valid JSON string</param>
        /// <returns>Cogniac.Auth object</returns>
        public static Auth FromJson(string json)
            => JsonConvert.DeserializeObject<Auth>(json, Converter.Settings);
    }
}
