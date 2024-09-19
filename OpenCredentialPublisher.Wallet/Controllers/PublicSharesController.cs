using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Models;
using System.Threading.Tasks;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using OpenCredentialPublisher.Wallet.Models.Evidence;
using OpenCredentialPublisher.Wallet.Models.PublicShares;
using OpenCredentialPublisher.Wallet.Models.Shared;
using OpenCredentialPublisher.Services.Implementations;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    public class PublicSharesController : ApiControllerBase<PublicSharesController>

    {
        private readonly ShareService _shareService;
        private readonly CredentialService _credentialService;
        private readonly EvidenceService _evidenceService;

        public PublicSharesController(
            ILogger<PublicSharesController> logger, 
            ShareService shareService, 
            CredentialService credentialService, 
            EvidenceService evidenceService) :  base(logger)
        {
            _shareService = shareService;
            _credentialService = credentialService;
            _evidenceService = evidenceService;
        }


        [HttpGet("{shareId}")]
        [ProducesResponseType(200, Type = typeof(PublicShareResponseModel))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetPublicShareResponseAsync(long shareId, string hash)
        {
            try
            { 
                var share = await _shareService.GetPublicShareAsync(shareId, hash);
                if (share == null)
                {
                    throw new ApiModelNotFoundException("The specified share was not found.");
                }

                var shareDetailsResponseViewModel = PublicShareResponseModel.FromModel(share, hash);

                return ApiOk(shareDetailsResponseViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ShareController.GetPublicShareResponseAsync shareId: {shareId}, hash: {hash}");
                throw;
            }
        }

        [HttpPost("{shareId}")]
        [ProducesResponseType(200, Type = typeof(PublicShareDetailResponseModel))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetPublicShareDetailResponseAsync(long shareId, [FromBody] PublicShareRequestModel request)
        {
            try
            {
                var publicShareDetailResult = await _shareService.GetPublicShareDetailResultAsync(shareId, request.Hash, request.Code);
                if (publicShareDetailResult == null)
                {
                    throw new ApiModelNotFoundException("The specified share was not found.");
                }

                var shareDetailsResponseViewModel = PublicShareDetailResponseModel.FromResultsModel(publicShareDetailResult, request.Hash, request.Code);

                return ApiOk(shareDetailsResponseViewModel);
            }
            catch (InvalidDataException)
            {
                _logger.LogError($"Invalid shareId: {shareId}, hash: {request.Hash}, code: {request.Code}, unable to find it.");

                ModelState.AddModelError("", "That share no longer exists, or the code is incorrect, please try again.");

                return ApiOkModelInvalid(ModelState);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ShareController.GetPublicShareDetailResponseAsync shareId: {shareId}, hash: {request.Hash}, code: {request.Code}");
                throw;
            }
        }

        [HttpPost("{shareId}/credentials/{verifiableCredentialId}")]
        [ProducesResponseType(200, Type = typeof(CredentialCardResponseModel))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetVerifiableCredentialAsync(long verifiableCredentialId, long shareId, [FromBody] PublicShareRequestModel request)
        {
            try
            {
                var  publicShareValidateResult = await _shareService.GetPublicShareValidateResult(shareId, request.Hash, request.Code, verifiableCredentialId);

                var verifiableCredential = await _credentialService.GetAsync(publicShareValidateResult.ShareFromUserId, verifiableCredentialId);

                if (verifiableCredential == null)
                {
                    throw new ApiModelNotFoundException("The specified credential was not found.");
                }

                var credentialCardResponseViewModel = CredentialCardResponseModel.FromModel(publicShareValidateResult.ShareFromUserId, verifiableCredential);

                return ApiOk(credentialCardResponseViewModel);
            }
            catch (InvalidDataException)
            {
                _logger.LogError($"Invalid shareId: {shareId}, hash: {request.Hash}, code: {request.Code} and verifiableCredentialId: {verifiableCredentialId}, unable to find it.");

                ModelState.AddModelError("", "That share no longer exists, or the code is incorrect, please try again");

                return ApiOkModelInvalid(ModelState);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ShareController.GetVerifiableCredentialAsync shareId: {shareId}, hash: {request.Hash}, code: {request.Code}, verifiableCredentialId: {verifiableCredentialId}");
                throw;
            }
        }

        [HttpPost("{shareId}/credentials/{verifiableCredentialId}/evidence")]
        [ProducesResponseType(200, Type = typeof(EvidenceResponseModel))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetEvidenceAsyncByVerifiableCredentialId(long shareId, long verifiableCredentialId, [FromBody] PublicShareRequestModel request)
        {
            try
            {
                var publicShareValidateResult = await _shareService.GetPublicShareValidateResult(shareId, request.Hash, request.Code, verifiableCredentialId);

                var evidences = await _evidenceService.GetByVerifiableCredentialId(publicShareValidateResult.ShareFromUserId, verifiableCredentialId);

                var evidencesResponseViewModel = evidences.ToList().Select(EvidenceResponseModel.FromModel).ToImmutableList();

                return ApiOk(evidencesResponseViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ShareController.GetEvidenceAsyncByVerifiableCredentialId  verifiableCredentialId: {0}", verifiableCredentialId);
                throw;
            }
        }

    }
}
