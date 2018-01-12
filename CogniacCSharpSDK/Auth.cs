using Newtonsoft.Json;

namespace Cogniac
{
    partial class Auth
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("tenant_id")]
        public string TenantId { get; set; }

        [JsonProperty("expires_in")]
        public long? ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("tenant_name")]
        public string TenantName { get; set; }

        [JsonProperty("user_email")]
        public string UserEmail { get; set; }
    }

    public partial class Auth
    {
        public static Auth FromJson(string json)
            => JsonConvert.DeserializeObject<Auth>(json, Converter.Settings);
    }
}
