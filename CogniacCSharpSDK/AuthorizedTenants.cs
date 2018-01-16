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
        [JsonProperty("tenant_id")]
        public string TenantId { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

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
        /// Created by
        /// </summary>
        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }
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
