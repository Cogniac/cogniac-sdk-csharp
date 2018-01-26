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
using System.Collections.Generic;

namespace Cogniac
{
    /// <summary>
    /// Cogniac.Applications class
    /// </summary>
    public partial class Applications
    {
        /// <summary>
        /// List of Cogniac.Application objects
        /// </summary>
        [JsonProperty("data")]
        public Application[] Application { get; set; }
    }

    /// <summary>
    /// Cogniac.Application class
    /// </summary>
    public partial class Application
    {
        /// <summary>
        /// Refresh feedback flag
        /// </summary>
        [JsonProperty("refresh_feedback")]
        public bool? RefreshFeedback { get; set; }

        /// <summary>
        /// Application ID
        /// </summary>
        [JsonProperty("application_id")]
        public string ApplicationId { get; set; }

        /// <summary>
        /// Validation data count
        /// </summary>
        [JsonProperty("validation_data_count")]
        public long? ValidationDataCount { get; set; }

        /// <summary>
        /// System feedback per hour
        /// </summary>
        [JsonProperty("system_feedback_per_hour")]
        public long? SystemFeedbackPerHour { get; set; }

        /// <summary>
        /// Application managers
        /// </summary>
        [JsonProperty("app_managers")]
        public string[] AppManagers { get; set; }

        /// <summary>
        /// Output subjects external ID's
        /// </summary>
        [JsonProperty("output_subjects_external_ids")]
        public object OutputSubjectsExternalIds { get; set; }

       /// <summary>
       /// Replay flag
       /// </summary>
        [JsonProperty("replay")]
        public bool? Replay { get; set; }

        /// <summary>
        /// Input subjects
        /// </summary>
        [JsonProperty("input_subjects")]
        public string[] InputSubjects { get; set; }

        /// <summary>
        /// Last released time
        /// </summary>
        [JsonProperty("last_released_at")]
        public double? LastReleasedAt { get; set; }

        /// <summary>
        /// Custom fields
        /// </summary>
        [JsonProperty("custom_fields")]
        public object CustomFields { get; set; }

        /// <summary>
        /// Release model count
        /// </summary>
        [JsonProperty("release_model_count")]
        public long? ReleaseModelCount { get; set; }

        /// <summary>
        /// Created by
        /// </summary>
        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Training data count
        /// </summary>
        [JsonProperty("training_data_count")]
        public long? TrainingDataCount { get; set; }

        /// <summary>
        /// Output subjects
        /// </summary>
        [JsonProperty("output_subjects")]
        public string[] OutputSubjects { get; set; }

        /// <summary>
        /// Last candidate time
        /// </summary>
        [JsonProperty("last_candidate_at")]
        public double? LastCandidateAt { get; set; }

        /// <summary>
        /// HPO credit
        /// </summary>
        [JsonProperty("hpo_credit")]
        public long? HpoCredit { get; set; }

        /// <summary>
        /// Candidate model count
        /// </summary>
        [JsonProperty("candidate_model_count")]
        public long? CandidateModelCount { get; set; }

        /// <summary>
        /// Release metrics
        /// </summary>
        [JsonProperty("release_metrics")]
        public string ReleaseMetrics { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Detection post line users
        /// </summary>
        [JsonProperty("detection_post_line_users")]
        public string[] DetectionPostLineUsers { get; set; }

        /// <summary>
        /// Active flag
        /// </summary>
        [JsonProperty("active")]
        public bool? Active { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Created time
        /// </summary>
        [JsonProperty("created_at")]
        public double? CreatedAt { get; set; }

        /// <summary>
        /// Modified time
        /// </summary>
        [JsonProperty("modified_at")]
        public double? ModifiedAt { get; set; }

        /// <summary>
        /// Gateway post URL's
        /// </summary>
        [JsonProperty("gateway_post_urls")]
        public string[] GatewayPostUrls { get; set; }

        /// <summary>
        /// Requested feedback per hour count
        /// </summary>
        [JsonProperty("requested_feedback_per_hour")]
        public long? RequestedFeedbackPerHour { get; set; }

        /// <summary>
        /// Tenant ID
        /// </summary>
        [JsonProperty("tenant_id")]
        public string TenantId { get; set; }

        /// <summary>
        /// Detection thresholds
        /// </summary>
        [JsonProperty("detection_thresholds")]
        public Dictionary<string, string> DetectionThresholds { get; set; }

        /// <summary>
        /// Current performance
        /// </summary>
        [JsonProperty("current_performance")]
        public double? CurrentPerformance { get; set; }

        /// <summary>
        /// Detection post URL's
        /// </summary>
        [JsonProperty("detection_post_urls")]
        public string[] DetectionPostUrls { get; set; }

        /// <summary>
        /// Best model CCP filename
        /// </summary>
        [JsonProperty("best_model_ccp_filename")]
        public string BestModelCcpFilename { get; set; }
    }

    public partial class Application
    {
        /// <summary>
        /// Build the Cogniac.Application object from a valid JSON string
        /// </summary>
        /// <param name="json">A valid JSON string</param>
        /// <returns>Cogniac.Application object</returns>
        public static Application FromJson(string json) => JsonConvert.DeserializeObject<Application>(json, Converter.Settings);
    }

    public partial class Applications
    {
        /// <summary>
        /// Build the Cogniac.Applications object from a valid JSON string
        /// </summary>
        /// <param name="json">A valid JSON string</param>
        /// <returns>Cogniac.Applications object</returns>
        public static Applications FromJson(string json) 
            => JsonConvert.DeserializeObject<Applications>(json, Converter.Settings);
    }
}
