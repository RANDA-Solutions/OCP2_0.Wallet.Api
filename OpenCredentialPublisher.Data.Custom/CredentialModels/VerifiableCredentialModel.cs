using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace OpenCredentialPublisher.Data.Custom.CredentialModels
{
    public abstract class VerifiableCredentialModel : IJsonSerializable
    {

        [JsonProperty("@context", Order = 1, NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("@context")]
        public List<object> Context { get; set; }

        [JsonProperty("type", Order = 2, NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("type")]
        public List<string> Type { get; set; }

        [JsonProperty("id", Order = 3, NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("name", Order = 4, NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("description", Order = 5, NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonProperty("image", Order = 6, NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("image")]
        public ImageModel Image { get; set; }

        [JsonProperty("credentialSubject", Order = 7, NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("credentialSubject")]
        public virtual object CredentialSubject { get; set; }

        [JsonProperty("issuer", Order = 8, NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("issuer")]
        [Newtonsoft.Json.JsonConverter(typeof(Shared.Converters.Newtonsoft.StringOrObjectConverter<ProfileModel>))]
        [System.Text.Json.Serialization.JsonConverter(typeof(Shared.Converters.Json.StringOrObjectConverter<ProfileModel>))]
        public virtual ProfileModel Issuer { get; set; }

        [JsonProperty("validFrom", Order = 9, NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("validFrom")]
        public string ValidFrom { get; set; }

        [JsonProperty("validUntil", Order = 10, NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("validUntil")]
        public string ValidUntil { get; set; }

        [JsonProperty("proof", Order = 11, NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("proof")]
        [Newtonsoft.Json.JsonConverter(typeof(Shared.Converters.Newtonsoft.SingleOrArrayConverter<ProofModel>))]
        [System.Text.Json.Serialization.JsonConverter(typeof(Shared.Converters.Json.SingleOrListConverter<ProofModel>))]
        public List<ProofModel> Proof { get; set; }

        [JsonProperty("credentialSchema", Order = 12, NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("credentialSchema")]
        public List<BasicPropertiesModel> CredentialSchema { get; set; }

        [JsonProperty("credentialStatus", Order = 13, NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("credentialStatus")]
        public BasicPropertiesModel CredentialStatus { get; set; }

        [JsonProperty("refreshService", Order = 14, NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("refreshService")]
        public BasicPropertiesModel RefreshService { get; set; }

        [JsonProperty("termsOfUse", Order = 15, NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("termsOfUse")]
        public List<BasicPropertiesModel> TermsOfUse { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public string OriginalJson { get; set; }
    }
}
