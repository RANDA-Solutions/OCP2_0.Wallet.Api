using System.Collections.Generic;

namespace OpenCredentialPublisher.Data.Custom.Commands
{
    public record CredentialCollectionSaveCommand(
        long CredentialCollectionId,
        string Name,
        string Description,
        List<long> VerifiableCredentialIds);
}