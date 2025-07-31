using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Shared.Interfaces;

namespace OpenCredentialPublisher.Data.Custom.EFModels
{
    public class Share : IBaseEntity
    {
        public long ShareId { get; set; }

        public string ShareSecureHash { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string Email { get; set; }

        public string Description { get; set; }

        public string AccessCode { get; set; }
        [Required, MaxLength(30), DefaultValue("email")]
        public string ShareType { get; set; }

        public bool IsDeleted { get; set; }
        public List<ShareVerifiableCredential> ShareVerifiableCredentials { get; set; } = new();
        public List<ShareCredentialCollection> ShareCredentialCollections { get; set; } = new();


        public DateTimeOffset? ModifiedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        [NotMapped]
        public string ShareSecureUrl { get; set; }

        public void Delete()
        {
            IsDeleted = true;
            ModifiedAt = DateTimeOffset.UtcNow;

            foreach (var shareCredentialCollection in ShareCredentialCollections)
            {
                shareCredentialCollection.Delete();
            }

            foreach (var shareVerifiableCredential in ShareVerifiableCredentials)
            {
                shareVerifiableCredential.Delete();
            }
        }

        public int TotalCredentialCount =>
            ShareVerifiableCredentials.Select(c => c.VerifiableCredentialId).Distinct().Count()
            +
            ShareCredentialCollections
                .Select(scc => scc.CredentialCollection.CredentialCollectionVerifiableCredentials.Select(ccvc => ccvc.VerifiableCredentialId).Distinct()).Count();
    }
}