using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace OpenCredentialPublisher.Data.Custom.CredentialModels
{
    public class AchievementCredentialModel : EndorsedCredentialModel
    {

        public AchievementCredentialModel()
        {

        }

        [JsonProperty("credentialSubject", Order = 7, NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("credentialSubject")]
        public new AchievementSubjectModel CredentialSubject { get; set; }

        [JsonProperty("awardedDate", Order = 98, NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("awardedDate")]
        public string AwardedDate { get; set; }

        [JsonProperty("evidence", NullValueHandling = NullValueHandling.Ignore, Order = 99)]
        [JsonPropertyName("evidence")]
        public List<EvidenceModel> Evidence { get; set; }
    }
}