using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace OpenCredentialPublisher.Data.Custom.CredentialModels
{
    public class RubricCriterionLevelModel
    {
        public RubricCriterionLevelModel()
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

        [JsonProperty("alignment", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("alignment")]
        public List<AlignmentModel> Alignment { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonProperty("level", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("level")]
        public string Level { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("points", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("points")]
        public string Points { get; set; }
    }
}
