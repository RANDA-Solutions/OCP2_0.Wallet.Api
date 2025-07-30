using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Custom.Results;
using OpenCredentialPublisher.Services.Implementations;
using System.Threading.Tasks;
using OpenCredentialPublisher.Data.Custom.Contexts;
using OpenCredentialPublisher.Proof;
using OpenCredentialPublisher.Data.Custom.EFModels;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
using OpenCredentialPublisher.Data.Custom.CredentialModels;
using OpenCredentialPublisher.Data.Contexts;
using System.Linq;
using OpenCredentialPublisher.Services.Interfaces;

namespace OpenCredentialPublisher.Tests
{
    public class ETLServiceTests
    {
        private readonly WalletDbContext _context;
        private readonly Mock<ISchemaService> _mockSchemaService;
        private readonly Mock<IProofService> _mockProofService;
        private readonly Mock<IRevocationService> _mockRevocationService;
        private readonly Mock<ILogger<ETLService>> _mockLogger;
        private readonly ETLService _etlService;

        public ETLServiceTests()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<WalletDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new WalletDbContext(options);
            _context.Database.EnsureCreated();
            _mockSchemaService = new Mock<ISchemaService>();
            _mockProofService = new Mock<IProofService>();
            _mockRevocationService = new Mock<IRevocationService>();
            _mockLogger = new Mock<ILogger<ETLService>>();

            _etlService = new ETLService(
                _mockSchemaService.Object,
                _context,
                _mockProofService.Object,
                _mockLogger.Object,
                _mockRevocationService.Object
            );
        }

        [Fact]
        public async Task ProcessJson_InvalidSchema_ReturnsErrorMessages()
        {
            // Arrange
            var modelState = new ModelStateDictionary();
            var json = "{ \"type\": [\"InvalidType\"] }";
            var userId = "test-user";
            var authorization = new Authorization();

            _mockSchemaService
                .Setup(s => s.Validate(json))
                .Returns(new SchemaService.SchemaResult { ErrorMessages = new List<string> { "Invalid schema" } });

            // Act
            var result = await _etlService.ProcessJson(new DefaultHttpContext().Request, modelState, userId, null, json, authorization);

            // Assert
            Assert.NotNull(result);
            Assert.Contains("Invalid schema", result.ErrorMessages);
        }

        [Fact]
        public async Task ProcessClrCredential_DuplicateCredential_ReturnsErrorMessage()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<WalletDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            await using var context = new WalletDbContext(options);

            // Seed the database with a duplicate credential
            var existingCredential = new CredentialPackage
            {
                Id = Guid.NewGuid().ToString(),
                UserId = "test-user",
                CredentialPackageId = 1,
                VerifiableCredentials = new List<VerifiableCredential> { 
                    new VerifiableCredential
                    {
                        Id = "credential-1",
                        Type = new List<string> { "VerifiableCredential" },
                        Json = "{ \"id\": \"credential-1\", \"name\": \"Test Credential\" }",
                    }
                }
            };
            context.CredentialPackages2.Add(existingCredential);
            await context.SaveChangesAsync();

            var etlService = new ETLService(
                _mockSchemaService.Object,
                context,
                _mockProofService.Object,
                _mockLogger.Object,
                _mockRevocationService.Object
            );

            // Arrange
            var userId = "test-user";
            var json = "{ \"id\": \"credential-1\", \"name\": \"Test Credential\" }";
            var authorization = new Authorization();

            // Act
            var result = await etlService.ProcessClrCredential(userId, "test-file.json", json, authorization);

