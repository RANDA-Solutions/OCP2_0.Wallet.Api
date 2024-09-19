using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using OpenCredentialPublisher.Shared.Converters.Json;

namespace OpenCredentialPublisher.Data.Custom.CredentialModels
{
    public class AchievementModel
    {
        public AchievementModel()
        {

        }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("type")]
        [System.Text.Json.Serialization.JsonConverter(typeof(SingleStringOrSingleOrListConverter<string>))]
        public List<string> Type { get; set; }

        [JsonProperty("alignment", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("alignment")]
        public List<AlignmentModel> Alignment { get; set; }

        [JsonProperty("achievementType", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("achievementType")]
        [Newtonsoft.Json.JsonConverter(typeof(Shared.Converters.Newtonsoft.EnumExtStringConverter<AchievementTypeEnum>))]
        [System.Text.Json.Serialization.JsonConverter(typeof(EnumExtStringConverter<AchievementTypeEnum>))]
        public string AchievementType { get; set; }

        [JsonProperty("creator", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("creator")]
        public ProfileModel Creator { get; set; }

        [JsonProperty("creditsAvailable", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("creditsAvailable")]
        public float? CreditsAvailable { get; set; }

        [JsonProperty("criteria", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("criteria")]
        public CriteriaModel Criteria { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonProperty("endorsement", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("endorsement")]
        public List<EndorsementCredentialModel> Endorsement { get; set; }

        [JsonProperty("endorsementJwt", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("endorsementJwt")]
        public List<string> EndorsementJwt { get; set; }

        [JsonProperty("fieldOfStudy", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("fieldOfStudy")]
        public string FieldOfStudy { get; set; }

        [JsonProperty("humanCode", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("humanCode")]
        public string HumanCode { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("image")]
        public ImageModel Image { get; set; }

        [JsonProperty("@language", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("@language")]
        public string Language { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("otherIdentifier", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("otherIdentifier")]
        public List<IdentifierEntryModel> OtherIdentifier { get; set; }

        [JsonProperty("related", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("related")]
        public List<RelatedModel> Related { get; set; }

        [JsonProperty("resultDescription", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("resultDescription")]
        public List<ResultDescriptionModel> ResultDescription { get; set; }

        [JsonProperty("specialization", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("specialization")]
        public string Specialization { get; set; }

        [JsonProperty("tag", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("tag")]
        public List<string> Tag { get; set; }

        [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("version")]
        public string Version { get; set; }
    }
}
