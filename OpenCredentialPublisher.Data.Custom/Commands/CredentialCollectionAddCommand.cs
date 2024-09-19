using System.Collections.Generic;

namespace OpenCredentialPublisher.Data.Custom.Commands
{
    public record CredentialCollectionAddCommand(
string Name,
string Description,
List<long> VerifiableCredentialIds);
}
