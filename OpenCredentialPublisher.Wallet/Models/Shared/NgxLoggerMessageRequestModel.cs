using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Wallet.Models.Shared
{
    public class NgxLoggerMessageRequestModel
    {
        [JsonPropertyName("level")]
        public int Level { get; set; }
        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }
        [JsonPropertyName("fileName")]
        public string FileName { get; set; }
        [JsonPropertyName("lineNumber")]
        public string LineNumber { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("additional")]
        public string[] Additional { get; set; }
    }
}
