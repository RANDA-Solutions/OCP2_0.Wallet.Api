using System;
using OpenCredentialPublisher.Shared.Interfaces;

namespace OpenCredentialPublisher.Data.Custom.EFModels
{
    public class ShareVerifiableCredential : IBaseEntity
    {
        public long ShareId { get; set; }
        public Share Share { get; set; }


        public long VerifiableCredentialId { get; set; }
        public VerifiableCredential VerifiableCredential { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset? ModifiedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public void Delete()
        {
            IsDeleted = true;
            ModifiedAt = DateTimeOffset.UtcNow;
        }
    }
}