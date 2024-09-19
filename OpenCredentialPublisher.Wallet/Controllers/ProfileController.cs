using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Models;
using System.Threading.Tasks;
using OpenCredentialPublisher.Wallet.Models.Profile;
using OpenCredentialPublisher.Services.Implementations;

namespace OpenCredentialPublisher.Wallet.Controllers
{

    public class ProfileController : SecureApiControllerBase<ProfileController>
    {

        private readonly CredentialService _credentialService;
        public ProfileController(UserManager<ApplicationUser> userManager, 
            ILogger<ProfileController> logger, CredentialService credentialService) : base(userManager, logger)
        {
            _credentialService = credentialService;
        }

        /// <summary>
        /// Gets all UserPreferences for the current user
        /// GET api/userprefs
        /// </summary>
        /// <returns>Array of UserPreferences (Name/Value)</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(UserProfileResponseModel))]  /* success returns 200 - Ok */
        public async Task<IActionResult> Get()
        {
            var appUser = await _userManager.FindByIdAsync(_userId);
            if (appUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // TODO: get from CLR2 models
            var achievementCount = await _credentialService.GetAchievementCountAsync(_userId);
            var credentialCount = await _credentialService.GetVerifiableCredentialCountAsync(_userId);
            var resultsCount = await _credentialService.GetResultsCountAsync(_userId);
            return ApiOk(new UserProfileResponseModel
            {
                DisplayName = appUser.DisplayName,
                Email = appUser.Email,
                HasProfileImage = appUser.HasProfileImageUrl,
                ProfileImageUrl = appUser.ProfileImageUrl,
                Credentials = credentialCount,
                Achievements = achievementCount,
                Scores = resultsCount,
                ActiveLinks = 0
            });
        }
    }
}
