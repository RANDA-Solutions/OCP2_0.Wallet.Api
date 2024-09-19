using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace OpenCredentialPublisher.Data.Custom.CredentialModels
{
    public class GeoCoordinatesModel
    {
        public GeoCoordinatesModel()
        {

        }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("latitude", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("longitude")]
        public string Longitude { get; set; }
    }
}
