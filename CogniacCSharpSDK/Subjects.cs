/*
    Copyright 2018 Cogniac Corporation.

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
    /// Cogniac.Subjects class
    /// </summary>
    public partial class Subjects
    {
        /// <summary>
        /// List of Cogniac.Subject objects
        /// </summary>
        [JsonProperty("data")]
        public Subject[] Subject { get; set; }
    }

    /// <summary>
    /// Cogniac.Subject class
    /// </summary>
    public partial class Subject
    {
        /// <summary>
        /// Subject UID
        /// </summary>
        [JsonProperty("subject_uid")]
        public string SubjectUid { get; set; }

        /// <summary>
        /// Subject description
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Created by time
        /// </summary>
        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Consensus summary
        /// </summary>
        [JsonProperty("consensus_summary")]
        public ConsensusSummary[] ConsensusSummary { get; set; }

        /// <summary>
        /// Expiration time
        /// </summary>
        [JsonProperty("expires_in")]
        public long? ExpiresIn { get; set; }

        /// <summary>
        /// Tenant ID
        /// </summary>
        [JsonProperty("tenant_id")]
        public string TenantId { get; set; }

        /// <summary>
        /// Created at time
        /// </summary>
        [JsonProperty("created_at")]
        public double? CreatedAt { get; set; }

        /// <summary>
        /// Modified at time
        /// </summary>
        [JsonProperty("modified_at")]
        public double? ModifiedAt { get; set; }

        /// <summary>
        /// Public write flag
        /// </summary>
        [JsonProperty("public_write")]
        public bool? PublicWrite { get; set; }

        /// <summary>
        /// External ID
        /// </summary>
        [JsonProperty("external_id")]
        public string ExternalId { get; set; }

        /// <summary>
        /// Public read flag
        /// </summary>
        [JsonProperty("public_read")]
        public bool? PublicRead { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Cogniac.ConsensusSummary class
    /// </summary>
    public partial class ConsensusSummary
    {
        /// <summary>
        /// Count
        /// </summary>
        [JsonProperty("count")]
        public long? Count { get; set; }

        /// <summary>
        /// Consensus
        /// </summary>
        [JsonProperty("consensus")]
        public string Consensus { get; set; }

        /// <summary>
        /// Application data type
        /// </summary>
        [JsonProperty("app_data_type")]
        public object AppDataType { get; set; }
    }

    public partial class Subjects
    {
        /// <summary>
        /// Build the Cogniac.Subjects object from a valid JSON string
        /// </summary>
        /// <param name="json">A valid JSON string</param>
        /// <returns>Cogniac.Subjects object</returns>
        public static Subjects FromJson(string json) 
            => JsonConvert.DeserializeObject<Subjects>(json, Converter.Settings);
    }

    public partial class Subject
    {
        /// <summary>
        /// Build the Cogniac.Subject object from a valid JSON string
        /// </summary>
        /// <param name="json">A valid JSON string</param>
        /// <returns>Cogniac.Subject object</returns>
        public static Subject FromJson(string json) 
            => JsonConvert.DeserializeObject<Subject>(json, Converter.Settings);
    }

}
