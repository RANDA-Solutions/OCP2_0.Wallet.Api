using Microsoft.AspNetCore.Authentication;
using OpenCredentialPublisher.Wallet.Models.Shared;
using System.Collections.Generic;

namespace OpenCredentialPublisher.Wallet.Models.Account
{
    public class LoginRequestModel: PostModel
    {
        public class InputRequestModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public bool RememberMe { get; set; }
            public string ReturnUrl { get; set; }
        }

        public LoginResultEnum? Result { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }
    }

    public enum LoginResultEnum
    {
        Success, TwoFactorAuthentication, Lockout, Error
    }
}
