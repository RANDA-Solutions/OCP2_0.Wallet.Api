using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Models;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenCredentialPublisher.Wallet.Models.Collections;
using OpenCredentialPublisher.Wallet.Models.Credentials;
using OpenCredentialPublisher.Wallet.Models.Shared;
using OpenCredentialPublisher.Services.Implementations;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    public class CollectionsController : SecureApiControllerBase<CollectionsController>
    {
        private readonly CredentialCollectionService _credentialCollectionService;

        public CollectionsController(UserManager<ApplicationUser> userManager, ILogger<CollectionsController> logger, CredentialCollectionService credentialCollectionService) : base(userManager, logger)
        {
            _credentialCollectionService = credentialCollectionService;
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(List<long>))]  /* success returns 200 - Ok */
        public async Task<IActionResult> SearchAsync(CredentialCollectionsSearchRequestModel model)
        {
            try
            {
                var searchCredentialCollections = await _credentialCollectionService.SearchAsync(_userId,
                    model.Keywords,
                    model.SortBy);

                return ApiOk(searchCredentialCollections.Select(scc => scc.CredentialCollectionId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CollectionsController.SearchAsync");
                throw;
            }
        }

        [HttpGet("{id}/card")]
        [ProducesResponseType(200,
            Type = typeof(CredentialCollectionCardResponseModel))] /* success returns 200 - Ok */
        public async Task<IActionResult> GetCardAsync(long id)
        {
            try
            {
                var credentialCollection = await _credentialCollectionService.GetAsync(_userId, id);

                return ApiOk(CredentialCollectionCardResponseModel.FromModel(_userId, credentialCollection));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CollectionsController.GetCardAsync");
                throw;
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200,
            Type = typeof(CredentialCollectionAddEditRequestResponseModel))] /* success returns 200 - Ok */
        public async Task<IActionResult> GetAsync(long id)
        {
            try
            {
                var credentialCollection = await _credentialCollectionService.GetAsync(_userId, id);

                if (credentialCollection == null)
                {
                    throw new ApiModelNotFoundException("The specified collection was not found.");
                }

                return ApiOk(CredentialCollectionAddEditRequestResponseModel.FromModel(_userId, credentialCollection));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CollectionsController.GetAsync");
                throw;
            }
        }

        [HttpPut]
        [ProducesResponseType(200,
            Type = typeof(long))] /* success returns 200 - Ok */
        public async Task<IActionResult> AddAsync(CredentialCollectionAddEditRequestResponseModel model)
        {
            if (!ModelState.IsValid) return ApiOkModelInvalid(ModelState);

            try
            {
                var collection = await _credentialCollectionService.AddAsync(_userId, model.ToAddCommand());

                return ApiOk(collection.CredentialCollectionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CollectionsController.AddAsync");
                throw;
            }
        }

        [HttpPost("{id}")]
        [ProducesResponseType(200,
            Type = typeof(long))] /* success returns 200 - Ok */
        public async Task<IActionResult> SaveAsync(long id, CredentialCollectionAddEditRequestResponseModel model)
        {
            if (!ModelState.IsValid) return ApiOkModelInvalid(ModelState);

            try
            {
                var collection = await _credentialCollectionService.SaveAsync(_userId, model.ToSaveCommand());

                return ApiOk(collection.CredentialCollectionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CollectionsController.SaveAsync");
                throw;
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204, Type = typeof(ApiResponse))]  /* success returns 204 - No Content */
        public async Task<IActionResult> DeleteAsync(long id)
        {
            try
            {
                await _credentialCollectionService.DeleteAsync(_userId, id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CollectionsController.DeleteAsync");

                return StatusCode(500, "An error occurred attempting to remove the selected collection.");
            }
        }
    }
}
