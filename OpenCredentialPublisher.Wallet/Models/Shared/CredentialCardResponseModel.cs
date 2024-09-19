using System;
using System.Collections.Immutable;
using System.Linq;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Wallet.Models.Shared
{
    public record CredentialCardResponseModel
    {
        protected CredentialCardResponseModel(string userId, VerifiableCredential verifiableCredential)
        {
            VerifiableCredentialId = verifiableCredential.VerifiableCredentialId;
            Name = verifiableCredential.Name;
            CanDelete = userId == verifiableCredential.CredentialPackage?.UserId;
            Description = verifiableCredential.Description;
            IssuerName = verifiableCredential.IssuerProfile.Name;
            EffectiveImageUrl = verifiableCredential.EffectiveImageUrl;
            AchievementType = verifiableCredential.Achievement?.AchievementType;
            AchievementIdUrl = verifiableCredential.Achievement?.Id;
            HumanCode = verifiableCredential.Achievement?.HumanCode;
            CreatedAt = verifiableCredential.CreatedAt.DateTime;
            ShareCount = verifiableCredential.ShareVerifiableCredentials.Count + 
                         verifiableCredential.CredentialCollectionVerifiableCredentials.Select(ccvc => ccvc.CredentialCollection.ShareCredentialCollections).Count();
            RecipientName = verifiableCredential.Achievement?.Identifier?.DisplayName;
            LicenseNumber = verifiableCredential.Achievement?.LicenseNumber;
            EffectiveAt = verifiableCredential.EffectiveAt.DateTime;
            ExpiresAt = verifiableCredential.ValidUntilDate?.DateTime;
            Alignments = verifiableCredential.Achievement?.Alignments
                .Select(CredentialDetailsAlignmentResponseModel.FromModel).ToImmutableList() ?? ImmutableList<CredentialDetailsAlignmentResponseModel>.Empty;

            Results = verifiableCredential.Results
                .Select(CredentialDetailsResultResponseModel.FromModel).ToImmutableList();

            HasEvidence = verifiableCredential.Evidences != null && verifiableCredential.Evidences.Any();
            IsVerified = verifiableCredential.IsVerified;
            IsRevoked = verifiableCredential.IsRevoked;
            RevokedReason = verifiableCredential.RevokedReason;
        }

        public long VerifiableCredentialId { get;  }
        public string Name { get;  }
        public string Description { get;  }
        public string HumanCode { get; }
        public string IssuerName { get;  }
        public string EffectiveImageUrl { get;  }
        public string AchievementType { get;  }
        public string AchievementIdUrl { get;  }
        public DateTime CreatedAt { get;  }
        public bool CanDelete { get; }
        public int ShareCount { get;  }
        public string RecipientName { get;  }
        public string LicenseNumber { get;  }
        public DateTime EffectiveAt { get;  }
        public DateTime? ExpiresAt { get;  }

        public IImmutableList<CredentialDetailsAlignmentResponseModel> Alignments { get;  } = ImmutableList<CredentialDetailsAlignmentResponseModel>.Empty;
        public IImmutableList<CredentialDetailsResultResponseModel> Results { get; } = ImmutableList<CredentialDetailsResultResponseModel>.Empty;

        public bool HasEvidence { get; set; }
        public bool IsVerified { get; }
        public bool IsRevoked { get; set; }
        public string RevokedReason { get; set; }


        public static CredentialCardResponseModel FromModel(string userId, VerifiableCredential verifiableCredential)
        {
            return new CredentialCardResponseModel(userId, verifiableCredential);
        }
    }
}