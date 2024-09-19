using System;
using OpenCredentialPublisher.Shared.Interfaces;

namespace OpenCredentialPublisher.Data.Custom.EFModels
{
    public class CredentialCollectionVerifiableCredential : IBaseEntity
    {
        public long CredentialCollectionId { get; set; }
        public CredentialCollection CredentialCollection { get; set; }

        public long VerifiableCredentialId { get; set; }
        public VerifiableCredential VerifiableCredential { get; set; }

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
