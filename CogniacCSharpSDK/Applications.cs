using Newtonsoft.Json;
using System.Collections.Generic;

namespace Cogniac
{
    public partial class Applications
    {
        [JsonProperty("data")]
        public Application[] Application { get; set; }
    }

    public partial class Application
    {
        [JsonProperty("refresh_feedback")]
        public bool? RefreshFeedback { get; set; }

        [JsonProperty("application_id")]
        public string ApplicationId { get; set; }

        [JsonProperty("validation_data_count")]
        public long? ValidationDataCount { get; set; }

        [JsonProperty("system_feedback_per_hour")]
        public long? SystemFeedbackPerHour { get; set; }

        [JsonProperty("app_managers")]
        public string[] AppManagers { get; set; }

        [JsonProperty("output_subjects_external_ids")]
        public object OutputSubjectsExternalIds { get; set; }

        [JsonProperty("replay")]
        public bool? Replay { get; set; }

        [JsonProperty("input_subjects")]
        public string[] InputSubjects { get; set; }

        [JsonProperty("last_released_at")]
        public double? LastReleasedAt { get; set; }

        [JsonProperty("custom_fields")]
        public object CustomFields { get; set; }

        [JsonProperty("release_model_count")]
        public long? ReleaseModelCount { get; set; }

        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }

        [JsonProperty("training_data_count")]
        public long? TrainingDataCount { get; set; }

        [JsonProperty("output_subjects")]
        public string[] OutputSubjects { get; set; }

        [JsonProperty("last_candidate_at")]
        public double? LastCandidateAt { get; set; }

        [JsonProperty("hpo_credit")]
        public long HpoCredit { get; set; }

        [JsonProperty("candidate_model_count")]
        public long CandidateModelCount { get; set; }

        [JsonProperty("release_metrics")]
        public string ReleaseMetrics { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("detection_post_line_users")]
        public string[] DetectionPostLineUsers { get; set; }

        [JsonProperty("active")]
        public bool? Active { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("created_at")]
        public double? CreatedAt { get; set; }

        [JsonProperty("modified_at")]
        public double? ModifiedAt { get; set; }

        [JsonProperty("gateway_post_urls")]
        public string[] GatewayPostUrls { get; set; }

        [JsonProperty("requested_feedback_per_hour")]
        public long? RequestedFeedbackPerHour { get; set; }

        [JsonProperty("tenant_id")]
        public string TenantId { get; set; }

        [JsonProperty("detection_thresholds")]
        public Dictionary<string, string> DetectionThresholds { get; set; }

        [JsonProperty("current_performance")]
        public double? CurrentPerformance { get; set; }

        [JsonProperty("detection_post_urls")]
        public string[] DetectionPostUrls { get; set; }

        [JsonProperty("best_model_ccp_filename")]
        public string BestModelCcpFilename { get; set; }
    }

    public partial class Application
    {
        public static Application FromJson(string json) => JsonConvert.DeserializeObject<Application>(json, Converter.Settings);
    }

    public partial class Applications
    {
        public static Applications FromJson(string json) => JsonConvert.DeserializeObject<Applications>(json, Converter.Settings);
    }
}
