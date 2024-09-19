using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Custom.Options;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Wallet.Models.Account;
using OpenCredentialPublisher.Wallet.Models.Shared;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using VerifyEmailRequestModel = OpenCredentialPublisher.Wallet.Models.Shared.VerifyEmailRequestModel;

namespace OpenCredentialPublisher.Wallet.Controllers.Account
{
    public class AccountController : SecureApiControllerBase<AccountController>
    {
        private readonly IEmailSender _emailSender;
        private readonly ProfileImageService _profileImageService;

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly SiteSettingsOptions _siteSettings;

        public AccountController(UserManager<ApplicationUser> userManager,
            ILogger<AccountController> logger,
            ProfileImageService profileImageService,
            IEmailSender emailSender,
            SignInManager<ApplicationUser> signInManager,
            IOptions<SiteSettingsOptions> siteSettings) : base(userManager, logger)
        {
            _profileImageService = profileImageService;
            _signInManager = signInManager;
            _siteSettings = siteSettings?.Value ?? throw new NullReferenceException("Site settings were not set.");
            _emailSender = emailSender;
        }

        //[HttpPost]
        //[Route("deleteUser")]
        //public async Task<IActionResult> DeleteUser()
        //{
        //    var user = await _userManager.FindByIdAsync(_userId);
        //    if (user == null)
        //    {
        //        return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        //    }

        //    await _forgetMeService.ForgetUser(user.Id);

        //    if (await _profileImageService.DeleteImageFromBlobAsync(user.ProfileImageUrl))
        //    {
        //        user.ProfileImageUrl = null;
        //        await _userManager.UpdateAsync(user);
        //    }

        //    await _userManager.DeleteAsync(user);
        //    try
        //    {
        //        await _signInManager.SignOutAsync();
        //    }
        //    catch(Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message, user);
        //    }

        //    return Ok();
        //}

        //[HttpPost("saveProfileImage"), DisableRequestSizeLimit]
        //public async Task<IActionResult> SaveProfileImage([FromForm] FileInput theFile)
        //{
        //    try
        //    {
        //        var file = Request.Form.Files[0];//formCollection.Files.First();

        //        using var image = Image.Load(file.OpenReadStream());
        //        image.Mutate(x => x.Resize(150, 150));
        //        byte[] imageBytes;
        //        using (var ms = new MemoryStream())
        //        {
        //            await image.SaveAsPngAsync(ms);
        //            imageBytes = ms.ToArray();
        //        }

        //        var url = await _profileImageService.SaveImageToBlobAsync(_userId, imageBytes);
        //        return ApiOk(url);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //    }
        //    return BadRequest("Error setting saving image.");
        //}

        //[HttpPost("removeProfileImage")]
        //public async Task<IActionResult> RemoveProfileImage()
        //{
        //    try
        //    {
        //        var user = await _userManager.FindByIdAsync(_userId);
        //        if (user == null)
        //        {
        //            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        //        }

        //        var result = await _profileImageService.DeleteImageFromBlobAsync(user.ProfileImageUrl);
        //        user.ProfileImageUrl = null;
        //        await _userManager.UpdateAsync(user);

        //        return ApiOk(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //    }
        //    return BadRequest("Error setting saving image.");
        //}

        [AllowAnonymous]
        [HttpPost("ConfirmEmail/{userId}")]
        public async Task<IActionResult> ConfirmEmailAsync(string userId, [FromQuery(Name = "code")] string code)
        {
            if (userId == null || code == null)
            {
                ModelState.AddModelError("ConfirmEmail", "Passwords do not match.");
                return ApiOkModelInvalid(ModelState);
            }

            var user = await _userManager.FindByIdAsync(userId!);
            if (user == null)
            {
                ModelState.AddModelError("ConfirmEmail", $"Unable to load user with ID '{userId}'.");
                return ApiOkModelInvalid(ModelState);
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var result = await _userManager.ConfirmEmailAsync(user!, code);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("ConfirmEmail", "Error confirming your email.");
                return ApiOkModelInvalid(ModelState);
            }

            return ApiOk("Thank you for confirming your email.");
        }

        [AllowAnonymous]
        [HttpPost("ConfirmEmailChange")]
        public async Task<IActionResult> ConfirmEmailChangeAsync(VerifyEmailRequestModel vm)
        {
            if (vm.UserId == null || vm.Email == null || vm.Code == null)
            {
                ModelState.AddModelError("", "Incomplete verification information.");
                return ApiOkModelInvalid(ModelState);
            }

            var user = await _userManager.FindByIdAsync(vm.UserId);
            if (user == null) return NotFound($"Unable to load user with ID '{vm.UserId}'.");

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(vm.Code));
            var result = await _userManager.ChangeEmailAsync(user, vm.Email, code);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Error changing email.");
                return ApiOkModelInvalid(ModelState);
            }

