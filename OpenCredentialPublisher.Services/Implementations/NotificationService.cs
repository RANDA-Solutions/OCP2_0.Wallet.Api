using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Data.Custom.Contexts;
using OpenCredentialPublisher.Data.Custom.CredentialModels;
using OpenCredentialPublisher.Data.Custom.EFModels;
using OpenCredentialPublisher.Data.Custom.Results;
using OpenCredentialPublisher.Data.Models.Enums;

//2021-06-17 EF Tracking OK
namespace OpenCredentialPublisher.Services.Implementations
{
    public class NotificationService
    {
        private readonly WalletDbContext _context;
        private readonly ETLService _etlService;

        public NotificationService(WalletDbContext context,
            ETLService etlService)
        {
            _context = context;
            _etlService = etlService;
        }

        public async Task<List<NotificationResult>> GetNotificationsAsync(string userId)
        {
            var result = (await _context.Notifications.AsNoTracking()
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync())
                .Select(n => new NotificationResult(n)).ToList();

            return result;
        }

        public async Task<Notification> GetNotificationAsync(int notificationId, string userId)
        {
            var result = await _context.Notifications
                .Where(n => n.UserId == userId && n.NotificationId == notificationId).FirstOrDefaultAsync();

            return result;
        }

        public async Task RemoveNotificationAsync(Notification notification)
        {
            if (notification == null)
                return;

            notification.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task MarkAsReadAsync(Notification notification)
        {
            notification.Status = StatusEnum.Read;
            await _context.SaveChangesAsync();
        }

        public async Task MarkAsUnReadAsync(Notification notification)
        {
            notification.Status = StatusEnum.Unread;
            await _context.SaveChangesAsync();
        }

        public async Task<CredentialResponse> AddNotificationAsync(string userId, string clrJson, HttpRequest request)
        {
            var (clrCredentialModel, credentialResponse) = await _etlService.GetClrCredentialModelAsync(clrJson, request);
            if (credentialResponse.HasError)
                return credentialResponse;

            var notification = new Notification
            {
                UserId = userId,
                IssuerName = clrCredentialModel.Issuer.Name ?? clrCredentialModel.Issuer.Id,
                IssuerImageUrl = clrCredentialModel.Image?.Id,
                Name = clrCredentialModel.Name,
                Description = clrCredentialModel.Description,
                Status = StatusEnum.Unread,
                Json = clrJson,
                IsDeleted = false,
                AchievementCount = clrCredentialModel.CredentialSubject.VerifiableCredential
                    .Select(vc => vc.CredentialSubject is AchievementSubjectModel).Count()
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return credentialResponse;
        }
    }
}
