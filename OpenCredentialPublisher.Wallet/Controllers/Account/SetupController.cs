using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OpenCredentialPublisher.Wallet.Models.Account;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Wallet.Controllers.Account
{
    [Route("api/account/[controller]")]
    public class SetupController : ApiControllerBase<SetupController>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SetupService _setupService;

        public SetupController(ILogger<SetupController> logger,
            UserManager<ApplicationUser> userManager, 
            SetupService setupService) : base(logger)
        {
            _userManager = userManager;
            _setupService = setupService;
        }

        [HttpGet("Email")]
        public async Task<IActionResult> VerifyEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var setupModel = new VerifyEmailResponseModel
            {
                Email = email,
                Status = GetAgentSetupStatusInitial(user)
            };

            return ApiOk(setupModel);
        }

        [HttpPost("Email/AccessCode")]
        public async Task<IActionResult> GetAccessCodeEmailAsync(VerificationNeededRequestModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            var setupModel = new VerifyEmailResponseModel
            {
                Email = model.Email,
                Status = GetAgentSetupStatus(user)
            };

            if(setupModel.Status == AccountSetupStatusEnum.VerifyEmailNeeded)
            {
                // create setup link and email access code
                await _setupService.GenerateAndSendAccessCodeAsync(user);
            }

            return ApiOk(setupModel);
        }

        [HttpPost("Email")]
        public async Task<IActionResult> VerifyEmailAsync([FromBody] VerifyEmailRequestModel model)
        {
            if (!ModelState.IsValid)
                return ApiOkModelInvalid(ModelState);

            var accessCodeStatus = await _setupService.VerifyAccessCodeAsync(model.Email,model.AccessCode);

            var verifyEmailResponseModel = new VerifyEmailResponseModel
            {
                Email = model.Email,
            };

            return UpdateStatusAndGetResult(accessCodeStatus, verifyEmailResponseModel);
        }


        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]AccountSetupRequestModel model)
        {
            var modelState = new ModelStateDictionary();

            if (model.Password != model.ConfirmPassword)
            {
                modelState.AddModelError(nameof(AccountSetupRequestModel.ConfirmPassword), "Passwords do not match.");
            }
            if (string.IsNullOrEmpty(model.DisplayName))
            {
                modelState.AddModelError(nameof(AccountSetupRequestModel.DisplayName), "Name is required.");
            }

            if (modelState.ErrorCount > 0)
            {
                return ApiOkModelInvalid(modelState);
            }

            var verifyEmailResponseModel = new VerifyEmailResponseModel
            {
                Email = model.Email,
            };

            var accessCodeStatus = await _setupService.UpdateUserAsync(model.Email, model.AccessCode, model.DisplayName, model.Password, modelState);

            if (accessCodeStatus == AccessCodeStatusEnum.AccountUpdateFailed)
            {
                return ApiOkModelInvalid(modelState);
            }

            return UpdateStatusAndGetResult(accessCodeStatus, verifyEmailResponseModel);
        }

        private AccountSetupStatusEnum GetAgentSetupStatus(ApplicationUser user)
        {
            if (user == null)
            {
                return AccountSetupStatusEnum.AccountNotFound;
            }

            if (string.IsNullOrEmpty(user.PasswordHash) && !user.EmailConfirmed)
            {
                return AccountSetupStatusEnum.VerifyEmailNeeded;
            }

            if (string.IsNullOrEmpty(user.PasswordHash) && user.EmailConfirmed)
            {
                return AccountSetupStatusEnum.AccountSetupNeeded;
            }

            return AccountSetupStatusEnum.AccountComplete;
        }

        private AccountSetupStatusEnum GetAgentSetupStatusInitial(ApplicationUser user)
        {
            if (user == null)
            {
                return AccountSetupStatusEnum.AccountNotFound;
            }

            if (string.IsNullOrEmpty(user.PasswordHash) && !user.EmailConfirmed)
            {
                return AccountSetupStatusEnum.VerifyEmailRequestNeeded;
            }

            if (string.IsNullOrEmpty(user.PasswordHash) && user.EmailConfirmed)
            {
                return AccountSetupStatusEnum.AccountSetupNeeded;
            }

            return AccountSetupStatusEnum.AccountComplete;
        }


        private IActionResult UpdateStatusAndGetResult(AccessCodeStatusEnum accessCodeStatus,
            VerifyEmailResponseModel verifyEmailResponseModel)
        {
            switch (accessCodeStatus)
            {
                case AccessCodeStatusEnum.NotFound:
                    ModelState.AddModelError(nameof(VerifyEmailRequestModel.AccessCode), "The required setup information could not be found.");
                    return ApiOkCustomInvalid((int)HttpStatusCode.NotFound, ModelState);
                case AccessCodeStatusEnum.AccountComplete:
                    verifyEmailResponseModel.Status = AccountSetupStatusEnum.AccountComplete;
                    return ApiOk(verifyEmailResponseModel);
                case AccessCodeStatusEnum.Valid:
                    verifyEmailResponseModel.Status = AccountSetupStatusEnum.AccountSetupNeeded;
                    return ApiOk(verifyEmailResponseModel);
                case AccessCodeStatusEnum.Expired:
                    verifyEmailResponseModel.Status = AccountSetupStatusEnum.AccessCodeExpired;
                    return ApiOk(verifyEmailResponseModel);
                case AccessCodeStatusEnum.Invalid:
                default:
                    ModelState.AddModelError(nameof(VerifyEmailRequestModel.AccessCode), $"Access code is {accessCodeStatus.ToString().ToLowerInvariant()}.");
                    return ApiOkModelInvalid(ModelState);
            }
        }

    }


}
