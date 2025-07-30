using System;
using System.Collections.Generic;
using OpenCredentialPublisher.Shared.Interfaces;

// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength

namespace OpenCredentialPublisher.Data.Custom.EFModels
{
    public class VerifiableCredential : IBaseEntity
    {
        public long VerifiableCredentialId { get; set; }

        public long? ParentVerifiableCredentialId { get; set; }
        public VerifiableCredential ParentVerifiableCredential { get; set; }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Type { get; set; }
        public DateTimeOffset? AwardedDate { get; set; }
        public DateTimeOffset ValidFromDate { get; set; }
        public DateTimeOffset? ValidUntilDate { get; set; }
        public long IssuerProfileId { get; set; }
        public Profile IssuerProfile { get; set; } // Navigation property
        public string ImageUrl { get; set; }
        public string Json { get; set; }

        public long CredentialPackageId { get; set; } // Foreign key property
        public CredentialPackage CredentialPackage { get; set; } // Navigation property

        public Achievement Achievement { get; set; }
        public List<Evidence> Evidences { get; set; } = new();
        public List<Association> SourceAssociations { get; set; } = new();
        public List<Association> TargetAssociations { get; set; } = new();

        public bool IsChild { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
        public bool IsVerified { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsRevoked { get; set;  }
        public string RevokedReason { get; set;  }

        public void Delete()
        {
            IsDeleted = true;
            ModifiedAt = DateTimeOffset.UtcNow;

            Achievement?.Delete();

            foreach (var evidence in Evidences)
            {
                evidence.Delete();
            }

            // remove from any collections, too
            foreach (var credentialCollectionVerifiableCredential in CredentialCollectionVerifiableCredentials)
            {
                credentialCollectionVerifiableCredential.Delete();
            }

            // remove from any shares, too
            foreach (var shareVerifiableCredential in ShareVerifiableCredentials)
            {
                shareVerifiableCredential.Delete();
            }

            foreach (var result in Results)
            {
                result.Delete();
            }

        }

        public List<CredentialCollectionVerifiableCredential> CredentialCollectionVerifiableCredentials { get; set; } =
            new();

        public List<ShareVerifiableCredential> ShareVerifiableCredentials { get; set; } = new();

        public List<Result> Results { get; set; } = new();


        public DateTimeOffset EffectiveAt => AwardedDate ?? ValidFromDate; 

        /// <summary>
        /// Use image in this order: achievement subject -> achievement -> credential -> issuer
        /// </summary>
        public string EffectiveImageUrl => Achievement?.ImageUrl ?? ImageUrl ?? IssuerProfile?.ImageUrl;


    }
}
