using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Custom.Contexts;
using OpenCredentialPublisher.Data.Custom.CredentialModels;
using OpenCredentialPublisher.Data.Custom.EFModels;
using OpenCredentialPublisher.Data.Custom.Results;
using OpenCredentialPublisher.Proof;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Interfaces;
using Achievement = OpenCredentialPublisher.Data.Custom.EFModels.Achievement;
using Profile = OpenCredentialPublisher.Data.Custom.EFModels.Profile;
using ProfileModel = OpenCredentialPublisher.Data.Custom.CredentialModels.ProfileModel;

[assembly: InternalsVisibleTo("OpenCredentialPublisher.Tests")]
namespace OpenCredentialPublisher.Services.Implementations
{
    public class ETLService
    {
        // ReSharper disable once InconsistentNaming
        public const string CREDENTIAL_ALREADY_LOADED = "That credential has already been loaded.";

        private readonly WalletDbContext _context;
        private readonly ISchemaService _schemaService;
        private readonly ILogger<ETLService> _logger
            ;
        private readonly IProofService _proofService;
        private readonly IRevocationService _revocationService2;

        public ETLService(ISchemaService schemaService,
            WalletDbContext context,
            IProofService proofService,
            ILogger<ETLService> logger, 
            IRevocationService revocationService2)
        {
            _schemaService = schemaService;
            _context = context;
            _proofService = proofService;
            _logger = logger;
            _revocationService2 = revocationService2;
        }

        public async Task<CredentialResponse> ProcessJson(ControllerBase controller, string userId, string json,
            Authorization authorization)
        {
            var modelState = controller.ModelState;

            return await ProcessJson(controller.Request, modelState, userId, null, json, authorization);
        }

        public async Task<CredentialResponse> ProcessJson(HttpRequest request, ModelStateDictionary modelState,
            string userId, string fileName, string json, Authorization authorization)
        {
            var credentialResponse = new CredentialResponse();

            var assemblyType = GetTypeFromJson(json);

            SchemaService.SchemaResult schemaResult = _schemaService.Validate(json);
            if (schemaResult.IsValid && assemblyType == typeof(ClrCredentialModel))
            {
                return await ProcessClrCredential(userId, fileName, json, authorization);
            }

            if (schemaResult?.IsValid == false)
            {
                credentialResponse.ErrorMessages = schemaResult.ErrorMessages;
            }

            if (!modelState.IsValid)
            {
                credentialResponse.ErrorMessages = GetModelErrors(modelState);
            }

            return credentialResponse;
        }

        private Type GetTypeFromJson(string json)
        {
            var jsonDocument = JsonDocument.Parse(json);
            if (jsonDocument.RootElement.TryGetProperty("type", out var typeProperty))
            {
                var jsonTypes = typeProperty.EnumerateArray().Select(t => t.GetString()).ToArray();

                return _schemaService.GetTypeForJson(jsonTypes);
            }

            return null;
        }

        private List<string> GetModelErrors(ModelStateDictionary modelState)
        {
            var modelErrors = new List<string>();
            foreach (var ms in modelState.Values)
            {
                modelErrors.AddRange(ms.Errors.Select(modelError => modelError.ErrorMessage));
            }

            return modelErrors;
        }

        public async Task<CredentialResponse> ProcessClrCredential(string userId, string fileName, string json,
            Authorization authorization)
        {
            var credentialResponse = new CredentialResponse();
            try
            {
                var clrCredentialModel = DeserializeClrCredentialModel(json, credentialResponse);
                if (clrCredentialModel == null) return credentialResponse;

                if (await IsCredentialAlreadyLoadedAsync(userId, clrCredentialModel.Id, json))
                {
                    credentialResponse.ErrorMessages.Add(CREDENTIAL_ALREADY_LOADED);
                    return credentialResponse;
                }

                var package = await CreateCredentialPackageAsync(clrCredentialModel, authorization, userId, json);

                _context.CredentialPackages2.Add(package);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ETLService.ProcessVerifiableCredential");
                credentialResponse.ErrorMessages.Add(ex.Message);
            }

            return credentialResponse;
        }

        public async Task<(ClrCredentialModel clrCredentialModel, CredentialResponse credentialResponse)>
            GetClrCredentialModelAsync(string json, HttpRequest request)
        {
            await Task.CompletedTask;

            var credentialResponse = new CredentialResponse();

            var assemblyType = GetTypeFromJson(json);
            try
            {
                if (assemblyType == typeof(ClrCredentialModel))
                {
                    var schemaResult = _schemaService.Validate(json);

                    if (schemaResult.IsValid)
                    {
                        var clrCredentialModel = DeserializeClrCredentialModel(json, credentialResponse);
                        return (clrCredentialModel, credentialResponse);
                    }

                    throw new ArgumentException("Schema is not valid");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ETLServiceV2.GetClrCredentialModel");
                throw new ApplicationException("An error occurred while getting the CLR credential model.", ex);
            }

            return (null, credentialResponse);
        }


        private ClrCredentialModel DeserializeClrCredentialModel(string json, CredentialResponse credentialResponse)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    Converters =
                    {
                        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
                        new VerifiableCredentialModelConverter()
                    }
                };

