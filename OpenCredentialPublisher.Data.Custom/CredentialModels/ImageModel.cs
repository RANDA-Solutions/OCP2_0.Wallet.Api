using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace OpenCredentialPublisher.Data.Custom.CredentialModels
{
    public class ImageModel
    {
        public ImageModel()
        {

        }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("caption", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("caption")]
        public string Caption { get; set; }
    }
}
