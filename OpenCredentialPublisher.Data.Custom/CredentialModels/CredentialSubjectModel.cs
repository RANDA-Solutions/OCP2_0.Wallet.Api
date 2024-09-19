using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace OpenCredentialPublisher.Data.Custom.CredentialModels
{
    public abstract class CredentialSubjectModel
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("type")]
        [Newtonsoft.Json.JsonConverter(typeof(Shared.Converters.Newtonsoft.SingleOrArrayConverter<string>))]
        [System.Text.Json.Serialization.JsonConverter(typeof(Shared.Converters.Json.SingleOrListConverter<string>))]
        public List<string> Type { get; set; }
    }
}