using OpenCredentialPublisher.Shared.Interfaces;
using System;
using System.Collections.Generic;

namespace OpenCredentialPublisher.Data.Custom.EFModels
{
    public class Achievement: IBaseEntity
    {
        public const string UNSPECIFIED_ACHIEVEMENT_TYPE = "Unspecified";

        public long AchievementId { get; set; }

        public string Id { get; set; }
        public string AchievementType { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string HumanCode { get; set; }
        public string FieldOfStudy { get; set; }
        public string LicenseNumber { get; set; }
        public string ImageUrl { get; set; }

        public List<string> Type { get; set; }

        public List<AchievementAlignment> Alignments { get; set; } = new();

        public long? CreatorProfileId { get; set; }
        public Profile Creator { get; set; }

        //from the subject.
        public long? SourceProfileId { get; set; }
        public Profile Source { get; set; }

        public long VerifiableCredentialId { get; set; }
        public VerifiableCredential VerifiableCredential { get; set; }

        public AchievementIdentity Identifier { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        public void Delete()
        {
            IsDeleted = true;
            ModifiedAt = DateTimeOffset.UtcNow;

            foreach (var alignment in Alignments)
            {
                alignment.Delete();
            }

            // NOTE: Creator Profile, Source Profile, and Identifier are not deleted since could be referenced by others
        }
    }
}