using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Data.Custom.Commands;
using OpenCredentialPublisher.Data.Custom.Contexts;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class CredentialCollectionService
    {
        private readonly WalletDbContext _context;

        public CredentialCollectionService(WalletDbContext context)
        {
            _context = context;
        }
        public async Task<IImmutableList<SearchCredentialCollection>> SearchAsync(string userId, string keywords, string sortBy)
        {
            var query = _context.SearchCredentialCollections.AsNoTracking()
                .Where(cc => cc.OwnerUserId == userId);

            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(cc => cc.Name.Contains(keywords) || cc.Description.Contains(keywords));
            }

            if (nameof(SearchCredentialCollection.CreatedAt).Equals(sortBy, StringComparison.OrdinalIgnoreCase))
            {
                query = query.OrderByDescending(cc => cc.CreatedAt);
            }
            else if (nameof(SearchCredentialCollection.Name).Equals(sortBy, StringComparison.OrdinalIgnoreCase))
            {
                query = query.OrderBy(cc => cc.Name);
            }
            else if (nameof(SearchCredentialCollection.ShareCount).Equals(sortBy, StringComparison.OrdinalIgnoreCase))
            {
                query = query.OrderByDescending(cc => cc.ShareCount);
            }

            var credentialCollections = await query
                .ToListAsync();

            return credentialCollections.ToImmutableList();
        }

        public async Task DeleteAsync(string userId, long credentialCollectionId)
        {
            var credentialCollection = await GetAsync(userId, credentialCollectionId);

            credentialCollection?.Delete();

            await _context.SaveChangesAsync();
        }

        public async Task<CredentialCollection> GetAsync(string userId, long credentialCollectionId)
        {
            return await _context.CredentialCollection2
                .Include(cc => cc.CredentialCollectionVerifiableCredentials)
                    .ThenInclude(ccvc => ccvc.VerifiableCredential)
                .Include(cc => cc.ShareCredentialCollections)
                .Where(cc => cc.UserId == userId && cc.CredentialCollectionId == credentialCollectionId)
                .FirstOrDefaultAsync();
        }

        public async Task<IImmutableList<CredentialCollection>> GetAsync(string userId, List<long> credentialCollectionIds)
        {
            var credentialCollections = await _context.CredentialCollection2
                .Include(cc => cc.CredentialCollectionVerifiableCredentials)
                .ThenInclude(ccvc => ccvc.VerifiableCredential)
                .Include(cc => cc.ShareCredentialCollections)
                .Where(cc => cc.UserId == userId && credentialCollectionIds.Contains(cc.CredentialCollectionId))
                .ToListAsync();

            return credentialCollections.ToImmutableList();
        }


        public async Task<CredentialCollection> AddAsync(string userId, CredentialCollectionAddCommand command)
        {
            var verifiableCredentialIds = command.VerifiableCredentialIds;

            var verifiableCredentials = await _context.CredentialPackages2
                .Where(cp => cp.UserId == userId)
                .SelectMany(cp => cp.VerifiableCredentials)
                .Where(vc => verifiableCredentialIds.Contains(vc.VerifiableCredentialId))
                .ToListAsync();

            var credentialCollection = new CredentialCollection
            {
                UserId = userId,
                Name = command.Name,
                Description = command.Description,
                CredentialCollectionVerifiableCredentials = verifiableCredentials.Select(vc => new CredentialCollectionVerifiableCredential
                { VerifiableCredential = vc }).ToList()
            };

            await _context.CredentialCollection2.AddAsync(credentialCollection);

            await _context.SaveChangesAsync();

            return credentialCollection;
        }

        public async Task<CredentialCollection> SaveAsync(string userId, CredentialCollectionSaveCommand command)
        {
            var credentialCollection = await _context.CredentialCollection2
                .Include(cc => cc.CredentialCollectionVerifiableCredentials)
                .Where(cc => cc.UserId == userId && cc.CredentialCollectionId == command.CredentialCollectionId)
                .FirstOrDefaultAsync();

            if (credentialCollection == null)
                throw new ArgumentNullException(nameof(CredentialCollection),
                    "The specified collection was not found.");

            credentialCollection.Name = command.Name;
            credentialCollection.Description = command.Description;

            // reconcile credentials
            var incomingVerifiableCredentialIds = command.VerifiableCredentialIds;

            // load incoming -- enforces security
            var incomingVerifiableCredentials = await _context.CredentialPackages2
                .Where(cp => cp.UserId == userId)
                .SelectMany(cp => cp.VerifiableCredentials)
                .Where(vc => incomingVerifiableCredentialIds.Contains(vc.VerifiableCredentialId))
                .ToListAsync();

            var existingVerifiableCredentialIds = credentialCollection.CredentialCollectionVerifiableCredentials.Select(vc => vc.VerifiableCredentialId).ToList();

            // Identify credentials to remove from the collection
            var credentialsToRemove = credentialCollection.CredentialCollectionVerifiableCredentials
                .Where(src => !incomingVerifiableCredentialIds.Contains(src.VerifiableCredentialId))
                .ToList();

            // Identify credentials to add to the collection
            var credentialsToAdd = incomingVerifiableCredentials
                // ReSharper disable once SimplifyLinqExpressionUseAll
                .Where(src => !existingVerifiableCredentialIds.Contains(src.VerifiableCredentialId))
                .ToList();

            credentialCollection.CredentialCollectionVerifiableCredentials
                .AddRange(credentialsToAdd.Select(c => new CredentialCollectionVerifiableCredential { VerifiableCredential = c }));

            foreach (var credentialToRemove in credentialsToRemove)
            {
                var credentialCollectionVerifiableCredential = credentialCollection.CredentialCollectionVerifiableCredentials
                    .First(cvc => cvc.VerifiableCredentialId == credentialToRemove.VerifiableCredentialId);
                credentialCollection.CredentialCollectionVerifiableCredentials.Remove(credentialCollectionVerifiableCredential);
            }

            await _context.SaveChangesAsync();

            return credentialCollection;
        }
    }
}
