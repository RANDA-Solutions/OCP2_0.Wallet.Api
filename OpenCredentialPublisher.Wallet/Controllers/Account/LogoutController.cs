using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Models;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Controllers.Account
{
    public class LogoutController : ApiControllerBase<LogoutController>
    {
        
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LogoutController(SignInManager<ApplicationUser> signInManager
            , ILogger<LogoutController> logger)
            : base(logger)
        {
            _signInManager = signInManager;
        }

        [HttpPost, Route("")]
        public async Task<IActionResult> PostAsync()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return ApiOk(null);
        }
    }
}
