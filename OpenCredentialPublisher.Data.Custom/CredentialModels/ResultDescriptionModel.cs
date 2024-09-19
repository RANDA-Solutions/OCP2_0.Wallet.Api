using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using OpenCredentialPublisher.Shared.Converters.Json;

namespace OpenCredentialPublisher.Data.Custom.CredentialModels
{
    public class ResultDescriptionModel
    {
        public ResultDescriptionModel()
        {

        }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("type")]
        [Newtonsoft.Json.JsonConverter(typeof(Shared.Converters.Newtonsoft.SingleOrArrayConverter<string>))]
        [System.Text.Json.Serialization.JsonConverter(typeof(Shared.Converters.Json.SingleOrListConverter<string>))]
        public List<string> Type { get; set; } = new List<string> { nameof(ResultDescriptionModel) };

        [JsonProperty("alignment", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("alignment")]
        public List<AlignmentModel> Alignment { get; set; }

        [JsonProperty("allowedValue", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("allowedValue")]
        public List<string> AllowedValue { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("requiredLevel", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("requiredLevel")]
        public string RequiredLevel { get; set; }

        [JsonProperty("requiredValue", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("requiredValue")]
        public string RequiredValue { get; set; }

        [JsonProperty("resultType", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("resultType")]

        [Newtonsoft.Json.JsonConverter(typeof(Shared.Converters.Newtonsoft.EnumExtStringConverter<ResultTypeEnum>))]
        [System.Text.Json.Serialization.JsonConverter(typeof(EnumExtStringConverter<ResultTypeEnum>))]
        public string ResultType { get; set; }

        [JsonProperty("rubricCriterionLevel", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("rubricCriterionLevel")]
        public List<RubricCriterionLevelModel> RubricCriterionLevel { get; set; }

        [JsonProperty("valueMax", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("valueMax")]
        public string ValueMax { get; set; }

        [JsonProperty("valueMin", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("valueMin")]
        public string ValueMin { get; set; }
    }
}
