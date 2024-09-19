using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace OpenCredentialPublisher.Data.Custom.CredentialModels
{
    public class AssociationModel
    {
        public AssociationModel()
        {

        }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("associationType", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("associationType")]
        [Newtonsoft.Json.JsonConverter(typeof(Shared.Converters.Newtonsoft.EnumAsStringConverter<AssociationTypeEnum>))]
        public AssociationTypeEnum AssociationType { get; set; }

        [JsonProperty("sourceId", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("sourceId")]
        public string SourceId { get; set; }

        [JsonProperty("targetId", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("targetId")]
        public string TargetId { get; set; }
    }
}