            await _userManager.SetUserNameAsync(user, vm.Email);
            await _signInManager.RefreshSignInAsync(user);

            return ApiOk("Thank you for confirming your email change.");
        }

        [HttpGet("GetProfile")]
        public async Task<IActionResult> GetProfileAsync()
        {
            var user = await _userManager.FindByIdAsync(_userId);
            if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            return ApiOk(new AccountProfileResponseModel
            {
                DisplayName = user.DisplayName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user),
                HasProfileImage = user.HasProfileImageUrl,
                EnableEditEmail = _siteSettings.EnableEditEmail
            });
        }

        [HttpGet]
        [Route("GetProfileImage")]
        public async Task<IActionResult> GetProfileImageAsync()
        {
            var user = await _userManager.FindByIdAsync(_userId);
            if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            return ApiOk(user.ProfileImageUrl);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterAccountRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return ApiOkModelInvalid(ModelState);
            }

            var modelState = new ModelStateDictionary();

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, DisplayName = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl  = string.IsNullOrEmpty(model.ReturnUrl) ?
                    new Uri(
                        $"{_siteSettings.SpaClientUrl}/access/email-confirmation?userId={user.Id}&code={code}",
                        UriKind.Absolute)
                :
                    new Uri(
                        $"{_siteSettings.SpaClientUrl}/access/email-confirmation?userId={user.Id}&code={code}&returnUrl={HttpUtility.UrlEncode(model.ReturnUrl)}",
                        UriKind.Absolute);

                var confirmUrl = new Uri(
                    $"{_siteSettings.SpaClientUrl}/access/register-confirmation?userId={user.Id}&code={code}",
                    UriKind.Absolute);

                await _emailSender.SendEmailAsync(model.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl.AbsoluteUri)}'>clicking here</a>.");

                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    return ApiOk(RegisterResultEnum.ConfirmEmail, null, confirmUrl.PathAndQuery);

                Uri mainUrl;
                if (string.IsNullOrEmpty(model.ReturnUrl))
                    Uri.TryCreate($"{_siteSettings.SpaClientUrl}/credentials", UriKind.Absolute,
                        out mainUrl);
                else if (Uri.IsWellFormedUriString(model.ReturnUrl, UriKind.Relative))
                    Uri.TryCreate($"{_siteSettings.SpaClientUrl}{model.ReturnUrl}",
                        UriKind.Absolute, out mainUrl);
                else
                    Uri.TryCreate($"{model.ReturnUrl}", UriKind.Absolute, out mainUrl);

                await _signInManager.SignInAsync(user, false);

                return ApiOk(RegisterResultEnum.Success, null, mainUrl?.PathAndQuery);
            }

            var errorMessages = result.Errors.Select(err => err.Description);
            foreach (var errorMessage in errorMessages)
            {
                modelState.AddModelError("", errorMessage);
            }

            return ApiOkModelInvalid(modelState);
        }

        [HttpPost("SaveProfile")]
        public async Task<IActionResult> SaveProfileAsync([FromForm] AccountProfileRequestModel model)
        {
            var modelState = new ModelStateDictionary();
            var statusMessageStringBuilder = new StringBuilder();

            var user = await _userManager.FindByIdAsync(_userId);
            if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            var actionResult = await SaveProfileDisplayNameAsync(model, user, modelState);
            if (actionResult != null) return actionResult;

            actionResult = await SaveProfilePhoneNumberAsync(model, user, modelState);
            if (actionResult != null) return actionResult;

            actionResult = await SaveProfileEmailAsync(model, user, statusMessageStringBuilder);
            if (actionResult != null) return actionResult;

            actionResult = await SaveProfileImageAsync(model, user, modelState);
            if (actionResult != null) return actionResult;

            actionResult = await SaveProfilePasswordAsync(model, user, modelState);
            if (actionResult != null) return actionResult;

            return ApiOk(new AccountProfileResponseModel
            {
                DisplayName = user.DisplayName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user),
                HasProfileImage = user.HasProfileImageUrl
            }, statusMessageStringBuilder.ToString());
        }

        [HttpGet]
        [Route("VerificationEmail")]
        public async Task<IActionResult> SendVerificationEmailAsync()
        {
            var user = await _userManager.FindByIdAsync(_userId);
            if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");


            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUri =
                new Uri(
                    $"{_siteSettings.SpaClientUrl}/access/email-confirmation?code={code}&userId={userId}",
                    UriKind.Absolute);

            await _emailSender.SendEmailAsync(
                email!,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUri.AbsoluteUri)}'>clicking here</a>.");

            return ApiOk(null, "Verification email sent. Please check your email.");
        }

        private async Task<IActionResult> SaveProfileDisplayNameAsync(AccountProfileRequestModel model,
            ApplicationUser user,
            ModelStateDictionary modelState)
        {
            if (model.DisplayName != user.DisplayName)
            {
                if (string.IsNullOrWhiteSpace(model.DisplayName))
                {
                    modelState.AddModelError(nameof(AccountProfileRequestModel.DisplayName),
                        "Please provide a display name.");
                    return ApiOkModelInvalid(modelState);
                }

                user.DisplayName = model.DisplayName.Trim();
                var setDisplayNameResult = await _userManager.UpdateAsync(user);
                if (!setDisplayNameResult.Succeeded)
                {
                    foreach (var identityError in setDisplayNameResult.Errors)
                    {
                        var errorDescription = identityError.Description;
                        if (!string.IsNullOrEmpty(errorDescription))
                            modelState.AddModelError(nameof(AccountProfileRequestModel.DisplayName),
                                errorDescription);
                    }

                    return ApiOkModelInvalid(modelState);
                }
            }

            return null;
        }

        private async Task<IActionResult> SaveProfileEmailAsync(AccountProfileRequestModel model,
            ApplicationUser user, StringBuilder statusMessageStringBuilder)
        {
            var email = await _userManager.GetEmailAsync(user);
            // only if email changed AND we support editing email
            if (model.Email != email && _siteSettings.EnableEditEmail)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, model.Email);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUri =
                    new Uri(
                        $"{_siteSettings.SpaClientUrl}/access/confirm-email-change?code={code}&userId={userId}&email={model.Email}",
                        UriKind.Absolute);
                await _emailSender.SendEmailAsync(
                    model.Email,
                    "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUri.AbsoluteUri)}'>clicking here</a>.");

                statusMessageStringBuilder.AppendLine(
                    "A confirmation email was sent to your changed email. Please check your email and click to confirm.");
            }

            return null;
        }

        private async Task<IActionResult> SaveProfileImageAsync(AccountProfileRequestModel model,
            ApplicationUser user, ModelStateDictionary modelState)
        {
            if (model.ProfileImageFile != null)
            {
                using var image = await Image.LoadAsync(model.ProfileImageFile.OpenReadStream());
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(150, 150),
                    Mode = ResizeMode.Max
                }));

                byte[] imageBytes;
                using (var ms = new MemoryStream())
                {
                    await image.SaveAsPngAsync(ms);
                    imageBytes = ms.ToArray();
                }

                user.ProfileImageUrl =
                    await _profileImageService.SaveImageToBlobAsync(_userId, user.ProfileImageUrl, imageBytes);
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    foreach (var identityError in updateResult.Errors)
                    {
                        var errorDescription = identityError.Description;
                        if (!string.IsNullOrEmpty(errorDescription))
                            modelState.AddModelError(nameof(AccountProfileRequestModel.ProfileImageFile),
                                errorDescription);
                    }

                    return ApiOkModelInvalid(modelState);
                }
            }

            return null;
        }

        private async Task<IActionResult> SaveProfilePasswordAsync(AccountProfileRequestModel model,
            ApplicationUser user, ModelStateDictionary modelState)
        {
            if (!string.IsNullOrEmpty(model.CurrentPassword) && !string.IsNullOrEmpty(model.NewPassword))
            {
                if (model.NewPassword != model.ConfirmPassword)
                {
                    modelState.AddModelError(nameof(AccountProfileRequestModel.ConfirmPassword),
                        "New and confirm passwords must match.");
                    return ApiOkModelInvalid(modelState);
                }

                var changePasswordResult =
                    await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    foreach (var identityError in changePasswordResult.Errors)
                    {
                        var errorDescription = identityError.Description;
                        if (!string.IsNullOrEmpty(errorDescription))
                            modelState.AddModelError(nameof(AccountProfileRequestModel.NewPassword),
                                errorDescription);
                    }

                    return ApiOkModelInvalid(modelState);
                }
            }

            return null;
        }

        private async Task<IActionResult> SaveProfilePhoneNumberAsync(AccountProfileRequestModel model,
            ApplicationUser user,
            ModelStateDictionary modelState)
        {
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (model.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    foreach (var identityError in setPhoneResult.Errors)
                    {
                        var errorDescription = identityError.Description;
                        if (!string.IsNullOrEmpty(errorDescription))
                            modelState.AddModelError(nameof(AccountProfileRequestModel.DisplayName),
                                errorDescription);
                    }

                    return ApiOkModelInvalid(modelState);
                }
            }

            return null;
        }
    }
}