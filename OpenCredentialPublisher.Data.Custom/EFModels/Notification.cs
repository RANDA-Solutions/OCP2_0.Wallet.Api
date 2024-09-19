using System;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Shared.Interfaces;

namespace OpenCredentialPublisher.Data.Custom.EFModels
{
    public class Notification : IBaseEntity
    {
        public long NotificationId { get; set; }

        public string UserId { get; set; }

        public string IssuerName { get; set; }
        public string IssuerImageUrl { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public StatusEnum Status { get; set; }

        public string Json { get; set; }
        public bool IsDeleted { get; set; }

        public int AchievementCount { get; set; }

        public DateTimeOffset? ModifiedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public void Delete()
        {
            IsDeleted = true;
            ModifiedAt = DateTimeOffset.UtcNow;
        }

    }
}