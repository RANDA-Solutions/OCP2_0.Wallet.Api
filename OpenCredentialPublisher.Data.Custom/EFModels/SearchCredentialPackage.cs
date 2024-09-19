using System;
using System.Collections.Generic;

namespace OpenCredentialPublisher.Data.Custom.EFModels
{
    public record SearchCredentialPackage(
        long CredentialPackageId,
        string Name,
        string IssuerName,
        string EffectiveImageUrl,
        string OwnerUserId,
        int AchievementCount,
        int ShareCount,
        DateTimeOffset EffectiveAt,
        DateTimeOffset? ExpiresAt,
        DateTimeOffset CreatedAt,
        bool IsVerified,
        bool IsRevoked,
        string RevokedReason,
        string Json,
        long VerifiableCredentialId)
    {
        public IList<SearchCredentialPackageIssuer> Issuers { get; init; } = new List<SearchCredentialPackageIssuer>();
        public IList<SearchCredentialPackageAchievementType> AchievementTypes { get; init; } = new List<SearchCredentialPackageAchievementType>();
    }
}
