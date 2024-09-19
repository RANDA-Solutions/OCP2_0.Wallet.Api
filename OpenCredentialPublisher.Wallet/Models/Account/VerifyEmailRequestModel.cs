using System.ComponentModel.DataAnnotations;

namespace OpenCredentialPublisher.Wallet.Models.Account
{
    public class VerifyEmailRequestModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string AccessCode { get; set; }
    }

}