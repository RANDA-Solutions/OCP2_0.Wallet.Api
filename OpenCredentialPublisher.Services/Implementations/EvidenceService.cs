using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Data.Custom.Contexts;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class EvidenceService
    {
        private readonly WalletDbContext _context;

        public EvidenceService(WalletDbContext context)
        {
            _context = context;
        }

        public async Task<IImmutableList<Evidence>> GetByVerifiableCredentialId(string userId, long verifiableCredentialId)
        {
            var query = _context.Evidences2
                .Include(ev => ev.VerifiableCredential)
                .ThenInclude(ev => ev.CredentialPackage)
                .AsNoTracking()
                .Where(ev => ev.VerifiableCredentialId == verifiableCredentialId && ev.VerifiableCredential.CredentialPackage.UserId == userId);

            var evidences = await query
                .OrderByDescending(x => x.Name)
                .ToListAsync();

            return evidences.ToImmutableList();
        }

        public async Task DeleteAsync(long evidenceId)
        {
            var evidence = await _context.Evidences2.FirstOrDefaultAsync(ev => ev.EvidenceId == evidenceId);

            if (evidence != null)
            {
                evidence?.Delete();

                await _context.SaveChangesAsync();
            }
        }
    }
}