            // Assert
            Assert.NotNull(result);
            Assert.Contains(ETLService.CREDENTIAL_ALREADY_LOADED, result.ErrorMessages);
        }

        [Fact]
        public async Task CreateAssociation_ShouldSaveToDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<WalletDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            await using var context = new WalletDbContext(options);

            var sourceCredential = new VerifiableCredential { VerifiableCredentialId = 1, Type = new List<string> { "VerifiableCredential" }, Id = Guid.NewGuid().ToString(), Json = "{}"};
            var targetCredential = new VerifiableCredential { VerifiableCredentialId = 2, Type = new List<string> { "VerifiableCredential" }, Id = Guid.NewGuid().ToString(), Json = "{}" };

            context.VerifiableCredentials2.AddRange(sourceCredential, targetCredential);
            await context.SaveChangesAsync();

            var association = new Association
            {
                SourceVerifiableCredentialId = sourceCredential.VerifiableCredentialId,
                TargetVerifiableCredentialId = targetCredential.VerifiableCredentialId,
                AssociationType = "ExampleType"
            };

            // Act
            context.Associations2.Add(association);
            await context.SaveChangesAsync();

            // Assert
            var savedAssociation = await context.Associations2
                .Include(a => a.SourceVerifiableCredential)
                .Include(a => a.TargetVerifiableCredential)
                .FirstOrDefaultAsync();

            Assert.NotNull(savedAssociation);
            Assert.Equal("ExampleType", savedAssociation.AssociationType);
            Assert.Equal(sourceCredential.VerifiableCredentialId, savedAssociation.SourceVerifiableCredentialId);
            Assert.Equal(targetCredential.VerifiableCredentialId, savedAssociation.TargetVerifiableCredentialId);
        }

        [Fact]
        public async Task CreateClrVerifiableCredentialsAsync_ValidModel_CreatesVerifiableCredentials()
        {
            // Arrange
            var json = @"{
                ""issuer"": {
                    ""id"": ""did:web:randaocpservice-test.azurewebsites.net:issuers:360beaba-2a51-4733-a410-60cb303a6355"",
                    ""type"": [""Profile"", ""Organization""],
                    ""name"": ""Tennessee Board of Regents"",
                    ""url"": ""https://tbr.edu/board/tennessee-board-regents"",
                    ""phone"": ""615-366-4400"",
                    ""address"": {
                        ""type"": [""Address""],
                        ""addressCountry"": ""USA"",
                        ""addressRegion"": ""TN"",
                        ""addressLocality"": ""Nashville"",
                        ""streetAddress"": ""1 Bridgestone Park, Third Floor"",
                        ""postalCode"": ""37214""
                    },
                    ""otherIdentifier"": [
                        {
                            ""type"": ""IdentifierEntry"",
                            ""identifier"": ""360beaba-2a51-4733-a410-60cb303a6355"",
                            ""identifierType"": ""identifier""
                        }
                    ]
                },
                ""credentialSubject"": {
                    ""type"": [""ClrSubject""],
                    ""identifier"": [
                        {
                            ""type"": ""IdentityObject"",
                            ""hashed"": false,
                            ""identityHash"": ""1fd1726f-81aa-4953-abca-75226a0e6093"",
                            ""identityType"": ""identifier""
                        },
                        {
                            ""type"": ""IdentityObject"",
                            ""hashed"": false,
                            ""identityHash"": ""Example Person"",
                            ""identityType"": ""name""
                        },
                        {
                            ""type"": ""IdentityObject"",
                            ""hashed"": false,
                            ""identityHash"": ""example_user@leveldata.com"",
                            ""identityType"": ""emailAddress""
                        },
                        {
                            ""type"": ""IdentityObject"",
                            ""hashed"": false,
                            ""identityHash"": ""V9999999"",
                            ""identityType"": ""ext:studentId""
                        }
                    ],
                    ""verifiableCredential"": [
                        {
                            ""issuer"": {
                                ""id"": ""did:web:randaocpservice-test.azurewebsites.net:issuers:0fdb7fa1-7070-48f8-9a60-96cb74a27272"",
                                ""type"": [""Profile""],
                                ""name"": ""Volunteer State CC"",
                                ""url"": ""https://volstate.edu/"",
                                ""phone"": ""(615)452"",
                                ""address"": {
                                    ""type"": [""Address""],
                                    ""addressCountry"": ""USA"",
                                    ""addressRegion"": ""TN"",
                                    ""addressLocality"": ""Gallatin"",
                                    ""streetAddress"": ""1480 Nashville Pike"",
                                    ""postalCode"": ""37066""
                                }
                            },
                            ""credentialSubject"": {
                                ""type"": [""AchievementSubject""],
                                ""achievement"": {
                                    ""id"": ""https://credentialfinder.org/resources/88bf7ff2-bd4e-4f28-ac1e-c8aa89204716"",
                                    ""type"": [""Achievement""],
                                    ""achievementType"": ""ext:AssociateDegree"",
                                    ""criteria"": {
                                        ""id"": ""https://credentialfinder.org/resources/88bf7ff2-bd4e-4f28-ac1e-c8aa89204716""
                                    },
                                    ""description"": """",
                                    ""fieldOfStudy"": ""University Parallel"",
                                    ""name"": ""General Studies AA"",
                                    ""specialization"": ""General Studies""
                                },
                                ""identifier"": [
                                    {
                                        ""type"": ""IdentityObject"",
                                        ""hashed"": false,
                                        ""identityHash"": ""1fd1726f-81aa-4953-abca-75226a0e6093"",
                                        ""identityType"": ""identifier""
                                    },
                                    {
                                        ""type"": ""IdentityObject"",
                                        ""hashed"": false,
                                        ""identityHash"": ""Example Person"",
                                        ""identityType"": ""name""
                                    },
                                    {
                                        ""type"": ""IdentityObject"",
                                        ""hashed"": false,
                                        ""identityHash"": ""example_user@leveldata.com"",
                                        ""identityType"": ""emailAddress""
                                    },
                                    {
                                        ""type"": ""IdentityObject"",
                                        ""hashed"": false,
                                        ""identityHash"": ""e0b1f80a-524b-42d2-bbbd-3cdf5df6eaae"",
                                        ""identityType"": ""sourcedId""
                                    }
                                ],
                                ""result"": [
                                    {
                                        ""type"": [""Result""],
                                        ""resultDescription"": """",
                                        ""value"": ""Awarded""
                                    }
                                ]
                            },
                            ""evidence"": [
                                {
                                    ""id"": ""https://credentialfinder.org/resources/88bf7ff2-bd4e-4f28-ac1e-c8aa89204716"",
                                    ""type"": [""Evidence""],
                                    ""name"": ""Learn more about General Studies AA""
                                }
                            ],
                            ""awardedDate"": ""2024-12-12T00:00:00Z"",
                            ""@context"": [
                                ""https://www.w3.org/ns/credentials/v2"",
                                ""https://purl.imsglobal.org/spec/ob/v3p0/context-3.0.3.json"",
                                ""https://w3id.org/security/suites/ed25519-2020/v1"",
                                {
                                    ""givenName"": ""https://schema.org/givenName"",
                                    ""familyName"": ""https://schema.org/familyName"",
                                    ""additionalName"": ""https://schema.org/additionalName"",
                                    ""Organization"": ""https://schema.org/Organization""
                                }
                            ],
                            ""type"": [""VerifiableCredential"", ""AchievementCredential""],
                            ""id"": ""https://credentialfinder.org/resources/88bf7ff2-bd4e-4f28-ac1e-c8aa89204716"",
                            ""name"": ""General Studies AA"",
                            ""description"": """",
                            ""validFrom"": ""2024-12-12T00:00:00Z"",
                            ""proof"": [
                                {
                                    ""type"": ""Ed25519Signature2020"",
                                    ""created"": ""2025-04-28T20:32:47.4778149Z"",
                                    ""proofPurpose"": ""assertionMethod"",
                                    ""verificationMethod"": ""did:web:randaocpservice-test.azurewebsites.net:issuers:0fdb7fa1-7070-48f8-9a60-96cb74a27272#key-70af4162aac44a9a86bd943b931b05e1"",
                                    ""proofValue"": ""z2Z8RHcBQsi1CzpyfHjNzvY7gfSSzxuxnoYJUAY4rwx6Qhz6KevuaQfowLFDsCPhNt5jdYAy9npf7VKaeyS2qHqXV"",
                                    ""nonce"": ""NDRlYTg1ZTYtOGU3NC00ZjhmLWFlN2UtYTNkYzNiZWZiYmRi""
                                }
                            ]
                        }
                    ],
                    ""association"": []
                },
                ""id"": ""credential-1"",
                ""name"": ""Test Credential"",
                ""description"": ""A test credential"",
                ""awardedDate"": ""2023-01-01T00:00:00Z"",
                ""validFrom"": ""2023-01-01T00:00:00Z"",
                ""validUntil"": ""2024-01-01T00:00:00Z"",
                ""type"": [""VerifiableCredential"", ""ClrCredential""],
                ""image"": { ""id"": ""image-1"" }
            }";

            var authorization = new Authorization { UserId = "test-user" };

            // Mock ISchemaService.Validate to return valid
            _mockSchemaService
                .Setup(s => s.Validate(It.IsAny<string>()))
                .Returns(new SchemaService.SchemaResult { ErrorMessages = new List<string>() });

            // Mock ISchemaService.GetTypeForJson to return ClrCredentialModel type
            _mockSchemaService
                .Setup(s => s.GetTypeForJson(It.IsAny<string[]>()))
                .Returns(typeof(ClrCredentialModel));

            // Mock IProofService.VerifyProof to return true
            _mockProofService
                .Setup(p => p.VerifyProof(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Mock IRevocationService.GetRevocationResultAsync to return not revoked
            _mockRevocationService
                .Setup(r => r.GetRevocationResultAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new RevocationResult { IsRevoked = false });

            // Mock WalletDbContext to include Profiles2 if necessary
            var issuerProfile = new Profile
            {
                Id = "issuer-1",
                Name = "Test Issuer",
                Url = "https://issuer.example.com"
            };
            _context.Profiles2.Add(issuerProfile);
            await _context.SaveChangesAsync();

            // Act
            var credentialResponse = await _etlService.ProcessClrCredential("test-user", null, json, authorization);

            // Assert
            Assert.NotNull(credentialResponse);
            Assert.Empty(credentialResponse.ErrorMessages);

            var credentialPackage = await _context.CredentialPackages2
                .Include(cp => cp.VerifiableCredentials)
                .ThenInclude(vc => vc.ParentVerifiableCredential)
                .Include(cp => cp.VerifiableCredentials)
                .ThenInclude(vc => vc.IssuerProfile)
                .FirstOrDefaultAsync(cp => cp.Id == "credential-1");

            Assert.NotNull(credentialPackage);
            Assert.Equal("Test Credential", credentialPackage.Name);
            Assert.Equal("test-user", credentialPackage.UserId);
            Assert.Equal(2, credentialPackage.VerifiableCredentials.Count);

            var verifiableCredential = credentialPackage.VerifiableCredentials.First();
            Assert.Equal("credential-1", verifiableCredential.Id);
            Assert.Equal("Test Credential", verifiableCredential.Name);
            Assert.Equal("A test credential", verifiableCredential.Description);
            Assert.True(verifiableCredential.IsVerified);
            Assert.False(verifiableCredential.IsRevoked);
            Assert.Equal("image-1", verifiableCredential.ImageUrl);
            Assert.Equal("https://tbr.edu/board/tennessee-board-regents", verifiableCredential.IssuerProfile.Url);
        }
    }
}
