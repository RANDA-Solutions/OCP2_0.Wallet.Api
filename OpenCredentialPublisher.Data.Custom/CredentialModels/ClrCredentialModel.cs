using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace OpenCredentialPublisher.Data.Custom.CredentialModels
{
    public class ClrCredentialModel : EndorsedCredentialModel
    {
        public ClrCredentialModel()
        {

        }

        [JsonProperty("credentialSubject", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("credentialSubject")]
        public new ClrSubjectModel CredentialSubject { get; set; }

        [JsonProperty("partial", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("partial")]
        public bool Partial { get; set; }

        [JsonProperty("awardedDate", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("awardedDate")]
        public string AwardedDate { get; set; }
    }
}