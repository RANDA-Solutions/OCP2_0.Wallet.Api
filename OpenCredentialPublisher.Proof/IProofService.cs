using System.Threading.Tasks;

namespace OpenCredentialPublisher.Proof
{
    public interface IProofService
    {
        Task<bool> VerifyProof(string originalJson);
    }
}