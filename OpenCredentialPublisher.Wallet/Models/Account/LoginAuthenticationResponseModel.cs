using OpenCredentialPublisher.Wallet.Models.Shared;

namespace OpenCredentialPublisher.Wallet.Models.Account
{
    public class LoginAuthenticationResponseModel : PostModel
    {
        public string Email { get; set; }
        public LoginAuthInputResponseModel InputModel { get; set; }

        public LoginAuthenticationResultEnum? Result { get; set; }

        public string ErrorMessage { get; set; }
        public string ReturnUrl { get; set; }
    }

    public enum LoginAuthenticationResultEnum
    {
        Success, Lockout, Error, Required
    }
}
