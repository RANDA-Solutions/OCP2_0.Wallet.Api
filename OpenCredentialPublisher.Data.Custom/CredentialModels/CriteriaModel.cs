using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace OpenCredentialPublisher.Data.Custom.CredentialModels
{
    public class CriteriaModel
    {
        public CriteriaModel()
        {

        }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("narrative", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("narrative")]
        public string Narrative { get; set; }
    }
}