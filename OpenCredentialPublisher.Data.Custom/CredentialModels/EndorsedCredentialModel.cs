using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace OpenCredentialPublisher.Data.Custom.CredentialModels
{
    public abstract class EndorsedCredentialModel : VerifiableCredentialModel
    {
        [JsonProperty("endorsement", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("endorsement")]
        public List<EndorsementCredentialModel> Endorsement { get; set; }

        [JsonProperty("endorsementJwt", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("endorsementJwt")]
        public List<string> EndorsementJwt { get; set; }
    }
}
