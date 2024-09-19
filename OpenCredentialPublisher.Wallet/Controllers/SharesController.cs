using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using OpenCredentialPublisher.Wallet.Models.Shares;
using OpenCredentialPublisher.Services.Implementations;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    public class SharesController : SecureApiControllerBase<CollectionsController>
    {
        private readonly ShareService _shareService;
        private readonly CredentialCollectionService _collectionService;

        public SharesController(UserManager<ApplicationUser> userManager, ILogger<CollectionsController> logger, ShareService shareService, CredentialCollectionService collectionService) : base(userManager, logger)
        {
            _shareService = shareService;
            _collectionService = collectionService;
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(List<ShareListResponseModel>))]  /* success returns 200 - Ok */
        public async Task<IActionResult> ListAsync()
        {
            try
            {
                var shares = await _shareService.GetAllAsync(_userId);

                var shareResponseViewModels = shares.Select(ShareListResponseModel.FromModel);

                return ApiOk(shareResponseViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SharesController.ListAsync");
                throw;
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(List<ShareDetailsResponseModel>))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetAsync(long id)
        {
            try
            {
                var share = await _shareService.GetAsync(_userId, id);

                var shareDetailsResponseViewModel = ShareDetailsResponseModel.FromModel(_userId, share);

                return ApiOk(shareDetailsResponseViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SharesController.GetAsync");
                throw;
            }
        }

        [HttpPut]
        [ProducesResponseType(200, Type = typeof(long))]  /* success returns 200 - Ok */
        public async Task<IActionResult> AddAsync(ShareAddRequestModel model)
        {
            var share = await _shareService.AddAsync(_userId,model.ToCommand());

            return ApiOk(share.ShareId);
        }

        [HttpPost("collections/credentials")]
        [ProducesResponseType(200, Type = typeof(List<long>))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetCredentialIdsByCollectionAsync(List<long> collectionIds)
        {
            var collections = await _collectionService.GetAsync(_userId, collectionIds);

            var credentialIds = collections
                .SelectMany(c => c.CredentialCollectionVerifiableCredentials)
                .Select(cc => cc.VerifiableCredentialId)
                .Distinct()
                .ToList();

            return ApiOk(credentialIds);
        }
    }
}
