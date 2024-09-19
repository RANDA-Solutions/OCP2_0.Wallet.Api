using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Models;
using System;
using System.Threading.Tasks;
using OpenCredentialPublisher.Wallet.Models.Connect;
using OpenCredentialPublisher.Services.Implementations;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    public class ConnectController : SecureApiControllerBase<ConnectController>
    {
        private readonly ConnectService _connectService;

        public ConnectController(
            UserManager<ApplicationUser> userManager, 
            ILogger<ConnectController> logger, 
            ConnectService connectService
            ) : base(userManager, logger)
        {
            _connectService = connectService;
        }

        
        [AllowAnonymous]
        [HttpPost, Route("External")]
        public async Task<IActionResult> ExternalAsync([FromBody] ConnectRequestModel model)
        {
            try
            {
                
                var result = await _connectService.ConnectExternalAsync(this, model.ToCommand());
                if (result.HasError)
                {
                    foreach (var err in result.ErrorMessages)
                    {
                        ModelState.AddModelError(string.Empty, err);
                    }

                    return ApiErrorModelInvalid(ModelState);
                }
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"Endpoint: {model?.Endpoint}, Scope:{model?.Scope}, Payload:{model?.Payload}, Issuer:{model?.Issuer}, Method:{model?.Method}");
                return StatusCode(500, "An unexpected error occured.  Please check the Wallet logs for details.");
            }
        }

        //public record UserRequest(string Email);
        //[AllowAnonymous]
        //[HttpPost, Route("UserTest")]
        //public async Task<IActionResult> UserTestAsync([FromBody] UserRequest request)
        //{
        //    try
        //    {

        //        var result = await _connectService.GetOrCreateUserAsync(request.Email, request.Email);
        //        return new JsonResult(result);
        //    }
        //    catch (Exception ex) 
        //    {
        //        //_logger.LogError(ex, $"Endpoint: {model?.Endpoint}, Scope:{model?.Scope}, Payload:{model?.Payload}, Issuer:{model?.Issuer}, Method:{model?.Method}");
        //        return StatusCode(500, "An unexpected error occured.  Please check the Wallet logs for details.");
        //    }
        //}
        //[AllowAnonymous]
        //[HttpPost("prooftest/{id}")]
        //public async Task<IActionResult> ProofTest(long id)
        //{
        //    try
        //    {
        //        var vc = await _context.VerifiableCredentials2.FirstOrDefaultAsync(x => x.VerifiableCredentialId == id);
        //        var valid = await _proofServiceV2.VerifyProof(vc.Json);
        //        return new JsonResult(valid);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "There was a problem processing this request.", null);
        //        return new JsonResult(new PostModel { ErrorMessages = new List<string> { ex.Message } });
        //    }
        //}
    }
}
