using System.ComponentModel.DataAnnotations;

namespace OpenCredentialPublisher.Wallet.Models.Account
{
    public class PasswordResetRequestModel
    {
        public class InputRequestModel
        {

            [Required]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }

            [Required]
            public string ConfirmPassword { get; set; }
            [Required]
            public string Code { get; set; }
        }
    }
}
