using System;
using OpenCredentialPublisher.Data.Custom.EFModels;
using OpenCredentialPublisher.Data.Models.Enums;

namespace OpenCredentialPublisher.Wallet.Models.Notifications
{
    public record NotificationResponseModel
    {
        public NotificationResponseModel(Notification notification)
        {
            NotificationId = notification.NotificationId;
            IssuerName = notification.IssuerName;
            IssuerImage = notification.IssuerImageUrl;
            Name = notification.Name;
            Description = notification.Description;
            Status = notification.Status;
            AchievementCount = notification.AchievementCount;
            Date = notification.CreatedAt;
        }

        public long NotificationId { get; set; }
        public string IssuerName { get; set; }
        public string IssuerImage { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AchievementCount { get; set; }

        public StatusEnum Status { get; set; }
        public DateTimeOffset Date { get; set; }

    }
}