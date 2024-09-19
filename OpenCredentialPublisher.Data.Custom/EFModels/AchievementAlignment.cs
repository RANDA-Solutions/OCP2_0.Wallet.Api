using OpenCredentialPublisher.Shared.Interfaces;
using System;
using System.Collections.Generic;

namespace OpenCredentialPublisher.Data.Custom.EFModels
{
    public class AchievementAlignment : IBaseEntity
    {
        public long AchievementAlignmentId { get; set; }

        public long AchievementId { get; set; }
        public Achievement Achievement { get; set; }

        public List<string> Type { get; set; } = new() { "Alignment" };

        public string TargetCode { get; set; }
        public string TargetDescription { get; set; }
        public string TargetName { get; set; }
        public string TargetFramework { get; set; }
        public string TargetType { get; set; }
        public string TargetUrl { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        public void Delete()
        {
            IsDeleted = true;
            ModifiedAt = DateTimeOffset.UtcNow;
        }
    }
}