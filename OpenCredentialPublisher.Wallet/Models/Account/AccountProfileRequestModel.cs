using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace OpenCredentialPublisher.Wallet.Models.Account
{
    public class AccountProfileRequestModel
    {
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [MaxLength(255)]
        [Display(Name = "Displayable Name")]
        public string DisplayName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public string ProfileImageUrl { get; set; }

        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public IFormFile ProfileImageFile { get; set; }
    }
}