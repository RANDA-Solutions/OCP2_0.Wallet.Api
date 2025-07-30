using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using OpenCredentialPublisher.Wallet.Models.Credentials;
using OpenCredentialPublisher.Wallet.Models.Shared;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Data.Custom.Commands;
using OpenCredentialPublisher.Wallet.Models.Revocation;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Custom.Options;
using OpenCredentialPublisher.Shared.Custom.Models;
using OpenCredentialPublisher.Shared.Custom.Models.Enums;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    public class CredentialsController : SecureApiControllerBase<CredentialsController>
    {
        private readonly CredentialService _credentialService;
        private readonly RevocationService _revocationService;
        private readonly SiteSettingsOptions _siteSettings;


        public CredentialsController(UserManager<ApplicationUser> userManager,
            ILogger<CredentialsController> logger,
            CredentialService credentialService,
            RevocationService revocationService,
            IOptions<SiteSettingsOptions> siteSettings) : base(userManager, logger)
        {
            _credentialService = credentialService;
            _revocationService = revocationService;
            _siteSettings = siteSettings.Value;
        }


        [HttpPost]
        [ProducesResponseType(200, Type = typeof(List<CredentialCardResponseModel>))]  /* success returns 200 - Ok */
        public async Task<IActionResult> SearchAsync(CredentialsSearchRequestModel model)
        {
            try
            {
                var searchCredentials = await _credentialService.SearchAsync(_userId,
                    model.Keywords,
                    model.IssuerName,
                    model.AchievementType,
                model.EffectiveAtYear);

                var verifiableCredentialIds = searchCredentials.Select(vc => vc.VerifiableCredentialId)
                    .Distinct()
                    .ToImmutableList();

                var issuerNames = searchCredentials.Select(vc => vc.IssuerName)
                    .OrderBy(pn => pn)
                    .Distinct()
                .ToImmutableList();

                var achievementTypes = searchCredentials.Select(vc => vc.AchievementType)
                    .OrderBy(ct => ct)
                    .Distinct()
                .ToImmutableList();

                var effectiveAtYears = searchCredentials.Select(vc => vc.EffectiveAtYear)
                    .OrderBy(iy => iy)
                    .Distinct()
                    .ToImmutableList();

                var credentialsSearchResponseViewModel = new CredentialsSearchResponseModel(
                    verifiableCredentialIds,
                    issuerNames,
                    achievementTypes,
                    effectiveAtYears
                );

                return ApiOk(credentialsSearchResponseViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CredentialsController.SearchAsync");
                throw;
            }
        }

        [HttpGet("{id}/card")]
        [ProducesResponseType(200, Type = typeof(CredentialCardResponseModel))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetCardAsync(long id)
        {
            try
            {
                var verifiableCredential = await _credentialService.GetAsync(_userId, id);

                if (verifiableCredential == null)
                {
                    throw new ApiModelNotFoundException("The specified credential was not found.");
                }

                var credentialCardResponseViewModel = CredentialCardResponseModel.FromModel(_userId, verifiableCredential);

                return ApiOk(credentialCardResponseViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CredentialsController.GetCardAsync verifiableCredentialId: {0}", id);
                throw;
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]  /* success returns 204 - No Content */
        public async Task<IActionResult> DeleteAsync(long id)
        {
            try
            {
                await _credentialService.DeleteAsync(_userId, id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CredentialsController.DeleteAsync");

                return StatusCode(500, "An error occurred attempting to remove the selected collection.");
            }
        }

        [HttpGet("report")]
        [ProducesResponseType(200, Type = typeof(CredentialsReportDto))]
        [Produces("application/json", new string[] { "text/csv" })]
        public async Task<IActionResult> GetReportAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var authorized = await _userManager.IsInRoleAsync(user, RoleEnum.SystemAdmin);
            if (!authorized)
            {
                return Forbid();
            }

            try
            {
                var report = await _credentialService.GetCredentialsReportAsync();

                if (Request.Headers.Accept.Any(header => header.Contains("text/csv", StringComparison.OrdinalIgnoreCase)))
                {
                    var csv = report.ToCsv();
                    return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", $"CredentialsReport_{DateTime.UtcNow:yyyyMMddHHmmss}.csv");
                }

                return ApiOk(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CredentialsController.GetReportAsync");
                throw;
            }
        }
    }
}
