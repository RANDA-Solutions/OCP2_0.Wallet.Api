using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace OpenCredentialPublisher.Data.Custom.CredentialModels
{
    public class IdentifierEntryModel
    {
        public IdentifierEntryModel()
        {

        }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("identifier", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }

        [JsonProperty("identifierType", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("identifierType")]
        public string IdentifierType { get; set; }
    }
}
