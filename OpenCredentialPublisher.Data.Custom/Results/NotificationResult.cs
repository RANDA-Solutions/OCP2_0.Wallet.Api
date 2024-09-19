using System;
using OpenCredentialPublisher.Data.Custom.EFModels;
using OpenCredentialPublisher.Data.Models.Enums;

namespace OpenCredentialPublisher.Data.Custom.Results
{
    public record NotificationResult(
        long NotificationId,
        string IssuerName,
        string IssuerImage,
        string Name,
        string Description,
        int AchievementCount,
        StatusEnum Status,
        DateTimeOffset Date
    )

    {
        public NotificationResult(Notification notification) : this(
            notification.NotificationId,
            notification.IssuerName,
            notification.IssuerImageUrl,
            notification.Name,
            notification.Description,
            notification.AchievementCount,
            notification.Status,
            notification.CreatedAt
            )
        {
        }
    };
}
