using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenCredentialPublisher.Data.Options;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Wallet.Models.Account;
using Swashbuckle.AspNetCore.Annotations;

namespace OpenCredentialPublisher.Wallet.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Handles user authentication, including login and token refresh.")]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtConfiguration _jwtConfiguration;

        public AuthenticateController(
            UserManager<ApplicationUser> userManager,
            IOptions<JwtConfiguration> jwtConfigurationOptions)
        {
            _userManager = userManager;
            _jwtConfiguration = jwtConfigurationOptions.Value;
        }

        [HttpPost]
        [Route("login")]
        [SwaggerOperation(
            Summary = "Authenticate a user and generate a JWT token.",
            Description = "Validates the user's credentials and returns a JWT token if successful."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Authentication successful.", typeof(object))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Authentication failed.")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel.InputRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email) ?? await _userManager.FindByEmailAsync(model.Email);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    if (user.NormalizedEmail == null)
                        throw new Exception("Email is null");

                    var userRoles = await _userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                    {
                        new(ClaimTypes.NameIdentifier, user.Id),
                        new(ClaimTypes.Name, user.NormalizedEmail),
                        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var token = GetToken(authClaims);

                    return Ok(new
                    {
                        email = user.NormalizedEmail,
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
            }

            return Unauthorized();
        }

        [Authorize]
        [HttpPost]
        [Route("refresh")]
        [SwaggerOperation(
            Summary = "Refresh the JWT token.",
            Description = "Generates a new JWT token for an authenticated user."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Token refreshed successfully.", typeof(object))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated.")]
        public async Task<IActionResult> Refresh()
        {
            await Task.Delay(0);
            if (User?.Identity == null)
                return Unauthorized();

            if (!User.Identity.IsAuthenticated)
                return Unauthorized();
            var token = GetToken(User.Claims.ToList());

            return Ok(new
            {
                email = User.NormalizedEmail(),
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Secret));

            var token = new JwtSecurityToken(
                issuer: _jwtConfiguration.ValidIssuer,
                audience: _jwtConfiguration.ValidAudience,
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
    }
}

