using System;

namespace OpenCredentialPublisher.Data.Custom.EFModels
{
    public record SearchCredential(
        long CredentialPackageId,
        long VerifiableCredentialId,
        string AchievementType,
        string IssuerName,
        string OwnerUserId,
        DateTimeOffset EffectiveAt,
        int EffectiveAtYear,
        DateTimeOffset CreatedAt,
        string Json);
}