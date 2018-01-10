using Newtonsoft.Json;

namespace CogniacCSharpSDK
{
    public partial class CogniacTenantsObject
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
    }

    public partial class CogniacTenantsObject
    {
        public static CogniacTenantsObject FromJson(string json)
            => JsonConvert.DeserializeObject<CogniacTenantsObject>(json, Converter.Settings);
    }
}
