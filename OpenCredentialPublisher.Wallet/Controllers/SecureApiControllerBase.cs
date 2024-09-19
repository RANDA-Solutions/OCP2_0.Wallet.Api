using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Models;
using System;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SecureApiControllerBase<T> : ApiControllerBase<T>
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        protected string _userId => UserId;
        public SecureApiControllerBase(UserManager<ApplicationUser> userManager, ILogger<T> logger): base(logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
    }
}
