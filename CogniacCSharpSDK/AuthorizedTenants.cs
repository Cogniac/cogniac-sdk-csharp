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
    /// Cogniac.AuthorizedTenants class
    /// </summary>
    public partial class AuthorizedTenants
    {
        /// <summary>
        /// List of Cogniac.Tenant objects
        /// </summary>
        [JsonProperty("tenants")]
        public Tenant[] Tenants { get; set; }
    }

    /// <summary>
    /// Cogniac.Tenant class
    /// </summary>
    public partial class Tenant
    {
        /// <summary>
        /// Tenant ID
        /// </summary>
        [JsonProperty("tenant_id", NullValueHandling = NullValueHandling.Ignore)]
        public string TenantId { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        /// <summary>
        /// Created time
        /// </summary>
        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public double? CreatedAt { get; set; }

        /// <summary>
        /// Modified time
        /// </summary>
        [JsonProperty("modified_at", NullValueHandling = NullValueHandling.Ignore)]
        public double? ModifiedAt { get; set; }

        /// <summary>
        /// Created by
        /// </summary>
        [JsonProperty("created_by", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Beta Object
        /// </summary>
        [JsonProperty("beta")]
        public object Beta { get; set; }

        /// <summary>
        /// Azure SAS Tokens
        /// </summary>
        [JsonProperty("azure_sas_tokens")]
        public object AzureSasTokens { get; set; }

        /// <summary>
        /// Custom Data
        /// </summary>
        [JsonProperty("custom_data")]
        public object CustomData { get; set; }

        /// <summary>
        /// Region
        /// </summary>
        [JsonProperty("region")]
        public object Region { get; set; }
    }

    public partial class Tenant
    {
        /// <summary>
        /// Build the Cogniac.Tenant object from a valid JSON string
        /// </summary>
        /// <param name="json">A valid JSON string</param>
        /// <returns>Cogniac.Tenant object</returns>
        public static Tenant FromJson(string json)
            => JsonConvert.DeserializeObject<Tenant>(json, Converter.Settings);
    }

    public partial class AuthorizedTenants
    {
        /// <summary>
        /// Build the Cogniac.AuthorizedTenants object from a valid JSON string
        /// </summary>
        /// <param name="json">A valid JSON string</param>
        /// <returns>Cogniac.AuthorizedTenants object</returns>
        public static AuthorizedTenants FromJson(string json)
            => JsonConvert.DeserializeObject<AuthorizedTenants>(json, Converter.Settings);
    }
}
