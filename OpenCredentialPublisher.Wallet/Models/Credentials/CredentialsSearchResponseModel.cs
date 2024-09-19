using System.Collections.Immutable;

namespace OpenCredentialPublisher.Wallet.Models.Credentials
{
    public record CredentialsSearchResponseModel
    {
        public CredentialsSearchResponseModel(IImmutableList<long> verifiableCredentialIds,
            IImmutableList<string> issuerNames,
            IImmutableList<string> achievementTypes,
            IImmutableList<int> effectiveAtYears)
        {
            VerifiableCredentialIds = verifiableCredentialIds;
            IssuerNames = issuerNames;
            AchievementTypes = achievementTypes;
            EffectiveAtYears = effectiveAtYears;
        }

        public IImmutableList<long> VerifiableCredentialIds { get; } = ImmutableList<long>.Empty;
        public IImmutableList<string> IssuerNames { get; } = ImmutableList<string>.Empty;
        public IImmutableList<int> EffectiveAtYears { get; } = ImmutableList<int>.Empty;
        public IImmutableList<string> AchievementTypes { get; } = ImmutableList<string>.Empty;
    }
}