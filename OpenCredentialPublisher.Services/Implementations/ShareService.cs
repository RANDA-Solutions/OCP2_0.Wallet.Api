using System;
using OpenCredentialPublisher.Data.Custom.Contexts;
using OpenCredentialPublisher.Data.Custom.EFModels;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IdentityModel;
using Microsoft.AspNetCore.WebUtilities;
using OpenCredentialPublisher.Shared.Utilities;
using OpenCredentialPublisher.Data.Models.Enums;
using System.Text;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Custom.Options;
using OpenCredentialPublisher.Data.Custom.Commands;
using OpenCredentialPublisher.Data.Custom.Results;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class ShareService
    {
        private readonly WalletDbContext _context;
        private readonly EmailService _emailService;
        private readonly SiteSettingsOptions _siteSettings;

        public ShareService(WalletDbContext context,
            EmailService emailService,
            IOptions<SiteSettingsOptions> siteSettings)
        {
            _context = context;
            _emailService = emailService;
            _siteSettings = siteSettings.Value;
        }

        public async Task<IImmutableList<Share>> GetAllAsync(string userId)
        {
            var query = _context.Shares
                .Include(s => s.ShareCredentialCollections)
                    .ThenInclude(sc => sc.CredentialCollection)
                        .ThenInclude(cc => cc.CredentialCollectionVerifiableCredentials)
                            .ThenInclude(ccvc => ccvc.VerifiableCredential)
                .Include(s => s.ShareVerifiableCredentials)
                    .ThenInclude(svc => svc.VerifiableCredential)
                .Include(s => s.User)
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.CreatedAt);

            return (await query.ToListAsync()).ToImmutableList();
        }

        public async Task<Share> GetAsync(string userId, long shareId)
        {
            var query = _context.Shares
                .Include(s => s.ShareCredentialCollections)
                    .ThenInclude(sc => sc.CredentialCollection)
                        .ThenInclude(cc => cc.CredentialCollectionVerifiableCredentials)
                            .ThenInclude(ccvc => ccvc.VerifiableCredential)
                                .ThenInclude(vc => vc.IssuerProfile)
                                .Include(s => s.ShareCredentialCollections)
                                .ThenInclude(sc => sc.CredentialCollection)
                                .ThenInclude(cc => cc.CredentialCollectionVerifiableCredentials)
                                .ThenInclude(ccvc => ccvc.VerifiableCredential)
                            .ThenInclude(vc => vc.Achievement)
                            .ThenInclude(a => a.Creator)
                            .Include(s => s.ShareCredentialCollections)
                            .ThenInclude(sc => sc.CredentialCollection)
                            .ThenInclude(cc => cc.CredentialCollectionVerifiableCredentials)
                            .ThenInclude(ccvc => ccvc.VerifiableCredential)
                            .ThenInclude(vc => vc.Achievement)
                            .ThenInclude(a => a.Alignments)
                            .Include(s => s.ShareCredentialCollections)
                            .ThenInclude(sc => sc.CredentialCollection)
                            .ThenInclude(cc => cc.CredentialCollectionVerifiableCredentials)
                            .ThenInclude(ccvc => ccvc.VerifiableCredential)
                            .ThenInclude(vc => vc.Achievement)
                            .ThenInclude(a => a.Source)
                            .Include(s => s.ShareCredentialCollections)
                            .ThenInclude(sc => sc.CredentialCollection)
                            .ThenInclude(cc => cc.CredentialCollectionVerifiableCredentials)
                            .ThenInclude(ccvc => ccvc.VerifiableCredential)
                            .ThenInclude(vc => vc.Achievement)
                            .ThenInclude(a => a.Identifier)
                .Include(s => s.ShareVerifiableCredentials)
                    .ThenInclude(svc => svc.VerifiableCredential)

                .ThenInclude(vc => vc.IssuerProfile)
                .Include(s => s.ShareVerifiableCredentials)
                .ThenInclude(ccvc => ccvc.VerifiableCredential)
                .ThenInclude(vc => vc.Achievement)
                .ThenInclude(a => a.Creator)
                .Include(s => s.ShareVerifiableCredentials)
                .ThenInclude(ccvc => ccvc.VerifiableCredential)
                .ThenInclude(vc => vc.Achievement)
                .ThenInclude(a => a.Alignments)
                .Include(s => s.ShareVerifiableCredentials)
                .ThenInclude(ccvc => ccvc.VerifiableCredential)
                .ThenInclude(vc => vc.Achievement)
                .ThenInclude(a => a.Source)
                .Include(s => s.ShareVerifiableCredentials)
                .ThenInclude(ccvc => ccvc.VerifiableCredential)
                .ThenInclude(vc => vc.Achievement)
                .ThenInclude(a => a.Identifier)
                .Include(s => s.User)
                .Where(s => s.UserId == userId && s.ShareId == shareId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Share> GetPublicShareAsync(long shareId, string hash)
        {
            return await _context.Shares
                .Include(x => x.User)
                .FirstOrDefaultAsync(s => s.ShareId == shareId && s.ShareSecureHash == hash);
        }

        public async Task<PublicShareDetailResult> GetPublicShareDetailResultAsync(long shareId, string hash, string code)
        {
            var share = await _context.Shares
                .Include(s => s.User)
                .Include(s => s.ShareVerifiableCredentials)
                .Include(s => s.ShareCredentialCollections)
                .ThenInclude(sc => sc.CredentialCollection)
                .ThenInclude(cc => cc.CredentialCollectionVerifiableCredentials)
                .FirstOrDefaultAsync(s => s.ShareId == shareId && s.AccessCode == code && s.ShareSecureHash == hash);


            if (share == null)
            {
                throw new InvalidDataException($"ShareId {shareId}, AccessCode: {code}, Hash: {hash} , are invalid or do not exist.");
            }

            return new PublicShareDetailResult
            {
                Share = share,
                VerifiableCredentialIds = share.ShareVerifiableCredentials.Select(vc => vc.VerifiableCredentialId)
                    .Concat(share.ShareCredentialCollections
                        .SelectMany(sc => sc.CredentialCollection.CredentialCollectionVerifiableCredentials)
                        .Select(ccvc => ccvc.VerifiableCredentialId))
                    .Distinct()
                    .ToImmutableList()
            };
        }

        public async Task<PublicShareValidateResult> GetPublicShareValidateResult(long shareId, string hash, string code, long verifiableCredentialId)
        {
            var result = await _context.Shares
                .Include(s => s.ShareVerifiableCredentials)
                .Include(s => s.ShareCredentialCollections)
                    .ThenInclude(scc => scc.CredentialCollection)
                    .ThenInclude(cc => cc.CredentialCollectionVerifiableCredentials)
                .Where(s => s.ShareId == shareId && s.AccessCode == code && s.ShareSecureHash == hash)
                .FirstOrDefaultAsync(s =>
                    s.ShareVerifiableCredentials.Any(vc => vc.VerifiableCredentialId == verifiableCredentialId) ||
                    s.ShareCredentialCollections.Any(scc => scc.CredentialCollection.CredentialCollectionVerifiableCredentials.Any(ccvc => ccvc.VerifiableCredentialId == verifiableCredentialId))
                );

            if (result == null)
            {
                throw new InvalidDataException($"ShareId {shareId}, AccessCode: {code}, Hash: {hash} , VerifiableCredentialId: {verifiableCredentialId} are invalid or do not exist.");
            }

            return new PublicShareValidateResult(result.ShareId, hash, code, result.UserId);
        }

        public async Task<Share> AddAsync(string userId, ShareAddCommand command)
        {
            var user = await _context.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null)
                throw new ArgumentNullException(nameof(user), $"User not found with userId: {userId}");

            // get credentials and ensure they are ours
            var verifiableCredentials = await _context.CredentialPackages2
                .Include(cp => cp.VerifiableCredentials)
                .Where(cp => cp.UserId == userId)
                .SelectMany(cp => cp.VerifiableCredentials)
                .Where(vc => vc.ParentVerifiableCredentialId != null)
                .Where(vc => command.VerifiableCredentialIds.Contains(vc.VerifiableCredentialId))
                .ToListAsync();

            // get collections
            var credentialCollections = await _context.CredentialCollection2
                .Where(cc => cc.UserId == userId)
                .Where(cc => command.CredentialCollectionIds.Contains(cc.CredentialCollectionId))
                .ToListAsync();

            var share = new Share
            {
                Email = command.Email,
                Description = command.Description,
                ShareSecureHash = WebEncoders.Base64UrlEncode(CryptoRandom.CreateRandomKey(38)),
                AccessCode = AccessCodeGenerator.GenerateUniqueNumericCode(CryptoRandom.CreateRandomKey(10)),
                ShareCredentialCollections = credentialCollections
                    .Select(cc => new ShareCredentialCollection() { CredentialCollection = cc }).ToList(),
                ShareVerifiableCredentials = verifiableCredentials
                    .Select(vc => new ShareVerifiableCredential() { VerifiableCredential = vc }).ToList(),
                User = user
            };

            await _context.Shares.AddAsync(share);
            // I hate this but need the share ID for the message!
            await _context.SaveChangesAsync();

            // subject: John Smith has shared credentials with you.
            // email: link and access code
            var bodyStringBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(share.Description))
            {
                bodyStringBuilder.AppendLine($"<p>{share.Description}</p><hr/>");
            }

            var plural = share.TotalCredentialCount > 1 ? "s" : "";
            var subjectArticle = share.TotalCredentialCount == 1 ? "a " : "";
            var bodyArticle = share.TotalCredentialCount == 1 ? "the " : "";
            bodyStringBuilder.AppendLine($"<p>To view {bodyArticle}credential{plural}, click on the link below and provide this access code:</p>");
            bodyStringBuilder.AppendLine($"<p style=\"text-align:center;font-size:24pt; font-weight:bold;\">{share.AccessCode}</p>");
            bodyStringBuilder.AppendLine($"<p style=\"text-align:center\"><a href=\"{_siteSettings.SpaClientUrl}/public/shares/{share.ShareId}/?hash={share.ShareSecureHash}\">View Credential{plural}</></p>");

            var shareMessage = new Message
            {
                Body = bodyStringBuilder.ToString(),
                Recipient = share.Email,
                Subject = $"{user.DisplayName} has shared {subjectArticle}credential{plural} with you.",
                SendAttempts = 0,
                StatusId = StatusEnum.Created
            };

            await _context.Messages.AddAsync(shareMessage);
            await _context.SaveChangesAsync();

            await _emailService.SendEmailAsync(shareMessage.Recipient,
                shareMessage.Subject,
                shareMessage.Body,
                true);

            // update message status to sent
            shareMessage.StatusId = StatusEnum.Sent;
            _context.Messages.Update(shareMessage);
            await _context.SaveChangesAsync();

            return share;
        }
    }
}
