using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace OpenCredentialPublisher.Data.Custom.CredentialModels
{
    public class IdentityObjectModel
    {
        public IdentityObjectModel()
        {

        }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("type")]
        public string Type { get; set; } = nameof(IdentityObjectModel);

        [JsonProperty("hashed", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("hashed")]
        public bool Hashed { get; set; }

        [JsonProperty("identityHash", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("identityHash")]
        public string IdentityHash { get; set; }

        [JsonProperty("identityType", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("identityType")]
        public string IdentityType { get; set; }

        [JsonProperty("salt", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("salt")]
        public string Salt { get; set; }
    }
}
