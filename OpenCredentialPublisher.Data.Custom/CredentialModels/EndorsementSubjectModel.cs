using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace OpenCredentialPublisher.Data.Custom.CredentialModels
{
    public class EndorsementSubjectModel : CredentialSubjectModel
    {
        public EndorsementSubjectModel()
        {

        }

        [JsonProperty("endorsementComment", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("endorsementComment")]
        public string EndorsementComment { get; set; }
    }
}
