using Microsoft.AspNetCore.Mvc.Filters;
using OpenCredentialPublisher.Data.Models;

namespace OpenCredentialPublisher.Wallet.Middleware
{
    public class ValidatorActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.ModelState.IsValid)
            {
                throw new ApiModelValidationException(filterContext.ModelState);
                // filterContext.Result = new BadRequestObjectResult(filterContext.ModelState);
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}
