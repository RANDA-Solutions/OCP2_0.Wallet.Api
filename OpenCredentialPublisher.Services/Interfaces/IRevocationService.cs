using OpenCredentialPublisher.Data.Custom.Results;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Interfaces
{
    public interface IRevocationService
    {
        Task<RevocationResult> CheckAndRevokeCredentialAsync(long verifiableCredentialId);
        Task<RevocationResult> GetRevocationResultAsync(string json, string verifiableCredentialModelId);
    }
}