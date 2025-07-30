using System.Collections.Generic;

namespace OpenCredentialPublisher.Data.Custom.Commands
{
    public record ShareAddShareTypeCommand(
        List<long> VerifiableCredentialIds,
        List<long> CredentialCollectionIds,
        string ShareType
    );
}

