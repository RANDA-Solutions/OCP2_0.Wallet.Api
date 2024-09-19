using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace OpenCredentialPublisher.Data.Custom.CredentialModels
{
    public class AchievementSubjectModel : CredentialSubjectModel
    {
        public AchievementSubjectModel()
        {

        }

        [JsonProperty("activityEndDate", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("activityEndDate")]
        [Newtonsoft.Json.JsonConverter(typeof(Shared.Converters.Newtonsoft.DateConverter<DateTime>), new object[] { "o" })]
        [System.Text.Json.Serialization.JsonConverter(typeof(Shared.Converters.Json.DateConverter))]
        public DateTime? ActivityEndDate { get; set; }

        [JsonProperty("activityStartDate", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("activityStartDate")]
        [Newtonsoft.Json.JsonConverter(typeof(Shared.Converters.Newtonsoft.DateConverter<DateTime>), new object[] { "o" })]
        [System.Text.Json.Serialization.JsonConverter(typeof(Shared.Converters.Json.DateConverter))]
        public DateTime? ActivityStartDate { get; set; }

        [JsonProperty("creditsEarned", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("creditsEarned")]
        public float? CreditsEarned { get; set; }

        [JsonProperty("achievement", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("achievement")]
        public AchievementModel Achievement { get; set; }

        [JsonProperty("identifier", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("identifier")]
        public List<IdentityObjectModel> Identifier { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("image")]
        public ImageModel Image { get; set; }

        [JsonProperty("licenseNumber", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("licenseNumber")]
        public string LicenseNumber { get; set; }

        [JsonProperty("narrative", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("narrative")]
        public string Narrative { get; set; }

        [JsonProperty("result", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("result")]
        public List<ResultModel> Result { get; set; }

        [JsonProperty("role", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonProperty("source", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("source")]
        public ProfileModel Source { get; set; }

        [JsonProperty("term", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("term")]
        public string Term { get; set; }
    }
}
