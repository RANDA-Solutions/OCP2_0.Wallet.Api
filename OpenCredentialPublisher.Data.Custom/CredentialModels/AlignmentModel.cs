using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using OpenCredentialPublisher.Shared.Converters.Json;

namespace OpenCredentialPublisher.Data.Custom.CredentialModels
{
    public class AlignmentModel
    {
        public AlignmentModel()
        {

        }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("type")]
        [Newtonsoft.Json.JsonConverter(typeof(Shared.Converters.Newtonsoft.SingleOrArrayConverter<string>))]
        [System.Text.Json.Serialization.JsonConverter(typeof(SingleStringOrSingleOrListConverter<string>))]
        public List<string> Type { get; set; } = new List<string> { nameof(AlignmentModel) };

        [JsonProperty("targetCode", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("targetCode")]
        public string TargetCode { get; set; }

        [JsonProperty("targetDescription", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("targetDescription")]
        public string TargetDescription { get; set; }

        [JsonProperty("targetName", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("targetName")]
        public string TargetName { get; set; }

        [JsonProperty("targetFramework", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("targetFramework")]
        public string TargetFramework { get; set; }

        [JsonProperty("targetType", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("targetType")]
        public string TargetType { get; set; }

        [JsonProperty("targetUrl", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("targetUrl")]
        public string TargetUrl { get; set; }
    }
}