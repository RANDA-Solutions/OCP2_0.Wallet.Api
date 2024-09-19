using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Data.Custom.Commands
{
    public record CredentialRevokeCommand(bool IsRevoked, string RevokeReason, VerifiableCredential VerifiableCredential);
}