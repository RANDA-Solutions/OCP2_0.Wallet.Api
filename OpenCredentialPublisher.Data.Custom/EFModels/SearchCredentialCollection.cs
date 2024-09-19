using System;

namespace OpenCredentialPublisher.Data.Custom.EFModels
{
    public record SearchCredentialCollection(
        long CredentialCollectionId,
        string OwnerUserId,
        string Name,
        string Description,
        int ShareCount,
        DateTimeOffset CreatedAt);
}