                return JsonSerializer.Deserialize<ClrCredentialModel>(json, options);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ETLServiceV2.DeserializeClrCredentialModel json: {0}", json);
                credentialResponse.ErrorMessages.Add(e.Message);
                return null;
            }
        }

        private async Task<bool> IsCredentialAlreadyLoadedAsync(string userId, string credentialId, string json)
        {
            using var hasher = SHA512.Create();
            var schemaHash = Convert.ToBase64String(hasher.ComputeHash(Encoding.UTF8.GetBytes(json)));
            var id = string.IsNullOrEmpty(credentialId) ? schemaHash : credentialId;

            return await _context.CredentialPackages2
                .Include(cp => cp.VerifiableCredentials)
                .AnyAsync(cp =>
                    cp.UserId == userId && cp.VerifiableCredentials.Any(vc => vc.Id == id) && !cp.IsDeleted);
        }

        private async Task<CredentialPackage> CreateCredentialPackageAsync(ClrCredentialModel clrCredentialModel,
            Authorization authorization, string userId, string json)
        {
            var package = new CredentialPackage
            {
                Id = clrCredentialModel.Id,
                Name = clrCredentialModel.Name,
                UserId = authorization?.UserId ?? userId,
                VerifiableCredentials = await CreateClrVerifiableCredentialsAsync(clrCredentialModel, json)
            };

            return package;
        }

        private async Task<Profile> GetOrCreateProfileAsync(ProfileModel profileModel)
        {
            if (profileModel == null)
                return null;

            // Check if the entity is already tracked by the context
            var trackedProfile = _context.ChangeTracker.Entries<Profile>()
                .FirstOrDefault(e => e.Entity.Id == profileModel.Id)?
                .Entity;

            if (trackedProfile != null)
            {
                return trackedProfile;
            }

            // If the entity is not tracked, fetch it from the database
            var issuerProfile = await _context.Profiles2.FirstOrDefaultAsync(p => p.Id == profileModel.Id);

            if (issuerProfile == null)
            {
                issuerProfile = new Profile
                {
                    Id = profileModel.Id,
                    Name = profileModel.Name,
                    ImageUrl = profileModel.Image?.Id,
                    Url = profileModel.Url
                };

                _context.Profiles2.Add(issuerProfile);
            }

            return issuerProfile;
        }


        internal async Task<List<VerifiableCredential>> CreateClrVerifiableCredentialsAsync(
            ClrCredentialModel clrCredentialModel, string packageJson)
        {
            var clrIssuerProfile = await GetOrCreateProfileAsync(clrCredentialModel.Issuer);

            var verifiableCredentialList = new List<VerifiableCredential>();
            var isClrVerified = await _proofService.VerifyProof(packageJson);
            var clrRevocationResult = await CheckRevocationAsync(packageJson, null, clrCredentialModel.Id);

            var clrVerifiableCredential = new VerifiableCredential
            {
                Id = clrCredentialModel.Id,
                Name = clrCredentialModel.Name,
                Description = clrCredentialModel.Description,
                AwardedDate = clrCredentialModel.AwardedDate?.ToDateTimeOffsetOrDefault(DateTimeOffset.UtcNow),
                ValidFromDate = clrCredentialModel.ValidFrom.ToDateTimeOffsetOrDefault(DateTimeOffset.UtcNow),
                ValidUntilDate = clrCredentialModel.ValidUntil?.ToDateTimeOffsetOrDefault(DateTimeOffset.MaxValue),
                Json = packageJson,
                Type = clrCredentialModel.Type,
                IssuerProfile = clrIssuerProfile,
                IsVerified = isClrVerified,
                ImageUrl = clrCredentialModel.Image?.Id,
                IsRevoked = clrRevocationResult.IsRevoked,
                RevokedReason = clrRevocationResult.RevokedReason
            };

            verifiableCredentialList.Add(clrVerifiableCredential);
            var associationModels = clrCredentialModel.CredentialSubject.Association;
            var associationDictionary = new Dictionary<string, VerifiableCredential>();
            foreach (var model in clrCredentialModel.CredentialSubject.VerifiableCredential)
            {
                var vcIssuerProfile = await GetOrCreateProfileAsync(model.Issuer);

                var isVerified = await _proofService.VerifyProof(model.OriginalJson);
                var revocationResult = await CheckRevocationAsync(model.OriginalJson, clrVerifiableCredential, model.Id);

                var verifiableCredential = new VerifiableCredential
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    ValidFromDate = model.ValidFrom.ToDateTimeOffsetOrDefault(DateTimeOffset.UtcNow),
                    ValidUntilDate = model.ValidUntil?.ToDateTimeOffsetOrDefault(DateTimeOffset.MaxValue),
                    Json = model.OriginalJson,
                    ParentVerifiableCredential = clrVerifiableCredential,
                    Type = model.Type,
                    IssuerProfile = vcIssuerProfile,
                    IsVerified = isVerified,
                    ImageUrl = model.Image?.Id,
                    IsRevoked = revocationResult.IsRevoked,
                    RevokedReason = revocationResult.RevokedReason,
                    IsChild = associationModels != null && associationModels.Any(x => x.TargetId == model.Id)
                };

                associationDictionary[model.Id] = verifiableCredential;

                verifiableCredentialList.Add(verifiableCredential);
                if (model is AchievementCredentialModel achievementCredential)
                {
                    // store awarded date on VC?
                    verifiableCredential.AwardedDate =
                        achievementCredential.AwardedDate?.ToDateTimeOffsetOrDefault(DateTimeOffset.UtcNow);

                    var achievement =
                        await CreateAchievementAsync(achievementCredential.CredentialSubject, verifiableCredential);

                    if (achievementCredential.Evidence != null)
                    {
                        foreach (var evidence in achievementCredential.Evidence)
                        {
                            await CreateEvidenceAsync(verifiableCredential, evidence);
                        }
                    }

                    CreateResults(achievementCredential.CredentialSubject, verifiableCredential);

                    _context.Achievements2.Add(achievement);
                }
            }
            
            var associations = new List<Association>();
            // Create associations
            if (associationModels != null)
            {
                foreach (var associationModel in associationModels)
                {
                    
                    var sourceVerifiableCredential = associationDictionary[associationModel.SourceId];
                    
                    var targetVerifiableCredential = associationDictionary[associationModel.TargetId];
                    
                    if (sourceVerifiableCredential != null && targetVerifiableCredential != null)
                    {
                        var association = new Association
                        {
                            SourceVerifiableCredential = sourceVerifiableCredential,
                            TargetVerifiableCredential = targetVerifiableCredential,
                            AssociationType = associationModel.AssociationType.ToString()
                        };
                        sourceVerifiableCredential.SourceAssociations.Add(association);
                    }
                }
            }

            return verifiableCredentialList;
        }

        private async Task<RevocationResult> CheckRevocationAsync(string json, VerifiableCredential parentCredential, string verifiableCredentialModelId)
        {
            if (parentCredential?.IsRevoked == true)
            {
                return new RevocationResult{IsRevoked = parentCredential.IsRevoked, RevokedReason = parentCredential.RevokedReason};
            }

            return await _revocationService2.GetRevocationResultAsync(json, verifiableCredentialModelId);
        }

        private async Task CreateEvidenceAsync(VerifiableCredential verifiableCredential, EvidenceModel evidenceModel)
        {
            var evidence = new Evidence
            {

                VerifiableCredential = verifiableCredential,
                EvidenceUrl = evidenceModel.Id,
                Type = evidenceModel.Type,
                Name = evidenceModel.Name
            };
            await _context.Evidences2.AddAsync(evidence);
        }


        private async Task<Achievement> CreateAchievementAsync(AchievementSubjectModel achievementSubject,
            VerifiableCredential verifiableCredential)
        {
            var sourceProfile = await GetOrCreateProfileAsync(achievementSubject.Source);
            var creatorProfile = await GetOrCreateProfileAsync(achievementSubject.Achievement.Creator);
            var achievementAlignments = CreateAchievementAlignments(achievementSubject);
            var achievementIdentity = CreateAchievementIdentityAsync(achievementSubject);

            return new Achievement
            {
                Id = achievementSubject.Achievement.Id,
                AchievementType = achievementSubject.Achievement.AchievementType,
                Description = achievementSubject.Achievement.Description,
                HumanCode = achievementSubject.Achievement.HumanCode,
                Name = achievementSubject.Achievement.Name,
                FieldOfStudy = achievementSubject.Achievement.FieldOfStudy,
                LicenseNumber = achievementSubject.LicenseNumber,

                // NOTE; Per spec, AchievementSubject.Image is the user's achievement.
                // If not present, we use the Achievement.Image which is not user-specific.
                ImageUrl = achievementSubject.Image?.Id ?? achievementSubject.Achievement.Image?.Id,

                Type = achievementSubject.Achievement.Type ?? new List<string>(),
                Creator = creatorProfile,
                Source = sourceProfile,
                VerifiableCredential = verifiableCredential, // Establish the relationship
                Alignments = achievementAlignments,
                Identifier = achievementIdentity
            };
        }

        private void CreateResults(AchievementSubjectModel achievementSubject, VerifiableCredential verifiableCredential)
        {
            // result is optional
            if (achievementSubject?.Result != null)
            {
                foreach (var resultModel in achievementSubject.Result)
                {
                    var result = new Result()
                    {
                        Value = resultModel.Value,
                        Status = resultModel.Status,
                        ResultDescriptionName = achievementSubject.Achievement.ResultDescription
                            ?.FirstOrDefault(x => x.Id == resultModel.ResultDescription)?.Name,
                        ResultDescriptionType = achievementSubject.Achievement.ResultDescription
                            ?.FirstOrDefault(x => x.Id == resultModel.ResultDescription)?.ResultType,
                        VerifiableCredential = verifiableCredential
                    };

                    verifiableCredential.Results.Add(result);
                }
            }
        }

        private AchievementIdentity CreateAchievementIdentityAsync(AchievementSubjectModel achievementSubject)
        {
            if (achievementSubject?.Identifier == null || !achievementSubject.Identifier.Any())

                return null;
            // extract email and name from identity collection (value in identityHash)
            var emailAddress = achievementSubject.Identifier.FirstOrDefault(i =>
                i.IdentityType.Equals("emailAddress", StringComparison.OrdinalIgnoreCase))?.IdentityHash;
            var name = achievementSubject.Identifier.FirstOrDefault(i =>
                i.IdentityType.Equals("name", StringComparison.OrdinalIgnoreCase))?.IdentityHash;

            // if neither present then nothing to lookup
            if (string.IsNullOrEmpty(emailAddress) && string.IsNullOrEmpty(name))
                return null;

            var achievementIdentity = new AchievementIdentity
            {
                EmailAddress = emailAddress,
                Name = name
            };

            _context.AchievementIdentities2.Add(achievementIdentity);

            return achievementIdentity;
        }

        private List<AchievementAlignment> CreateAchievementAlignments(AchievementSubjectModel achievementSubjectModel)
        {
            var achievementAlignmentList = new List<AchievementAlignment>();

            if (achievementSubjectModel.Achievement.Alignment == null)
                return achievementAlignmentList;


            foreach (var alignmentModel in achievementSubjectModel.Achievement.Alignment)
            {
                achievementAlignmentList.Add(new AchievementAlignment
                {
                    Type = alignmentModel.Type,
                    TargetCode = alignmentModel.TargetCode,
                    TargetDescription = alignmentModel.TargetDescription,
                    TargetName = alignmentModel.TargetName,
                    TargetFramework = alignmentModel.TargetFramework,
                    TargetType = alignmentModel.TargetType,
                    TargetUrl = alignmentModel.TargetUrl
                });
            }

            return achievementAlignmentList;
        }

        public string GetEmail(ClrCredentialModel clrCredentialModel)
        {
            if (clrCredentialModel is null)
                return null;


            var email = clrCredentialModel.CredentialSubject?
                .Identifier?.Where(sub =>
                    sub.IdentityType.Equals("emailaddress", StringComparison.InvariantCultureIgnoreCase))
                .Select(e => e.IdentityHash).FirstOrDefault();


            if (email != null)
                return email;

            if (clrCredentialModel.CredentialSubject != null)
            {
                foreach (var model in clrCredentialModel.CredentialSubject.VerifiableCredential)
                {
                    if (model is AchievementCredentialModel achievementCredential)
                    {
                        var achievementSubject = achievementCredential.CredentialSubject;
                        email = achievementSubject
                            .Identifier?
                            .Where(sub =>
                                sub.IdentityType.Equals("emailaddress", StringComparison.InvariantCultureIgnoreCase))
                            .Select(e => e.IdentityHash).FirstOrDefault();

                        if (email != null)
                            return email;

                        //try based on actual value. 
                        email = achievementSubject
                            .Identifier?
                            .Where(sub =>
                                sub.IdentityType.Equals("identifier", StringComparison.InvariantCultureIgnoreCase))
                            .Select(e => e.IdentityHash).FirstOrDefault();

                        if (MailAddress.TryCreate(email, out var emailAddress))
                        {
                            return emailAddress.ToString();
                        }
                    }
                }
            }

            return null;
    }
    }
}