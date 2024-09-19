namespace OpenCredentialPublisher.Wallet.Models.Credentials
{
    public record CredentialsSearchRequestModel(string Keywords, string IssuerName, string AchievementType, int? EffectiveAtYear);
}
