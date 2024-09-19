using System;
using System.Collections.Generic;
using OpenCredentialPublisher.Shared.Interfaces;

namespace OpenCredentialPublisher.Data.Custom.EFModels
{
    public class Evidence : IBaseEntity
    {
        public long EvidenceId { get; set; }

        public long VerifiableCredentialId { get; set; }
        public VerifiableCredential VerifiableCredential { get; set; }

        public string EvidenceUrl { get; set; }
        public List<string> Type { get; set; }
        public string Name { get; set; }

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