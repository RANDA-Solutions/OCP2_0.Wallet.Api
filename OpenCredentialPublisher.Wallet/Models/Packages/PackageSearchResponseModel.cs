using System;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Wallet.Models.Packages
{
    public record PackageSearchResponseModel
    {
        protected PackageSearchResponseModel(SearchCredentialPackage searchCredentialPackage, string currentUserId)
        {
            CredentialPackageId = searchCredentialPackage.CredentialPackageId;
            CanDelete = currentUserId == searchCredentialPackage.OwnerUserId;
            Name = searchCredentialPackage.Name;
            IssuerName = searchCredentialPackage.IssuerName;
            EffectiveImageUrl = searchCredentialPackage.EffectiveImageUrl;
            AchievementCount = searchCredentialPackage.AchievementCount;
            ShareCount = searchCredentialPackage.ShareCount;
            EffectiveAt = searchCredentialPackage.EffectiveAt.DateTime;
            ExpiresAt = searchCredentialPackage.ExpiresAt?.DateTime;
            CreatedAt = searchCredentialPackage.CreatedAt.DateTime;
            IsVerified = searchCredentialPackage.IsVerified;
            IsRevoked = searchCredentialPackage.IsRevoked;
            RevokedReason = searchCredentialPackage.RevokedReason;
            VerifiableCredentialId = searchCredentialPackage.VerifiableCredentialId;
        }


        public long CredentialPackageId { get; }
        public long VerifiableCredentialId { get; }
        public bool CanDelete { get; }
        public bool IsVerified { get; }
        public bool IsRevoked { get; set; }
        public string RevokedReason { get; set; }

        public string Name { get; }
        public string IssuerName { get; }
        public string EffectiveImageUrl { get; }
        public int AchievementCount { get;}
        public int ShareCount { get; }
        public DateTime EffectiveAt { get; }
        public DateTime? ExpiresAt { get; }
        public DateTime CreatedAt { get; }

        public static PackageSearchResponseModel FromModel(string currentUserId,
            SearchCredentialPackage searchCredential)
        {
            return new PackageSearchResponseModel(searchCredential, currentUserId);
        }
    };
}