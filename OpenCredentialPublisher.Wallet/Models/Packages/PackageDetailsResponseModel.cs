using System;
using System.Collections.Immutable;
using System.Linq;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Wallet.Models.Packages
{
    public record PackageDetailsResponseModel
    {
        protected PackageDetailsResponseModel(CredentialPackage credentialPackage, string currentUserId)
        {
            CredentialPackageId = credentialPackage.CredentialPackageId;
            CanDelete = credentialPackage.UserId == currentUserId;
            Name = credentialPackage.Name;
            Description = credentialPackage.ParentVerifiableCredential.Description;
            CreatedAt = credentialPackage.CreatedAt.DateTime;
            ShareCount = credentialPackage.VerifiableCredentials.SelectMany(vc => vc.ShareVerifiableCredentials).Count() + 
                         credentialPackage.VerifiableCredentials.SelectMany(vc => vc.CredentialCollectionVerifiableCredentials).Select(ccvc => ccvc.CredentialCollection.ShareCredentialCollections).Count();
            VerifiableCredentialIds = credentialPackage.ChildVerifiableCredentials
                .Select(cvc => cvc.VerifiableCredentialId)
                .ToImmutableList();
            IsVerified = credentialPackage.ParentVerifiableCredential.IsVerified;
            IsRevoked = credentialPackage.ParentVerifiableCredential.IsRevoked;
            RevokedReason = credentialPackage.ParentVerifiableCredential.RevokedReason;
            VerifiableCredentialId = credentialPackage.ParentVerifiableCredential.VerifiableCredentialId;
        }

        public long CredentialPackageId { get; }
        public bool CanDelete { get; }
        public bool IsVerified{ get; }
        public bool IsRevoked { get; set; }
        public string RevokedReason { get; set; }
        public long VerifiableCredentialId { get; }
        public string Name { get; }
        public string Description { get; }
        public DateTime CreatedAt { get; }
        public int ShareCount { get;  }
        public IImmutableList<long> VerifiableCredentialIds { get; } = ImmutableList<long>.Empty;

        public static PackageDetailsResponseModel FromModel(CredentialPackage credentialPackage, string
            currentUserId)
        {
            return new PackageDetailsResponseModel(credentialPackage, currentUserId);
        }
    }

}
