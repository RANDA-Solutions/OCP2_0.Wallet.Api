using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Wallet.Models.Account;
using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using OpenCredentialPublisher.Data.Custom.Options;
using OpenCredentialPublisher.Wallet.Models.Public;
using OpenCredentialPublisher.Wallet.Models.Shared;
using OpenCredentialPublisher.Services.Implementations;
using VerifyEmailRequestModel = OpenCredentialPublisher.Wallet.Models.Shared.VerifyEmailRequestModel;
using OpenCredentialPublisher.Wallet.Models.Revocation;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    //TODO Protect with CCG in addition to SameSite ?
    public class PublicController : ApiControllerBase<PublicController>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EmailService _emailSender;
        private readonly SiteSettingsOptions _siteSettings;
        private readonly RevocationService _revocationService;

        public PublicController(UserManager<ApplicationUser> userManager, ILogger<PublicController> logger,EmailService emailSender
            , IOptions<SiteSettingsOptions> siteSettings, RevocationService revocationService) : base (logger) 
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _siteSettings = siteSettings?.Value;
            _emailSender = emailSender;
            _revocationService = revocationService;
        }

        [HttpPost("Account/ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(VerifyEmailRequestModel vm)
        {
            var user = await _userManager.FindByIdAsync(vm.UserId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{vm.UserId}'.");
            }

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(vm.Code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            
            return ApiOk(null, result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.");
        }

        /// <summary>
        /// Gets site settings
        /// GET api/public/FooterSettings
        /// </summary>
        /// <returns>FooterSettings</returns>
        [HttpGet("FooterSettings")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public IActionResult GetFooterSettings()
        {
            try
            {
                return ApiOk(new FooterSettingsResponseModel
                {
                    ShowAddCredential = _siteSettings.ShowAddCredential,
                    ShowFooter = _siteSettings.ShowFooter,
                    ContactUsUrl = _siteSettings.ContactUsUrl,
                    PrivacyPolicyUrl = _siteSettings.PrivacyPolicyUrl,
                    TermsOfServiceUrl = _siteSettings.TermsOfServiceUrl,
                    FaqsUrl = _siteSettings.FaqsUrl

                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PublicController.GetFooterSettings");
                throw;
            }
        }

        [HttpPost("Account/Password/Forgot/{email}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return ApiOk(null);
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = new Uri($"{_siteSettings.SpaClientUrl}/access/reset-password?code={code}&email={email}", UriKind.Absolute);
                await _emailSender.SendEmailAsync(
                    email,
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl!.AbsoluteUri)}'>clicking here</a>.",true);

                return ApiOk(null);
            }

            return ApiOkModelInvalid(ModelState);
        }


        [HttpPost("Account/Confirmation/Resend/{email}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> ResendConfirmationAsync(string email)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return ApiOk(null);
                }

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl= new Uri($"{_siteSettings.SpaClientUrl}/access/email-confirmation?userId={user.Id}&code={code}", UriKind.Absolute);
                await _emailSender.SendEmailAsync(email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl.AbsoluteUri)}'>clicking here</a>.", true);

                return ApiOk(null);
            }

            return ApiOkModelInvalid(ModelState);
        }

        [HttpPost("Account/Password/Reset")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> ResetPassword(PasswordResetRequestModel.InputRequestModel input)
        {
            if (!ModelState.IsValid)
            {
                return ApiOkModelInvalid(ModelState);
            }
            if (input.ConfirmPassword != input.Password)
            {
                ModelState.AddModelError("", "Password and ConfirmPassword must match.");
            }

            if (!ModelState.IsValid)
            {
                return ApiOkModelInvalid(ModelState);
            }
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return ApiOk(new PostResponseModel());
            }

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(input.Code));
            var result = await _userManager.ResetPasswordAsync(user, code, input.Password);
            if (result.Succeeded)
            {
                return ApiOk(new PostResponseModel());
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return ApiOkModelInvalid(ModelState);
        }


        [HttpGet("credentials/{verifiableCredentialId}/revocation")]
        [ProducesResponseType(200, Type = typeof(RevocationResponseModel))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetRevocationByVerifiableCredentialId(long verifiableCredentialId)
        {
            try
            {
                Response.Headers.CacheControl = _siteSettings.GetRevocationCacheDurationHeaderString();

                //ensure user has access to this verifiable credential;
                var revocationResult = await _revocationService.CheckAndRevokeCredentialAsync(verifiableCredentialId);
                
                var revocationResponseModel = RevocationResponseModel.FromModel(revocationResult);

                return ApiOk(revocationResponseModel);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PublicController.GetRevocationByVerifiableCredentialId verifiableCredentialId: {0}", verifiableCredentialId);
                return ApiOk(RevocationResponseModel.FromModel(null));
            }
        }

        private new OkObjectResult ApiOk(object model, string message = null, string redirectUrl = null)
        {
            return Ok(new ApiOkResponse(model, message, redirectUrl));
        }
        
    }
}
