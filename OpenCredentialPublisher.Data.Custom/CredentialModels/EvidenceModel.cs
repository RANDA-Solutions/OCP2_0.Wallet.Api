using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace OpenCredentialPublisher.Data.Custom.CredentialModels
{
    public class EvidenceModel
    {
        public EvidenceModel()
        {

        }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("type")]
        [Newtonsoft.Json.JsonConverter(typeof(Shared.Converters.Newtonsoft.SingleOrArrayConverter<string>))]
        [System.Text.Json.Serialization.JsonConverter(typeof(Shared.Converters.Json.SingleOrListConverter<string>))]
        public List<string> Type { get; set; }

        [JsonProperty("narrative", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("narrative")]
        public string Narrative { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonProperty("genre", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("genre")]
        public string Genre { get; set; }

        [JsonProperty("audience", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("audience")]
        public string Audience { get; set; }
    }
}
