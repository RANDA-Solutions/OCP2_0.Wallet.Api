using System;

namespace OpenCredentialPublisher.Shared.Interfaces
{
    public interface IBaseEntity
    {
        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }

        public void Delete ()
        {
            IsDeleted = true;
            ModifiedAt = DateTimeOffset.UtcNow;
        }
    }
}
