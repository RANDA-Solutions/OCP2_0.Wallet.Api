using System.Collections.Immutable;
using OpenCredentialPublisher.Data.Custom.Results;

namespace OpenCredentialPublisher.Wallet.Models.PublicShares
{
    public record PublicShareDetailResponseModel
    {
        protected PublicShareDetailResponseModel(PublicShareDetailResult result, string hash, string code)
        {
            ShareId = result.Share.ShareId;
            Hash = hash;
            Code = code;
            DisplayName = result.Share.User.DisplayName ?? result.Share.User.Email;
            VerifiableCredentialIds = result.VerifiableCredentialIds ?? ImmutableList.Create<long>();
        }

        public long ShareId { get; }
        public string Hash { get; set; }
        public string Code { get; set; }
        public string DisplayName { get; set; }
        public IImmutableList<long> VerifiableCredentialIds { get; set; }

        public static PublicShareDetailResponseModel FromResultsModel(PublicShareDetailResult result, string hash, string code)
        {
            return new PublicShareDetailResponseModel(result, hash, code);
        }
    }
}