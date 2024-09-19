using System;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.ClrWallet.Utilities;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Shared.Extensions;
using OpenCredentialPublisher.Wallet.Models.Packages;
using OpenCredentialPublisher.Wallet.Models.Shared;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    public class PackagesController : SecureApiControllerBase<PackagesController>
    {
        private readonly CredentialPackageService _packageService;
        private readonly ETLService _etlService;
        private readonly NotificationService _notificationService;

        public PackagesController(UserManager<ApplicationUser> userManager,
            CredentialPackageService packageService,
            ETLService etlService,
            IHttpContextAccessor httpContextAccessor,
            ILogger<PackagesController> logger, NotificationService notificationService) : base(userManager,
            logger)
        {
            _packageService = packageService;
            _etlService = etlService;
            _notificationService = notificationService;
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(PackagesSearchResponseModel))]  /* success returns 200 - Ok */
        public async Task<IActionResult> SearchAsync(PackagesSearchRequestModel model)
        {
            try
            {
                var searchCredentialPackages = await _packageService.SearchAsync(_userId,
                    model.Keywords,
                    model.IssuerName,
                    model.AchievementType,
                    model.EffectiveAtYear);

                var issuerNames = searchCredentialPackages.SelectMany(scp => scp.Issuers)
                    .Select(i => i.IssuerName)
                    .OrderBy(pn => pn)
                    .Distinct()
                    .ToImmutableList();

                var achievementTypes = searchCredentialPackages.SelectMany(scp => scp.AchievementTypes)
                    .Select(ct => ct.AchievementType)
                    .OrderBy(ct => ct)
                    .Distinct()
                    .ToImmutableList();

                var effectiveAtYears = searchCredentialPackages.SelectMany(scp => scp.Issuers)
                    .Select(i => i.EffectiveAtYear)
                    .OrderBy(iy => iy)
                    .Distinct()
                    .ToImmutableList();

                var packagesSearchResponseViewModel = new PackagesSearchResponseModel(
                    searchCredentialPackages.Select(scp => PackageSearchResponseModel.FromModel(_userId, scp)).ToImmutableList(),
                    issuerNames,
                    achievementTypes,
                    effectiveAtYears);

                return ApiOk(packagesSearchResponseViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PackagesController.SearchPackages");
                throw;
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(PackageDetailsResponseModel))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetAsync(long id)
        {
            try
            {
                var credentialPackage = await _packageService.GetAsync(_userId, id);

                if (credentialPackage == null)
                {
                    throw new ApiModelNotFoundException("The specified credential package was not found.");
                }

                var packageDetailsResponseViewModel = PackageDetailsResponseModel.FromModel(credentialPackage, _userId);

                return ApiOk(packageDetailsResponseViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PackagesController.GetPackage");
                throw;
            }
        }

        /// <summary>
        /// Creates a new credential based on the payload
        /// PUT api/packages
        /// </summary>
        /// <returns>the credential</returns>
        [HttpPut, DisableRequestSizeLimit]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> AddAsync([FromForm] PackageAddRequestModel model)
        {
            var fileName = string.Empty;
            try
            {
                if (model?.PackageFile is { Length: > 0 })
                {
                    if (ContentDispositionHeaderValue.TryParse(model.PackageFile?.ContentDisposition,
                            out var fileNameHeaderValue))
                    {
                        if (!string.IsNullOrEmpty(fileNameHeaderValue.FileName))
                        {
                            fileName = fileNameHeaderValue.FileName.Trim('"');
                        }
                    }
                    var clrJson = await FileHelpers.ProcessFormFile(nameof(PackageAddRequestModel.PackageFile), model.PackageFile, ModelState);
                    if (!ModelState.IsValid)
                    {
                        return ApiOkModelInvalid(ModelState);
                    }

                    //start comment below out if you want to have it create a notification
                    /********************/
                    /*
                    var result = await _etlServiceV2.ProcessJson(_httpContextAccessor.HttpContext.Request, ModelState, _userId, fileName, clrJson, null);

                    if (result.HasError)
                    {
                        foreach (var err in result.ErrorMessages)
                        {
                            ModelState.AddModelError("ClrUpload", err);
                        }
                    }

                    if (!ModelState.IsValid) return ApiOkModelInvalid(ModelState);
                    return ApiOk(result.Id);
                    //stop comment out above if you want to have it create a notification
                    */
                    /********************/

                    /********************/
                    //start uncomment below if you want to create a notification
                    //start uncomment below if you want to create a notification
                    var credentialResponse = await _notificationService.AddNotificationAsync(_userId, clrJson, HttpContext.Request);
                    if (credentialResponse.HasError)
                    {
                        foreach (var err in credentialResponse.ErrorMessages)
                        {
                            ModelState.AddModelError(nameof(PackageAddRequestModel.PackageFile), err);
                        }
                    }

                    if (!ModelState.IsValid)
                    {
                        return ApiOkModelInvalid(ModelState);
                    }

                    return ApiOk(0);
                    //stop uncomment above if you want to create a notification
                    /********************/
                }

                ModelState.AddModelError(nameof(PackageAddRequestModel.PackageFile), "Please select a file to upload.");
                return ApiOkModelInvalid(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem uploading file {0}", fileName);

                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// SoftDelete's a single CredentialPackage
        /// Post api/credentials/Package/Delete/id
        /// </summary>
        /// <returns>Single PackageVM</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  // success returns 200 - Ok 
        public async Task<IActionResult> DeletePackage(long id)
        {
            try
            {
                await _packageService.DeleteAsync(_userId, id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PackagesController.DeletePackage");
                
               return StatusCode(500, "An error occurred attempting to remove the selected package.");
            }
        }
    }
}