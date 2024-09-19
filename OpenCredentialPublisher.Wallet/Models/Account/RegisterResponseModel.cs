using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Models.Account
{
    public class RegisterResponseModel
    {

        public RegisterResultEnum? Result { get; set; }
        public string[] ErrorMessages { get; set; }
        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }
    }

    public enum RegisterResultEnum
    {
        Success = 1, ConfirmEmail = 2, Error = 3
    }
}
