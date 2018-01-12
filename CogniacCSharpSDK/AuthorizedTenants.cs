using Newtonsoft.Json;

namespace Cogniac
{
    public partial class AuthorizedTenants
    {
        [JsonProperty("tenants")]
        public Tenant[] Tenants { get; set; }
    }

    public partial class Tenant
    {
        [JsonProperty("tenant_id")]
        public string TenantId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("created_at")]
        public double? CreatedAt { get; set; }

        [JsonProperty("modified_at")]
        public double? ModifiedAt { get; set; }

        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }
    }

    public partial class Tenant
    {
        public static Tenant FromJson(string json)
            => JsonConvert.DeserializeObject<Tenant>(json, Converter.Settings);
    }

    public partial class AuthorizedTenants
    {
        public static AuthorizedTenants FromJson(string json)
            => JsonConvert.DeserializeObject<AuthorizedTenants>(json, Converter.Settings);
    }
}
