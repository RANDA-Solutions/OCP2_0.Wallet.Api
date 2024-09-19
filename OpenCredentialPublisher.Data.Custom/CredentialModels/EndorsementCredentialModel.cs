using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace OpenCredentialPublisher.Data.Custom.CredentialModels
{
    public class EndorsementCredentialModel : VerifiableCredentialModel
    {
        public EndorsementCredentialModel()
        {

        }

        [JsonProperty("credentialSubject", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("credentialSubject")]
        public new EndorsementSubjectModel CredentialSubject { get; set; }

        [JsonProperty("awardedDate", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("awardedDate")]
        public string AwardedDate { get; set; }
    }
}