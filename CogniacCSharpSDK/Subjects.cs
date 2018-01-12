using Newtonsoft.Json;

namespace Cogniac
{
    public partial class Subjects
    {
        [JsonProperty("data")]
        public Subject[] Subject { get; set; }
    }

    public partial class Subject
    {
        [JsonProperty("subject_uid")]
        public string SubjectUid { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }

        [JsonProperty("consensus_summary")]
        public ConsensusSummary[] ConsensusSummary { get; set; }

        [JsonProperty("expires_in")]
        public long? ExpiresIn { get; set; }

        [JsonProperty("tenant_id")]
        public string TenantId { get; set; }

        [JsonProperty("created_at")]
        public double? CreatedAt { get; set; }

        [JsonProperty("modified_at")]
        public double? ModifiedAt { get; set; }

        [JsonProperty("public_write")]
        public bool? PublicWrite { get; set; }

        [JsonProperty("external_id")]
        public string ExternalId { get; set; }

        [JsonProperty("public_read")]
        public bool? PublicRead { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class ConsensusSummary
    {
        [JsonProperty("count")]
        public long? Count { get; set; }

        [JsonProperty("consensus")]
        public string Consensus { get; set; }

        [JsonProperty("app_data_type")]
        public object AppDataType { get; set; }
    }

    public partial class Subjects
    {
        public static Subjects FromJson(string json) => JsonConvert.DeserializeObject<Subjects>(json, Converter.Settings);
    }

    public partial class Subject
    {
        public static Subject FromJson(string json) => JsonConvert.DeserializeObject<Subject>(json, Converter.Settings);
    }

}
