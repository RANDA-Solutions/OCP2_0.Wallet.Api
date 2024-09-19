using System;

namespace OpenCredentialPublisher.Data.Custom.EFModels
{
    public record SearchCredentialIssuer(
        long VerifiableCredentialId,
        string IssuerName,
        DateTimeOffset EffectiveAt,
        int EffectiveAtYear
    );
}