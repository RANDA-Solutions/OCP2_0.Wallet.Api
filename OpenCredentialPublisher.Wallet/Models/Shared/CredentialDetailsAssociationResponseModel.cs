using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Wallet.Models.Shared
{
    public record CredentialDetailsAssociationResponseModel
    {
        public string AchievementName { get; set; }
        public long VerifiableCredentialId { get; set; }

        protected CredentialDetailsAssociationResponseModel(VerifiableCredential verifiableCredential)
        {
            AchievementName = verifiableCredential.Name;
            VerifiableCredentialId = verifiableCredential.VerifiableCredentialId;
        }

        public static CredentialDetailsAssociationResponseModel FromModel(VerifiableCredential verifiableCredential)
        {
            return new CredentialDetailsAssociationResponseModel(verifiableCredential);
        }
    }
}