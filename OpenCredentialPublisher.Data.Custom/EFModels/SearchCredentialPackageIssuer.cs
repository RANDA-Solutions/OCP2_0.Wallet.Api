using System;

namespace OpenCredentialPublisher.Data.Custom.EFModels
{
    public record SearchCredentialPackageIssuer(
        long CredentialPackageId,
        string IssuerName,
        DateTimeOffset EffectiveAt,
        int EffectiveAtYear
    );
}