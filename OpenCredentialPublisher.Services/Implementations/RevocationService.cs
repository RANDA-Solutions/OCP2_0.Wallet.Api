using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenCredentialPublisher.Data.Custom.Contexts;
using OpenCredentialPublisher.Data.Custom.CredentialModels;
using OpenCredentialPublisher.Data.Custom.EFModels;
using OpenCredentialPublisher.Data.Custom.Results;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class RevocationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly WalletDbContext _context;
        private readonly ILogger<RevocationService> _logger;

        public RevocationService(
            IHttpClientFactory httpClientFactory,
            WalletDbContext context, 
            ILogger<RevocationService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
            _logger = logger;
        }

        public async Task<RevocationResult> CheckAndRevokeCredentialAsync( long verifiableCredentialId)
        {
            try
            {
                var vc = await _context.VerifiableCredentials2
                    .Include(x => x.ParentVerifiableCredential)
                    .FirstOrDefaultAsync(x =>
                        x.VerifiableCredentialId == verifiableCredentialId);

                if (vc == null)
                {
                    return null;
                }

                if (vc.IsRevoked)
                {
                    return new RevocationResult { IsRevoked = vc.IsRevoked, RevokedReason = vc.RevokedReason };
                }


                //first check the parent.
                if (vc.ParentVerifiableCredential != null)
                {
                    if (vc.ParentVerifiableCredential.IsRevoked)
                    {
                        return new RevocationResult { IsRevoked = vc.IsRevoked, RevokedReason = vc.RevokedReason };
                    }

                    var result = await GetRevocationResultAsync(vc.ParentVerifiableCredential.Json, vc.ParentVerifiableCredential.Id);

                    await RevokeCredentialAsync(vc.ParentVerifiableCredential, result);
                    return result;
                }

                //now check the individual
                var vcResult = await GetRevocationResultAsync(vc.Json, vc.Id);
                await RevokeCredentialAsync(vc, vcResult);

                return vcResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"CheckAndRevokeCredentialAsync for vc id {verifiableCredentialId}");
            }

            return null;

        }

        private async Task RevokeCredentialAsync(VerifiableCredential vc, RevocationResult revocationResult)
        {
            if (!revocationResult.IsRevoked)
                return;

            vc.IsRevoked = revocationResult.IsRevoked;
            vc.RevokedReason = revocationResult.RevokedReason;

            //if this vc has children then all the children should be revoked as well. 
            var children = await _context
                .VerifiableCredentials2
                .Where(c => c.ParentVerifiableCredentialId == vc.VerifiableCredentialId).ToListAsync();

            foreach (var child in children)
            {
                child.IsRevoked = revocationResult.IsRevoked;
                child.RevokedReason = revocationResult.RevokedReason;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<RevocationResult> GetRevocationResultAsync(string json, string verifiableCredentialModelId)
        {
            try
            {
                var credentialStatus = FindOutermostCredentialStatus(json);
                if (credentialStatus == null)
                {
                    return new RevocationResult { IsRevoked = false, RevokedReason = null};
                }

                using var client = _httpClientFactory.CreateClient();

                var request = new HttpRequestMessage(HttpMethod.Get, credentialStatus.Id);

                using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                var summaryResult =  System.Text.Json.JsonSerializer.Deserialize<RevocationSummaryResult>(await response.Content.ReadAsStringAsync());
                if (summaryResult is { Revocations: not null })
                {
                    
                    if (summaryResult.Revocations.Any(x => x.Id == verifiableCredentialModelId))
                    {

                        var revocation = summaryResult.Revocations.First(x => x.Id == verifiableCredentialModelId);
                        var reason = summaryResult.Statuses[revocation.Status];
                        return new RevocationResult { IsRevoked = true, RevokedReason = reason };
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetRevocationResultAsync for vc json id {json}");
            }
            return new RevocationResult { IsRevoked = false, RevokedReason = null };
        }


        private BasicPropertiesModel FindOutermostCredentialStatus(string json)
        {
            var root = JObject.Parse(json);

            // Check if the root object itself has "CredentialStatus" in a case-insensitive manner
            foreach (var property in root.Properties())
            {
                if (string.Equals(property.Name, "CredentialStatus", StringComparison.OrdinalIgnoreCase))
                {
                    // Deserialize the JToken to a BasicPropertiesModel object
                    return property.Value.ToObject<BasicPropertiesModel>();
                }
            }
            return null; // No CredentialStatus found at the outermost level
        }


        //only used for de-serialization.
        [NotMapped]
        private class Revocation
        {
            [JsonProperty("id"), JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonProperty("status"), JsonPropertyName("status")]
            public string Status { get; set; }
        }

        private class RevocationSummaryResult
        {
            [JsonProperty("statuses"), JsonPropertyName("statuses")]
            public Dictionary<string, string> Statuses { get; set; }

            [JsonProperty("revocations"), JsonPropertyName("revocations")]
            public List<Revocation> Revocations { get; set; }
        }



    }
}
