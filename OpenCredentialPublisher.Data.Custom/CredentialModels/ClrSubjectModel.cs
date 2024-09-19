using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace OpenCredentialPublisher.Data.Custom.CredentialModels
{
    public class ClrSubjectModel : CredentialSubjectModel
    {
        public ClrSubjectModel()
        {

        }

        [JsonProperty("identifier", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("identifier")]
        public List<IdentityObjectModel> Identifier { get; set; }

        [JsonProperty("achievement", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("achievement")]
        public List<AchievementModel> Achievement { get; set; }

        [JsonProperty("verifiableCredential", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("verifiableCredential")]
        public List<VerifiableCredentialModel> VerifiableCredential { get; set; }

        [JsonProperty("association", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("association")]
        public List<AssociationModel> Association { get; set; }

        [System.Text.Json.Serialization.JsonExtensionData]
        [JsonPropertyName("additionalProperties")]
        [JsonProperty("additionalProperties", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> AdditionalProperties { get; set; }
    }
}