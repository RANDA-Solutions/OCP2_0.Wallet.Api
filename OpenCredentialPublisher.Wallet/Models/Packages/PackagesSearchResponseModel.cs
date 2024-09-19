using System.Collections.Immutable;

namespace OpenCredentialPublisher.Wallet.Models.Packages
{
    public record PackagesSearchResponseModel
    {
        public PackagesSearchResponseModel(IImmutableList<PackageSearchResponseModel> packages,
            IImmutableList<string> issuerNames,
            IImmutableList<string> achievementTypes,
            IImmutableList<int> effectiveAtYears)
        {
            Packages = packages;
            IssuerNames = issuerNames;
            AchievementTypes = achievementTypes;
            EffectiveAtYears = effectiveAtYears;
        }

        public IImmutableList<PackageSearchResponseModel> Packages { get; } = ImmutableList<PackageSearchResponseModel>.Empty;
        public IImmutableList<string> IssuerNames { get; } = ImmutableList<string>.Empty;
        public IImmutableList<int> EffectiveAtYears { get; } = ImmutableList<int>.Empty;
        public IImmutableList<string> AchievementTypes { get; } = ImmutableList<string>.Empty;
    }
}
