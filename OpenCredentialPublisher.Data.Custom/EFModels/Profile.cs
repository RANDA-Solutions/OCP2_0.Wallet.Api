using OpenCredentialPublisher.Shared.Interfaces;
using System;

namespace OpenCredentialPublisher.Data.Custom.EFModels
{
    public class Profile : IBaseEntity
    {
        public long ProfileId { get; set; }

        public string Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }

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
