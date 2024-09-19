namespace OpenCredentialPublisher.Wallet.Models.Packages
{
    public record PackagesSearchRequestModel(string Keywords, string IssuerName, string AchievementType, int? EffectiveAtYear);
}