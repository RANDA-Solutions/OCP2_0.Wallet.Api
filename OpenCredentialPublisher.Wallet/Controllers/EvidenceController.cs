using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using OpenCredentialPublisher.Shared.Extensions;
using OpenCredentialPublisher.Wallet.Models.Evidence;
using OpenCredentialPublisher.Services.Implementations;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    public class EvidenceController : SecureApiControllerBase<EvidenceController>
    {
        private readonly EvidenceService _evidenceService;

        public EvidenceController(UserManager<ApplicationUser> userManager,
            ILogger<EvidenceController> logger, 
            EvidenceService credentialService) : base(userManager,logger)
        {
            _evidenceService = credentialService;
        }


        
        [HttpGet("{verifiableCredentialId}")]
        [ProducesResponseType(200, Type = typeof(EvidenceResponseModel))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetEvidenceAsyncByVerifiableCredentialId(long verifiableCredentialId)
        {
            try
            {
                var evidences = await _evidenceService.GetByVerifiableCredentialId(_userId, verifiableCredentialId);

                var evidencesResponseViewModel = evidences.ToList().Select( EvidenceResponseModel.FromModel).ToImmutableList();

                return ApiOk(evidencesResponseViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EvidenceController : GetEvidenceAsyncByVerifiableCredentialId verifiableCredentialId:{0}", verifiableCredentialId);
                throw;
            }
        }


    }
}
