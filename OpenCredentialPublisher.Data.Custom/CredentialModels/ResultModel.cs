using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace OpenCredentialPublisher.Data.Custom.CredentialModels
{
    public class ResultModel
    {
        public ResultModel()
        {

        }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("type")]
        [Newtonsoft.Json.JsonConverter(typeof(Shared.Converters.Newtonsoft.SingleOrArrayConverter<string>))]
        [System.Text.Json.Serialization.JsonConverter(typeof(Shared.Converters.Json.SingleOrListConverter<string>))]
        public List<string> Type { get; set; }

        [JsonProperty("achievedLevel", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("achievedLevel")]
        public string AchievedLevel { get; set; }

        [JsonProperty("alignment", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("alignment")]
        public List<AlignmentModel> Alignment { get; set; }

        [JsonProperty("resultDescription", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("resultDescription")]
        public string ResultDescription { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}