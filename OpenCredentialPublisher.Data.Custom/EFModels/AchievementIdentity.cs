using System;
using OpenCredentialPublisher.Shared.Interfaces;

namespace OpenCredentialPublisher.Data.Custom.EFModels
{
    public class AchievementIdentity : IBaseEntity
    {
        public long AchievementIdentityId { get; set; }

        public long AchievementId { get; set; }
        public Achievement Achievement { get; set; }

        public string Name { get; set; }
        public string EmailAddress { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        public void Delete()
        {
            IsDeleted = true;
            ModifiedAt = DateTimeOffset.UtcNow;
        }

        public string DisplayName => Name ?? EmailAddress;
    }
}
