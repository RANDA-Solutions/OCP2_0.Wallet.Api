using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using OpenCredentialPublisher.Wallet.Models.Shared;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiControllerBase<T> : ControllerBase
    {
        protected String UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
        protected readonly ILogger<T> _logger;

        public ApiControllerBase(ILogger<T> logger) {
            _logger = logger;
        }

        protected OkObjectResult ApiOk(object model, string message = null, string redirectUrl = null)
        {
            return Ok(new ApiOkResponse(model, message, redirectUrl));
        }

        protected OkObjectResult ApiOkCustom(int statusCode, object model, string message = null, string redirectUrl = null)
        {
            return Ok(new ApiCustomResponse(statusCode, model, message, redirectUrl));
        }

        protected OkObjectResult ApiOkCustomInvalid(int statusCode, ModelStateDictionary modelState)
        {
            return Ok(new ApiCustomResponse(statusCode, modelState));
        }


        protected OkObjectResult ApiOkModelInvalid(ModelStateDictionary modelState)
        {
            return Ok(new ApiBadRequestResponse(modelState));
        }

        protected IActionResult ApiErrorModelInvalid(ModelStateDictionary modelState)
        {
            return BadRequest(modelState);
        }
    }
}
