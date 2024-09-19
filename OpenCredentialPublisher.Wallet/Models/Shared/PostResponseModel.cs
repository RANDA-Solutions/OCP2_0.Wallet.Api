using System.Text.Json.Serialization;
using OpenCredentialPublisher.Data.Abstracts;

namespace OpenCredentialPublisher.Wallet.Models.Shared
{
    public class PostResponseModel : GenericModel
    {
        [JsonPropertyName("value")]
        public object Value { get; set; }
    }
}
