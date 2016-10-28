using Newtonsoft.Json;

namespace Integracao_Google_API_DOTNET.DTO
{
    public class PersonalServiceAccountCred
    {

        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("project_id")]
        public string projectId { get; set; }

        [JsonProperty("private_key_id")]
        public string privateKeyId { get; set; }

        [JsonProperty("private_key")]
        public string privateKey { get; set; }

        [JsonProperty("client_email")]
        public string clientEmail { get; set; }

        [JsonProperty("client_id")]
        public string clientId { get; set; }

        [JsonProperty("auth_uri")]
        public string authUri { get; set; }

        [JsonProperty("token_uri")]
        public string tokenUri { get; set; }

        [JsonProperty("auth_provider_x509_cert_url")]
        public string authProviderX509CertUrl { get; set; }

        [JsonProperty("client_x509_cert_url")]
        public string ClientX509CertUrl { get; set; }

    }
}