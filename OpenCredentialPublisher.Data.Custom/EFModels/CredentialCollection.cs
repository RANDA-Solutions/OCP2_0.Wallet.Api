using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OpenCredentialPublisher.Shared.Interfaces;

namespace OpenCredentialPublisher.Data.Custom.EFModels
{
    public class CredentialCollection : IBaseEntity
    {
        public long CredentialCollectionId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// NOTE; string length matches ASPNetUsers[Id]
        /// </summary>
        [MaxLength(450)]
        public string UserId { get; set; }

        public List<CredentialCollectionVerifiableCredential> CredentialCollectionVerifiableCredentials { get; set; } = new();
        public List<ShareCredentialCollection> ShareCredentialCollections { get; set; } = new();

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        public void Delete()
        {
            IsDeleted = true;
            ModifiedAt = DateTimeOffset.UtcNow;

            // loop over children and delete
            foreach (var credentialCollectionVerifiableCredential in CredentialCollectionVerifiableCredentials)
            {
                credentialCollectionVerifiableCredential.Delete();
            }

            // remove from any shares
            // loop over children and delete
            foreach (var shareCredentialCollection in ShareCredentialCollections)
            {
                shareCredentialCollection.Delete();
            }

        }
    }
}
