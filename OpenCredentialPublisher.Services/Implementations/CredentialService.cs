using System;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Data.Custom.Commands;
using OpenCredentialPublisher.Data.Custom.Contexts;
using OpenCredentialPublisher.Data.Custom.EFModels;
using OpenCredentialPublisher.Shared.Custom.Models;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class CredentialService
    {
        private readonly WalletDbContext _context;
        private readonly CredentialPackageService _credentialPackageService;

        public CredentialService(WalletDbContext context, CredentialPackageService credentialPackageService)
        {
            _context = context;
            _credentialPackageService = credentialPackageService;
        }

        public async Task<IImmutableList<SearchCredential>> SearchAsync(string userId,
            string keywordsFilter,
            string issuerNameFilter,
            string achievementTypeFilter,
            int? effectiveAtYearFilter)
        {
            var query = _context.SearchCredentials.AsNoTracking()
                .Where(cp => cp.OwnerUserId == userId);

            if (!string.IsNullOrEmpty(keywordsFilter))
            {
                query = query.Where(cp => cp.Json.Contains(keywordsFilter));
            }

            if (!string.IsNullOrEmpty(issuerNameFilter))
            {
                query = query.Where(cp => cp.IssuerName == issuerNameFilter);
            }

            if (!string.IsNullOrEmpty(achievementTypeFilter))
            {
                query = query.Where(cp => cp.AchievementType == achievementTypeFilter);
            }

            if (effectiveAtYearFilter.HasValue)
            {
                query = query.Where(cp => cp.EffectiveAtYear == effectiveAtYearFilter.Value);
            }

            var searchCredentials = await query
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return searchCredentials.ToImmutableList();
        }

        public async Task<VerifiableCredential> GetAsync(string userId, long verifiableCredentialId)
        {
            return await _context.VerifiableCredentials2
                .Include(vc => vc.ParentVerifiableCredential)
                .Include(vc => vc.CredentialPackage)
                .Include(vc => vc.IssuerProfile)
                .Include(vc => vc.Achievement)
                .ThenInclude(a => a.Creator)
                .Include(vc => vc.Achievement)
                .ThenInclude(a => a.Alignments)
                .Include(vc => vc.Achievement)
                .ThenInclude(a => a.Source)
                .Include(vc => vc.Achievement)
                .ThenInclude(a => a.Identifier)
                .Include(vc => vc.Evidences)
                .Include(vc => vc.ShareVerifiableCredentials)
                .Include(vc => vc.CredentialCollectionVerifiableCredentials)
                    .ThenInclude(ccvc => ccvc.CredentialCollection)
                    .ThenInclude(cc => cc.ShareCredentialCollections)
                .Include(vc => vc.Results)
                .Include(vc => vc.SourceAssociations)
                    .ThenInclude(sa => sa.TargetVerifiableCredential)
                        .ThenInclude(tvc => tvc.Achievement)
                .FirstOrDefaultAsync(vc => vc.CredentialPackage.UserId == userId &&
                                           vc.VerifiableCredentialId == verifiableCredentialId);
        }

        public async Task DeleteAsync(string userId, long verifiableCredentialId)
        {
            var credential = await GetAsync(userId, verifiableCredentialId);

            var package = await _credentialPackageService.GetAsync(userId, credential.CredentialPackageId);
            credential?.Delete();

            if (package != null && !package.ChildVerifiableCredentials.Any(x => x.VerifiableCredentialId != verifiableCredentialId))
            {
                package.Delete();
            }

            await _context.SaveChangesAsync();
        }

        public async Task<int> GetAchievementCountAsync(string userId)
        {
            var count = await _context.Achievements2.AsNoTracking()
                .Where(a => !a.IsDeleted)
                .Join(_context.VerifiableCredentials2.AsNoTracking().Where(vc => !vc.IsDeleted && vc.ParentVerifiableCredentialId != null),
                    a => a.VerifiableCredentialId,
                    vc => vc.VerifiableCredentialId,
                    (a, vc) => new { a, vc })
                .Join(_context.CredentialPackages2.AsNoTracking(),
                    avc => avc.vc.CredentialPackageId,
                    cp => cp.CredentialPackageId,
                    (avc, cp) => new { avc.a, avc.vc, cp })
                .Where(x => x.cp.UserId == userId)
                .CountAsync();

            return count;
        }

        public async Task<int> GetVerifiableCredentialCountAsync(string userId)
        {
            var count = await _context.VerifiableCredentials2.AsNoTracking()
                .Where(vc => !vc.IsDeleted && vc.ParentVerifiableCredentialId != null)
                .Join(_context.CredentialPackages2.AsNoTracking(),
                    vc => vc.CredentialPackageId,
                    cp => cp.CredentialPackageId,
                    (vc, cp) => new { vc.VerifiableCredentialId, cp.UserId })
                .Where(x => x.UserId == userId)
                .Select(x => x.VerifiableCredentialId)
                .CountAsync();

            return count;
        }

        public async Task<int> GetResultsCountAsync(string userId)
        {
            var count = await _context.VerifiableCredentials2.AsNoTracking()
                .Where(vc => !vc.IsDeleted && vc.ParentVerifiableCredentialId != null)
                .Join(_context.CredentialPackages2.AsNoTracking(),
                    vc => vc.CredentialPackageId,
                    cp => cp.CredentialPackageId,
                    (vc, cp) => new { vc.VerifiableCredentialId, cp.UserId })
                .Where(x => x.UserId == userId)
                .Join(_context.Results2.AsNoTracking(),
                    vc => vc.VerifiableCredentialId,
                    r => r.VerifiableCredentialId,
                    (vc, r) => r.ResultId)
                .CountAsync();
            return count;
        }

        public async Task<CredentialsReportDto> GetCredentialsReportAsync()
        {
            var offersResult = await _context.Notifications.AsNoTracking()
                .IgnoreQueryFilters()
                .Join(_context.Users.AsNoTracking(),
                    n => n.UserId,
                    u => u.Id,
                    (n, u) => new { n, u })
                .GroupBy(x => 1)
                .Select(g => new
                {
                    OfferedUsers = g.Select(x => x.u.Id).Distinct().Count(),
                    Offers = g.Select(x => x.n.NotificationId).Distinct().Count()
                }).FirstOrDefaultAsync();

            var credentialsResult = await _context.VerifiableCredentials2.AsNoTracking()
                .Join(_context.CredentialPackages2.AsNoTracking(),
                    vc => vc.CredentialPackageId,
                    cp => cp.CredentialPackageId,
                    (vc, cp) => new { vc, cp })
                .GroupBy(x => 1)
                .Select(g => new
                {
                    CredentialedUsers = g.Select(x => x.cp.UserId).Distinct().Count(),
                    CredentialPackagesGranted = g.Select(x => x.cp.CredentialPackageId).Distinct().Count()
                }).FirstOrDefaultAsync();

            var credentialsGranted = await _context.CredentialPackages2.AsNoTracking()
                .Where(cp => !cp.IsDeleted)
                .Join(_context.VerifiableCredentials2.AsNoTracking().Where(vc => !vc.IsDeleted),
                    cp => cp.CredentialPackageId,
                    vc => vc.CredentialPackageId,
                    (cp, vc) => vc)
                .Select(x => x.VerifiableCredentialId)
                .CountAsync();

            var users = await _context.Users.AsNoTracking()
                .OrderBy(x => x.EmailConfirmed).ThenBy(x => x.Email)
                .Select(x => new CredentialsReportUserDto
                {
                    DisplayName = x.DisplayName,
                    UserName = x.UserName,
                    EmailConfirmed = x.EmailConfirmed,
                    CreatedAt = x.CreatedAt,
                    ModifiedAt = x.ModifiedAt
                })
                .ToListAsync();

            return new CredentialsReportDto
            {
                OfferedUsers = offersResult?.OfferedUsers ?? 0,
                Offers = offersResult?.Offers ?? 0,
                CredentialedUsers = credentialsResult?.CredentialedUsers ?? 0,
                CredentialPackagesGranted = credentialsResult?.CredentialPackagesGranted ?? 0,
                CredentialsGranted = credentialsGranted,
                DateGenerated = DateTime.UtcNow,
                Users = users.ToArray()
            };
        }
    }
}
