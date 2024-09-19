using System.Collections.Generic;

namespace OpenCredentialPublisher.Data.Custom.Commands
{
    public record ShareAddCommand(
        string Email,
        string Description,
        List<long> VerifiableCredentialIds,
        List<long> CredentialCollectionIds
    );
}

