using System.ComponentModel.DataAnnotations;

namespace OpenCredentialPublisher.Wallet.Models.Account
{
    public class VerificationNeededRequestModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}