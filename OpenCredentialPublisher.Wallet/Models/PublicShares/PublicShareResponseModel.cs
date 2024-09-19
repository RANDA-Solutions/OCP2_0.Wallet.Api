using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Wallet.Models.PublicShares
{
    public record PublicShareResponseModel
    {
        protected PublicShareResponseModel(Share share, string hash)
        {
            ShareId = share.ShareId;
            Hash = hash;
            DisplayName = share.User.DisplayName;
        }

        public long ShareId { get; }
        public string Hash { get; set; }
        public string DisplayName { get; set; }

        public static PublicShareResponseModel FromModel(Share share, string hash)
        {
            return new PublicShareResponseModel(share, hash);
        }
    }
}
