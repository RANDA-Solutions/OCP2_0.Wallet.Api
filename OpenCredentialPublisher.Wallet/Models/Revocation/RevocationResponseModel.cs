using OpenCredentialPublisher.Data.Custom.Results;

namespace OpenCredentialPublisher.Wallet.Models.Revocation
{
    public record RevocationResponseModel
    {
        protected RevocationResponseModel(RevocationResult revocationResult)
        {
            IsRevoked = revocationResult?.IsRevoked ?? false;
            RevokedReason = revocationResult?.RevokedReason;
        }

        public bool IsRevoked { get; set; }
        public string RevokedReason { get; set; }

        public static RevocationResponseModel FromModel(RevocationResult revocationResult)
        {
            return new RevocationResponseModel(revocationResult);
        }
    }
}