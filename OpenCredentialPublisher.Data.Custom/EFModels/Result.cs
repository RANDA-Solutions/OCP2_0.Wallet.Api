using System;
using OpenCredentialPublisher.Shared.Interfaces;

namespace OpenCredentialPublisher.Data.Custom.EFModels
{
    public class Result : IBaseEntity
    {
        public long ResultId { get; set; }
        public bool IsDeleted { get; set; }

        public VerifiableCredential VerifiableCredential { get; set; } = new();
        public long VerifiableCredentialId { get; set; } = new();


        public string ResultDescriptionType { get; set; }
        public string ResultDescriptionName { get; set; }

        public string Status { get; set; }
        public string Value { get; set; }


        public DateTimeOffset? ModifiedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public void Delete()
        {
            IsDeleted = true;
            ModifiedAt = DateTimeOffset.UtcNow;
        }
    }
}