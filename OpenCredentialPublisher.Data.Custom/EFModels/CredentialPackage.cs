using OpenCredentialPublisher.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OpenCredentialPublisher.Data.Custom.EFModels
{
    public class CredentialPackage : IBaseEntity
    {
        public long CredentialPackageId { get; set; }

        public string Id { get; set; }
        public string Name { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }

        public VerifiableCredential ParentVerifiableCredential =>
            VerifiableCredentials.FirstOrDefault(vc => vc.ParentVerifiableCredential == null);

        public List<VerifiableCredential> ChildVerifiableCredentials =>
            VerifiableCredentials
                .Where(vc => vc.ParentVerifiableCredential != null)
                .ToList();

        public List<VerifiableCredential> VerifiableCredentials { get; set; } = new();

        /// <summary>
        /// NOTE; string length matches ASPNetUsers[Id]
        /// </summary>
        [MaxLength(450)]
        public string UserId { get; set; }

        public bool IsDeleted { get; set; }

        public void Delete()
        {
            IsDeleted = true;
            ModifiedAt = DateTimeOffset.UtcNow;

            foreach (var verifiableCredential in VerifiableCredentials)
            {
                verifiableCredential.Delete();
            }
        }

    }
}