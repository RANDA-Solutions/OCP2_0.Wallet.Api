using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpenCredentialPublisher.Shared.Interfaces;

// NOTE: Hack on namespace so partial class works
namespace OpenCredentialPublisher.Data.Models
{
    public class ApplicationUser: IdentityUser, IBaseEntity
    {
        [MaxLength(255)]
        public string DisplayName { get; set; }

		public string ProfileImageUrl { get; set; }

        [NotMapped]
        public bool HasProfileImageUrl => !string.IsNullOrEmpty(ProfileImageUrl);

        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
    }
}
