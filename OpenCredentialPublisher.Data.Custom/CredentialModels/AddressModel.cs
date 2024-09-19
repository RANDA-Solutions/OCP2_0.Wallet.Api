using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace OpenCredentialPublisher.Data.Custom.CredentialModels
{
    public class AddressModel
    {
        public AddressModel()
        {

        }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("type")]
        [Newtonsoft.Json.JsonConverter(typeof(Shared.Converters.Newtonsoft.SingleOrArrayConverter<string>))]
        [System.Text.Json.Serialization.JsonConverter(typeof(Shared.Converters.Json.SingleOrListConverter<string>))]
        public List<string> Type { get; set; }

        [JsonProperty("addressCountry", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("addressCountry")]
        public string AddressCountry { get; set; }

        [JsonProperty("addressCountryCode", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("addressCountryCode")]
        public string AddressCountryCode { get; set; }

        [JsonProperty("addressRegion", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("addressRegion")]
        public string AddressRegion { get; set; }

        [JsonProperty("addressLocality", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("addressLocality")]
        public string AddressLocality { get; set; }

        [JsonProperty("streetAddress", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("streetAddress")]
        public string StreetAddress { get; set; }

        [JsonProperty("postOfficeBoxNumber", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("postOfficeBoxNumber")]
        public string PostOfficeBoxNumber { get; set; }

        [JsonProperty("postalCode", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("geo", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("geo")]
        public GeoCoordinatesModel Geo { get; set; }
    }
}