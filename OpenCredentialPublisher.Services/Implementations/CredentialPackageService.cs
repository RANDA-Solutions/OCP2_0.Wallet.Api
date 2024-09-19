using OpenCredentialPublisher.Data.Custom.Contexts;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class CredentialPackageService
    {
        private readonly WalletDbContext _context;

        public CredentialPackageService(WalletDbContext context)
        {
            _context = context;
        }

        public async Task<IImmutableList<SearchCredentialPackage>> SearchAsync(string userId,
            string keywordsFilter,
            string issuerNameFilter,
            string achievementTypeFilter,
            int? effectiveAtYearFilter)
        {
            var query = _context.SearchCredentialPackages.AsNoTracking()
                .Include(p => p.Issuers).AsNoTracking()
                .Include(p => p.AchievementTypes).AsNoTracking()
                .Where(cp => cp.OwnerUserId == userId);

            if (!string.IsNullOrEmpty(keywordsFilter))
            {
                query = query.Where(cp => cp.Json.Contains(keywordsFilter));
            }

            if (!string.IsNullOrEmpty(issuerNameFilter))
            {
                query = query.Where(cp => cp.Issuers.Any(i => i.IssuerName == issuerNameFilter));
            }

            if (!string.IsNullOrEmpty(achievementTypeFilter))
            {
                query = query.Where(cp => cp.AchievementTypes.Any(ct => ct.AchievementType == achievementTypeFilter));
            }

            if (effectiveAtYearFilter.HasValue)
            {
                query = query.Where(cp => cp.Issuers.Any(i => i.EffectiveAtYear == effectiveAtYearFilter.Value));
            }

            var searchCredentialPackages = await query
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return searchCredentialPackages.ToImmutableList();
        }


        public async Task<CredentialPackage> GetAsync(string userId, long credentialPackageId)
        {
            var baseQuery = _context.CredentialPackages2.Where(cp => cp.UserId == userId && cp.CredentialPackageId == credentialPackageId);

            return await IncludeDependencies(baseQuery).FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(string userId, long credentialPackageId)
        {
            var credentialPackage = await GetAsync(userId, credentialPackageId);

            credentialPackage?.Delete();

            await _context.SaveChangesAsync();
        }


        private IQueryable<CredentialPackage> IncludeDependencies(IQueryable<CredentialPackage> query)
        {
            return query.Include(p => p.VerifiableCredentials)
                .ThenInclude(vc => vc.Evidences)
                .Include(p => p.VerifiableCredentials)
                .ThenInclude(vc => vc.ShareVerifiableCredentials)
                .Include(p => p.VerifiableCredentials)
                .ThenInclude(vc => vc.CredentialCollectionVerifiableCredentials)
                    .ThenInclude(ccvc => ccvc.CredentialCollection)
                        .ThenInclude(cc => cc.ShareCredentialCollections)
                .Include(p => p.VerifiableCredentials)
                .ThenInclude(vc => vc.ShareVerifiableCredentials)
                .Include(p => p.VerifiableCredentials)
                .ThenInclude(vc => vc.IssuerProfile)
                .Include(p => p.VerifiableCredentials)
                .ThenInclude(vc => vc.Achievement)
                .ThenInclude(a => a.Creator)
                .Include(p => p.VerifiableCredentials)
                .ThenInclude(vc => vc.Achievement)
                .ThenInclude(a => a.Alignments)
                .Include(p => p.VerifiableCredentials)
                .ThenInclude(vc => vc.Achievement)
                .ThenInclude(a => a.Source)
                .Include(p => p.VerifiableCredentials)
                .ThenInclude(vc => vc.Achievement)
                .ThenInclude(a => a.Identifier)
                .Include(p => p.VerifiableCredentials)
                .ThenInclude(vc => vc.ParentVerifiableCredential);
        }
    }
}
