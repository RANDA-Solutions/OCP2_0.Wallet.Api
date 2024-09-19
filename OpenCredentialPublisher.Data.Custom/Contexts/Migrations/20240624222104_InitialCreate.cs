using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OpenCredentialPublisher.Data.Custom.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cred");

            migrationBuilder.EnsureSchema(
                name: "idatafy");

            migrationBuilder.CreateTable(
                name: "AgentContexts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AgentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApiKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContextJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DomainDid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndpointUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuerDid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuerRegistered = table.Column<bool>(type: "bit", nullable: false),
                    IssuerVerKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Network = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SdkVerKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SdkVerKeyId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TokenHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThreadId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerityAgentVerKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerityPublicDid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerityPublicVerKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerityUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WalletKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WalletPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProvisioningTokenId = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentContexts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Alignments",
                schema: "cred",
                columns: table => new
                {
                    AlignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EducationalFramework = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alignments", x => x.AlignmentId);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationSettings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ProfileImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Associations",
                schema: "cred",
                columns: table => new
                {
                    AssociationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssociationType = table.Column<int>(type: "int", nullable: false),
                    TargetId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Associations", x => x.AssociationId);
                });

            migrationBuilder.CreateTable(
                name: "BadgrImageDType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BadgrImageDType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    Host = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IssuedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuedToName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.Host);
                });

            migrationBuilder.CreateTable(
                name: "CommerceAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressFirstName = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: false),
                    AddressLastName = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: false),
                    AddressLine1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AddressLine2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AddressCity = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AddressState = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    AddressPostalCode = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    AddressCountry = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    AddressEmail = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: true),
                    AddressPhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AdressCompany = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommerceAddresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommerceTransactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionTypeId = table.Column<int>(type: "int", nullable: false),
                    VisibleOnLedger = table.Column<bool>(type: "bit", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDebit = table.Column<bool>(type: "bit", nullable: false),
                    Amount = table.Column<decimal>(type: "money", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UniqueId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommerceTransactions", x => x.TransactionId);
                });

            migrationBuilder.CreateTable(
                name: "ConnectionRequestSteps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionRequestSteps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CredentialRequestSteps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CredentialRequestSteps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CredentialSchema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchemaId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThreadId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NetworkId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attributes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CredentialSchema", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Criteria",
                schema: "cred",
                columns: table => new
                {
                    CriteriaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Narrative = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Criteria", x => x.CriteriaId);
                });

            migrationBuilder.CreateTable(
                name: "DIDs",
                schema: "cred",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    KeyTypeId = table.Column<int>(type: "int", nullable: false),
                    Seed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DIDs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EndorsementClaims",
                schema: "cred",
                columns: table => new
                {
                    EndorsementClaimId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndorsementComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndorsementClaims", x => x.EndorsementClaimId);
                });

            migrationBuilder.CreateTable(
                name: "Evidence",
                schema: "cred",
                columns: table => new
                {
                    EvidenceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Audience = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Genre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Narrative = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evidence", x => x.EvidenceId);
                });

            migrationBuilder.CreateTable(
                name: "GivenNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GivenNames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HttpClientLogs",
                columns: table => new
                {
                    HttpClientLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Machine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestIpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestContentBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestUri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestRouteTemplate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestRouteData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestHeaders = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestTimestamp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResponseContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContentBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseStatusCode = table.Column<int>(type: "int", nullable: true),
                    ResponseHeaders = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseTimestamp = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HttpClientLogs", x => x.HttpClientLogId);
                });

            migrationBuilder.CreateTable(
                name: "Identities",
                schema: "cred",
                columns: table => new
                {
                    IdentityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Identity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hashed = table.Column<bool>(type: "bit", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identities", x => x.IdentityId);
                });

            migrationBuilder.CreateTable(
                name: "IdentityCertificates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DnsName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Certificate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValidUntil = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityCertificates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Keys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KeyTypeId = table.Column<byte>(type: "tinyint", nullable: false),
                    Private = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Public = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    KeyNumber = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsUsable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProofRequestSteps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProofRequestSteps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScoreReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Program = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShareTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientSecret = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Scope = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceTypeId = table.Column<int>(type: "int", nullable: false),
                    IsDeletable = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StreetAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreetAddresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Surnames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surnames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Verifications",
                schema: "cred",
                columns: table => new
                {
                    VerificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    AllowedOrigins = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartsWith = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerificationProperty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verifications", x => x.VerificationId);
                });

            migrationBuilder.CreateTable(
                name: "VerityThreads",
                columns: table => new
                {
                    ThreadId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FlowTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerityThreads", x => x.ThreadId);
                });

            migrationBuilder.CreateTable(
                name: "ProvisioningToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SponseeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SponsorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nonce = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sig = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SponsorVerKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    AgentContextId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProvisioningToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProvisioningToken_AgentContexts_AgentContextId",
                        column: x => x.AgentContextId,
                        principalTable: "AgentContexts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoginProofRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProofId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProofAttributeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProofContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProofPayload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QrCodeUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProofResponse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ProofRequestStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ValidUntil = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginProofRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoginProofRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Recipients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipients_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserAddresses",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AddressLine1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressLine2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Modified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAddresses", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_UserAddresses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserEmails",
                columns: table => new
                {
                    EmailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SendEmail = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Modified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEmails", x => x.EmailId);
                    table.ForeignKey(
                        name: "FK_UserEmails_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserPhoneNumbers",
                columns: table => new
                {
                    PhoneId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Modified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPhoneNumbers", x => x.PhoneId);
                    table.ForeignKey(
                        name: "FK_UserPhoneNumbers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WalletRelationships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WalletName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelationshipDid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelationshipVerKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InviteUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsConnected = table.Column<bool>(type: "bit", nullable: false),
                    AgentContextId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletRelationships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalletRelationships_AgentContexts_AgentContextId",
                        column: x => x.AgentContextId,
                        principalTable: "AgentContexts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WalletRelationships_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CredentialDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentContextId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CredentialSchemaId = table.Column<int>(type: "int", nullable: false),
                    ThreadId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CredentialDefinitionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CredentialDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CredentialDefinitions_AgentContexts_AgentContextId",
                        column: x => x.AgentContextId,
                        principalTable: "AgentContexts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CredentialDefinitions_CredentialSchema_CredentialSchemaId",
                        column: x => x.CredentialSchemaId,
                        principalTable: "CredentialSchema",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProofRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThreadId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Network = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisclosureId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForRelationship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProofAttributes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProofPredicates = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotificationAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CredentialSchemaId = table.Column<int>(type: "int", nullable: false),
                    InvitationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvitationLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortInvitationLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvitationQrCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StepId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProofRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProofRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProofRequests_CredentialSchema_CredentialSchemaId",
                        column: x => x.CredentialSchemaId,
                        principalTable: "CredentialSchema",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ScoreReportsCandidates",
                columns: table => new
                {
                    ScoreReportId = table.Column<int>(type: "int", nullable: false),
                    CandidateId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreReportsCandidates", x => new { x.ScoreReportId, x.CandidateId });
                    table.ForeignKey(
                        name: "FK_ScoreReportsCandidates_ScoreReports_ScoreReportId",
                        column: x => x.ScoreReportId,
                        principalTable: "ScoreReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Authorizations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorizationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeVerifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Endpoint = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Scopes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceForeignKey = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Authorizations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Authorizations_Sources_SourceForeignKey",
                        column: x => x.SourceForeignKey,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscoveryDocumentModel",
                schema: "cred",
                columns: table => new
                {
                    DiscoveryDocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceForeignKey = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApiBase = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TokenRevocationUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Context = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorizationUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrivacyPolicyUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScopesOffered = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TermsOfServiceUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TokenUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscoveryDocumentModel", x => x.DiscoveryDocumentId);
                    table.ForeignKey(
                        name: "FK_DiscoveryDocumentModel_Sources_SourceForeignKey",
                        column: x => x.SourceForeignKey,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Revocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RevocationListId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceId = table.Column<int>(type: "int", nullable: true),
                    IssuerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RevokedId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Revocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Revocations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Revocations_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LastCheckedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ReportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ssn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SsnHashed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SsnLastFour = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zipcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dob = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ethnicity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HighestEducationAttained = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UndergraduateMajor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GraduateMajor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UndergraduateGpa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Aiorg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Candidates_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Candidates_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Folio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Modified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Folio_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Folio_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserPaymentRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    ProjectedCredits = table.Column<int>(type: "int", nullable: false),
                    ActualCredits = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Modified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPaymentRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPaymentRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserPaymentRequests_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                schema: "cred",
                columns: table => new
                {
                    ProfileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsEndorsementProfile = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FamilyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GivenName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Identifiers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Official = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RevocationList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourcedId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentProfileId = table.Column<int>(type: "int", nullable: true),
                    VerificationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.ProfileId);
                    table.ForeignKey(
                        name: "FK_Profiles_Profiles_ParentProfileId",
                        column: x => x.ParentProfileId,
                        principalSchema: "cred",
                        principalTable: "Profiles",
                        principalColumn: "ProfileId");
                    table.ForeignKey(
                        name: "FK_Profiles_Verifications_VerificationId",
                        column: x => x.VerificationId,
                        principalSchema: "cred",
                        principalTable: "Verifications",
                        principalColumn: "VerificationId");
                });

            migrationBuilder.CreateTable(
                name: "ConnectionRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentContextId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ThreadId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConnectionRequestStep = table.Column<int>(type: "int", nullable: false),
                    WalletRelationshipId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConnectionRequests_AgentContexts_AgentContextId",
                        column: x => x.AgentContextId,
                        principalTable: "AgentContexts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConnectionRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConnectionRequests_WalletRelationships_WalletRelationshipId",
                        column: x => x.WalletRelationshipId,
                        principalTable: "WalletRelationships",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProofResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProofRequestId = table.Column<int>(type: "int", nullable: false),
                    ProofResultId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerificationResult = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProofResponseJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SelfAttestedAttributes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RevealedAttributes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Predicates = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnrevealedAttributes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Identifiers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProofResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProofResponses_ProofRequests_ProofRequestId",
                        column: x => x.ProofRequestId,
                        principalTable: "ProofRequests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CredentialPackages",
                schema: "cred",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    AssertionsCount = table.Column<int>(type: "int", nullable: false),
                    Revoked = table.Column<bool>(type: "bit", nullable: false),
                    RevocationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorizationForeignKey = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ETSResultsCandidateId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HasPraxisTest = table.Column<bool>(type: "bit", nullable: false),
                    ScoreReportId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CredentialPackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CredentialPackages_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CredentialPackages_Authorizations_AuthorizationForeignKey",
                        column: x => x.AuthorizationForeignKey,
                        principalTable: "Authorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CredentialPackages_ScoreReports_ScoreReportId",
                        column: x => x.ScoreReportId,
                        principalTable: "ScoreReports",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CandidateVerifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CandidateId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SsnAvailable = table.Column<bool>(type: "bit", nullable: false),
                    VerifiedSsn = table.Column<bool>(type: "bit", nullable: false),
                    VerifiedDob = table.Column<bool>(type: "bit", nullable: false),
                    VerifiedName = table.Column<bool>(type: "bit", nullable: false),
                    VerifiedAddress = table.Column<bool>(type: "bit", nullable: false),
                    Passed = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    VerificationSubmitted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateVerifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CandidateVerifications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CandidateVerifications_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CandidateId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TestDate = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TestCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReportDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Test_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestScore = table.Column<short>(type: "smallint", nullable: true),
                    Roe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rvsd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassingStatusAsOfReportDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiredScoreAsOfReportDate = table.Column<short>(type: "smallint", nullable: true),
                    RequiredMinimumScore = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinimumScoreMetNotMet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScoreReportId = table.Column<int>(type: "int", nullable: true),
                    FolioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tests_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tests_Folio_FolioId",
                        column: x => x.FolioId,
                        principalTable: "Folio",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tests_ScoreReports_ScoreReportId",
                        column: x => x.ScoreReportId,
                        principalTable: "ScoreReports",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserCredits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentRequestId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Modified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCredits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCredits_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserCredits_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserCredits_UserPaymentRequests_PaymentRequestId",
                        column: x => x.PaymentRequestId,
                        principalTable: "UserPaymentRequests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Achievements",
                schema: "cred",
                columns: table => new
                {
                    AchievementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AchievementType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreditsAvailable = table.Column<float>(type: "real", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HumanCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Identifiers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FieldOfStudy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CriteriaId = table.Column<int>(type: "int", nullable: true),
                    IssuerProfileId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievements", x => x.AchievementId);
                    table.ForeignKey(
                        name: "FK_Achievements_Criteria_CriteriaId",
                        column: x => x.CriteriaId,
                        principalSchema: "cred",
                        principalTable: "Criteria",
                        principalColumn: "CriteriaId");
                    table.ForeignKey(
                        name: "FK_Achievements_Profiles_IssuerProfileId",
                        column: x => x.IssuerProfileId,
                        principalSchema: "cred",
                        principalTable: "Profiles",
                        principalColumn: "ProfileId");
                });

            migrationBuilder.CreateTable(
                name: "Endorsements",
                schema: "cred",
                columns: table => new
                {
                    EndorsementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SignedEndorsement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsSigned = table.Column<bool>(type: "bit", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevocationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Revoked = table.Column<bool>(type: "bit", nullable: true),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuerId = table.Column<int>(type: "int", nullable: false),
                    VerificationId = table.Column<int>(type: "int", nullable: false),
                    EndorsementClaimId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Endorsements", x => x.EndorsementId);
                    table.ForeignKey(
                        name: "FK_Endorsements_EndorsementClaims_EndorsementClaimId",
                        column: x => x.EndorsementClaimId,
                        principalSchema: "cred",
                        principalTable: "EndorsementClaims",
                        principalColumn: "EndorsementClaimId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Endorsements_Profiles_IssuerId",
                        column: x => x.IssuerId,
                        principalSchema: "cred",
                        principalTable: "Profiles",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Endorsements_Verifications_VerificationId",
                        column: x => x.VerificationId,
                        principalSchema: "cred",
                        principalTable: "Verifications",
                        principalColumn: "VerificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BadgrBackpacks",
                schema: "cred",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentCredentialPackageId = table.Column<int>(type: "int", nullable: false),
                    AssertionsCount = table.Column<int>(type: "int", nullable: false),
                    IssuedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Provider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsBadgr = table.Column<bool>(type: "bit", nullable: false),
                    Revoked = table.Column<bool>(type: "bit", nullable: false),
                    RevocationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BadgrBackpacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BadgrBackpacks_CredentialPackages_ParentCredentialPackageId",
                        column: x => x.ParentCredentialPackageId,
                        principalSchema: "cred",
                        principalTable: "CredentialPackages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CredentialRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentContextId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WalletRelationshipId = table.Column<int>(type: "int", nullable: false),
                    ThreadId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CredentialRequestStep = table.Column<int>(type: "int", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CredentialPackageId = table.Column<int>(type: "int", nullable: false),
                    CredentialDefinitionId = table.Column<int>(type: "int", nullable: true),
                    CredentialSchemaId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    WalletRelationshipModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CredentialRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CredentialRequests_AgentContexts_AgentContextId",
                        column: x => x.AgentContextId,
                        principalTable: "AgentContexts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CredentialRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CredentialRequests_CredentialDefinitions_CredentialDefinitionId",
                        column: x => x.CredentialDefinitionId,
                        principalTable: "CredentialDefinitions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CredentialRequests_CredentialPackages_CredentialPackageId",
                        column: x => x.CredentialPackageId,
                        principalSchema: "cred",
                        principalTable: "CredentialPackages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CredentialRequests_CredentialSchema_CredentialSchemaId",
                        column: x => x.CredentialSchemaId,
                        principalTable: "CredentialSchema",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CredentialRequests_WalletRelationships_WalletRelationshipId",
                        column: x => x.WalletRelationshipId,
                        principalTable: "WalletRelationships",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CredentialRequests_WalletRelationships_WalletRelationshipModelId",
                        column: x => x.WalletRelationshipModelId,
                        principalTable: "WalletRelationships",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShareRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommonId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CredentialPackageId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShareRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ShareRequests_CredentialPackages_CredentialPackageId",
                        column: x => x.CredentialPackageId,
                        principalSchema: "cred",
                        principalTable: "CredentialPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VerifiableCredentials",
                schema: "cred",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentCredentialPackageId = table.Column<int>(type: "int", nullable: false),
                    CredentialsCount = table.Column<int>(type: "int", nullable: false),
                    Types = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Issuer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Revoked = table.Column<bool>(type: "bit", nullable: false),
                    RevocationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerifiableCredentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VerifiableCredentials_CredentialPackages_ParentCredentialPackageId",
                        column: x => x.ParentCredentialPackageId,
                        principalSchema: "cred",
                        principalTable: "CredentialPackages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NameVerifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VerificationId = table.Column<int>(type: "int", nullable: false),
                    GivenName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Correct = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NameVerifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NameVerifications_CandidateVerifications_VerificationId",
                        column: x => x.VerificationId,
                        principalTable: "CandidateVerifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StreetAddressVerifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VerificationId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Correct = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreetAddressVerifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StreetAddressVerifications_CandidateVerifications_VerificationId",
                        column: x => x.VerificationId,
                        principalTable: "CandidateVerifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestId = table.Column<int>(type: "int", nullable: false),
                    TestCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PointsEarned = table.Column<short>(type: "smallint", nullable: false),
                    PointsAvailable = table.Column<short>(type: "smallint", nullable: false),
                    AvgPerformanceRange = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestCategories_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AchievementAlignments",
                schema: "cred",
                columns: table => new
                {
                    AchievementAlignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AchievementId = table.Column<int>(type: "int", nullable: false),
                    AlignmentId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AchievementAlignments", x => x.AchievementAlignmentId);
                    table.ForeignKey(
                        name: "FK_AchievementAlignments_Achievements_AchievementId",
                        column: x => x.AchievementId,
                        principalSchema: "cred",
                        principalTable: "Achievements",
                        principalColumn: "AchievementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AchievementAlignments_Alignments_AlignmentId",
                        column: x => x.AlignmentId,
                        principalSchema: "cred",
                        principalTable: "Alignments",
                        principalColumn: "AlignmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AchievementAssociations",
                schema: "cred",
                columns: table => new
                {
                    AchievementAssociationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AchievementId = table.Column<int>(type: "int", nullable: false),
                    AssociationId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AchievementAssociations", x => x.AchievementAssociationId);
                    table.ForeignKey(
                        name: "FK_AchievementAssociations_Achievements_AchievementId",
                        column: x => x.AchievementId,
                        principalSchema: "cred",
                        principalTable: "Achievements",
                        principalColumn: "AchievementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AchievementAssociations_Associations_AssociationId",
                        column: x => x.AssociationId,
                        principalSchema: "cred",
                        principalTable: "Associations",
                        principalColumn: "AssociationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Assertions",
                schema: "cred",
                columns: table => new
                {
                    AssertionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SignedAssertion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreditsEarned = table.Column<float>(type: "real", nullable: true),
                    ActivityEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LicenseNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Narrative = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RevocationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Revoked = table.Column<bool>(type: "bit", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignedEndorsements = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActivityStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Term = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Context = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSigned = table.Column<bool>(type: "bit", nullable: false),
                    IsSelfPublished = table.Column<bool>(type: "bit", nullable: false),
                    Json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceId = table.Column<int>(type: "int", nullable: true),
                    VerificationId = table.Column<int>(type: "int", nullable: true),
                    RecipientId = table.Column<int>(type: "int", nullable: true),
                    AchievementId = table.Column<int>(type: "int", nullable: true),
                    ParentAssertionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assertions", x => x.AssertionId);
                    table.ForeignKey(
                        name: "FK_Assertions_Achievements_AchievementId",
                        column: x => x.AchievementId,
                        principalSchema: "cred",
                        principalTable: "Achievements",
                        principalColumn: "AchievementId");
                    table.ForeignKey(
                        name: "FK_Assertions_Assertions_ParentAssertionId",
                        column: x => x.ParentAssertionId,
                        principalSchema: "cred",
                        principalTable: "Assertions",
                        principalColumn: "AssertionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assertions_Identities_RecipientId",
                        column: x => x.RecipientId,
                        principalSchema: "cred",
                        principalTable: "Identities",
                        principalColumn: "IdentityId");
                    table.ForeignKey(
                        name: "FK_Assertions_Profiles_SourceId",
                        column: x => x.SourceId,
                        principalSchema: "cred",
                        principalTable: "Profiles",
                        principalColumn: "ProfileId");
                    table.ForeignKey(
                        name: "FK_Assertions_Verifications_VerificationId",
                        column: x => x.VerificationId,
                        principalSchema: "cred",
                        principalTable: "Verifications",
                        principalColumn: "VerificationId");
                });

            migrationBuilder.CreateTable(
                name: "ResultDescriptions",
                schema: "cred",
                columns: table => new
                {
                    ResultDescriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllowedValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequiredLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiredValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValueMax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValueMin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AchievementId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultDescriptions", x => x.ResultDescriptionId);
                    table.ForeignKey(
                        name: "FK_ResultDescriptions_Achievements_AchievementId",
                        column: x => x.AchievementId,
                        principalSchema: "cred",
                        principalTable: "Achievements",
                        principalColumn: "AchievementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AchievementEndorsements",
                schema: "cred",
                columns: table => new
                {
                    AchievementEndorsementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AchievementId = table.Column<int>(type: "int", nullable: false),
                    EndorsementId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AchievementEndorsements", x => x.AchievementEndorsementId);
                    table.ForeignKey(
                        name: "FK_AchievementEndorsements_Achievements_AchievementId",
                        column: x => x.AchievementId,
                        principalSchema: "cred",
                        principalTable: "Achievements",
                        principalColumn: "AchievementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AchievementEndorsements_Endorsements_EndorsementId",
                        column: x => x.EndorsementId,
                        principalSchema: "cred",
                        principalTable: "Endorsements",
                        principalColumn: "EndorsementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfileEndorsements",
                schema: "cred",
                columns: table => new
                {
                    ProfileEndorsementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileId = table.Column<int>(type: "int", nullable: false),
                    EndorsementId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileEndorsements", x => x.ProfileEndorsementId);
                    table.ForeignKey(
                        name: "FK_ProfileEndorsements_Endorsements_EndorsementId",
                        column: x => x.EndorsementId,
                        principalSchema: "cred",
                        principalTable: "Endorsements",
                        principalColumn: "EndorsementId");
                    table.ForeignKey(
                        name: "FK_ProfileEndorsements_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalSchema: "cred",
                        principalTable: "Profiles",
                        principalColumn: "ProfileId");
                });

            migrationBuilder.CreateTable(
                name: "BadgrAssertions",
                schema: "cred",
                columns: table => new
                {
                    BadgrAssertionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IssuerJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BadgeJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecipientJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BadgeClassJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BadgrBackpackId = table.Column<int>(type: "int", nullable: false),
                    IsBadgr = table.Column<bool>(type: "bit", nullable: false),
                    IsValidJson = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Acceptance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpenBadgeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Badgeclass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BadgeClassOpenBadgeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Issuer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuerOpenBadgeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageObjectId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IssuedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Narrative = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Revoked = table.Column<bool>(type: "bit", nullable: true),
                    RevocationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Pending = table.Column<bool>(type: "bit", nullable: false),
                    IssueStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValidationStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternalIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignedAssertion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BadgrAssertions", x => x.BadgrAssertionId);
                    table.ForeignKey(
                        name: "FK_BadgrAssertions_BadgrBackpacks_BadgrBackpackId",
                        column: x => x.BadgrBackpackId,
                        principalSchema: "cred",
                        principalTable: "BadgrBackpacks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BadgrAssertions_BadgrImageDType_ImageObjectId",
                        column: x => x.ImageObjectId,
                        principalTable: "BadgrImageDType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClrSets",
                schema: "cred",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentCredentialPackageId = table.Column<int>(type: "int", nullable: true),
                    ParentVerifiableCredentialId = table.Column<int>(type: "int", nullable: true),
                    ClrsCount = table.Column<int>(type: "int", nullable: false),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClrSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClrSets_CredentialPackages_ParentCredentialPackageId",
                        column: x => x.ParentCredentialPackageId,
                        principalSchema: "cred",
                        principalTable: "CredentialPackages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClrSets_VerifiableCredentials_ParentVerifiableCredentialId",
                        column: x => x.ParentVerifiableCredentialId,
                        principalSchema: "cred",
                        principalTable: "VerifiableCredentials",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CredentialSubjects",
                schema: "cred",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ParentVerifiableCredentialId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CredentialSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CredentialSubjects_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CredentialSubjects_VerifiableCredentials_ParentVerifiableCredentialId",
                        column: x => x.ParentVerifiableCredentialId,
                        principalSchema: "cred",
                        principalTable: "VerifiableCredentials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssertionEndorsements",
                schema: "cred",
                columns: table => new
                {
                    AssertionEndorsementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssertionId = table.Column<int>(type: "int", nullable: false),
                    EndorsementId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssertionEndorsements", x => x.AssertionEndorsementId);
                    table.ForeignKey(
                        name: "FK_AssertionEndorsements_Assertions_AssertionId",
                        column: x => x.AssertionId,
                        principalSchema: "cred",
                        principalTable: "Assertions",
                        principalColumn: "AssertionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssertionEndorsements_Endorsements_EndorsementId",
                        column: x => x.EndorsementId,
                        principalSchema: "cred",
                        principalTable: "Endorsements",
                        principalColumn: "EndorsementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssertionEvidence",
                schema: "cred",
                columns: table => new
                {
                    AssertionEvidenceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssertionId = table.Column<int>(type: "int", nullable: false),
                    EvidenceId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssertionEvidence", x => x.AssertionEvidenceId);
                    table.ForeignKey(
                        name: "FK_AssertionEvidence_Assertions_AssertionId",
                        column: x => x.AssertionId,
                        principalSchema: "cred",
                        principalTable: "Assertions",
                        principalColumn: "AssertionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssertionEvidence_Evidence_EvidenceId",
                        column: x => x.EvidenceId,
                        principalSchema: "cred",
                        principalTable: "Evidence",
                        principalColumn: "EvidenceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                schema: "cred",
                columns: table => new
                {
                    ResultId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AchievedLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssertionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.ResultId);
                    table.ForeignKey(
                        name: "FK_Results_Assertions_AssertionId",
                        column: x => x.AssertionId,
                        principalSchema: "cred",
                        principalTable: "Assertions",
                        principalColumn: "AssertionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResultDescriptionAlignments",
                schema: "cred",
                columns: table => new
                {
                    ResultDescriptionAlignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResultDescriptionId = table.Column<int>(type: "int", nullable: false),
                    AlignmentId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultDescriptionAlignments", x => x.ResultDescriptionAlignmentId);
                    table.ForeignKey(
                        name: "FK_ResultDescriptionAlignments_Alignments_AlignmentId",
                        column: x => x.AlignmentId,
                        principalSchema: "cred",
                        principalTable: "Alignments",
                        principalColumn: "AlignmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResultDescriptionAlignments_ResultDescriptions_ResultDescriptionId",
                        column: x => x.ResultDescriptionId,
                        principalSchema: "cred",
                        principalTable: "ResultDescriptions",
                        principalColumn: "ResultDescriptionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RubricCriterionLevels",
                schema: "cred",
                columns: table => new
                {
                    RubricCriterionLevelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Points = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultDescriptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RubricCriterionLevels", x => x.RubricCriterionLevelId);
                    table.ForeignKey(
                        name: "FK_RubricCriterionLevels_ResultDescriptions_ResultDescriptionId",
                        column: x => x.ResultDescriptionId,
                        principalSchema: "cred",
                        principalTable: "ResultDescriptions",
                        principalColumn: "ResultDescriptionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clrs",
                schema: "cred",
                columns: table => new
                {
                    ClrId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorizationForeignKey = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AssertionsCount = table.Column<int>(type: "int", nullable: false),
                    Json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LearnerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublisherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Context = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Partial = table.Column<bool>(type: "bit", nullable: true),
                    RevocationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Revoked = table.Column<bool>(type: "bit", nullable: true),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignedClr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CredentialPackageId = table.Column<int>(type: "int", nullable: false),
                    ParentCredentialPackageId = table.Column<int>(type: "int", nullable: true),
                    ParentVerifiableCredentialId = table.Column<int>(type: "int", nullable: true),
                    ParentClrSetId = table.Column<int>(type: "int", nullable: true),
                    LearnerId = table.Column<int>(type: "int", nullable: false),
                    PublisherId = table.Column<int>(type: "int", nullable: false),
                    VerificationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clrs", x => x.ClrId);
                    table.ForeignKey(
                        name: "FK_Clrs_Authorizations_AuthorizationForeignKey",
                        column: x => x.AuthorizationForeignKey,
                        principalTable: "Authorizations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Clrs_ClrSets_ParentClrSetId",
                        column: x => x.ParentClrSetId,
                        principalSchema: "cred",
                        principalTable: "ClrSets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Clrs_CredentialPackages_CredentialPackageId",
                        column: x => x.CredentialPackageId,
                        principalSchema: "cred",
                        principalTable: "CredentialPackages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Clrs_CredentialPackages_ParentCredentialPackageId",
                        column: x => x.ParentCredentialPackageId,
                        principalSchema: "cred",
                        principalTable: "CredentialPackages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Clrs_Profiles_LearnerId",
                        column: x => x.LearnerId,
                        principalSchema: "cred",
                        principalTable: "Profiles",
                        principalColumn: "ProfileId");
                    table.ForeignKey(
                        name: "FK_Clrs_Profiles_PublisherId",
                        column: x => x.PublisherId,
                        principalSchema: "cred",
                        principalTable: "Profiles",
                        principalColumn: "ProfileId");
                    table.ForeignKey(
                        name: "FK_Clrs_VerifiableCredentials_ParentVerifiableCredentialId",
                        column: x => x.ParentVerifiableCredentialId,
                        principalSchema: "cred",
                        principalTable: "VerifiableCredentials",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Clrs_Verifications_VerificationId",
                        column: x => x.VerificationId,
                        principalSchema: "cred",
                        principalTable: "Verifications",
                        principalColumn: "VerificationId");
                });

            migrationBuilder.CreateTable(
                name: "ResultAlignments",
                schema: "cred",
                columns: table => new
                {
                    ResultAlignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResultId = table.Column<int>(type: "int", nullable: false),
                    AlignmentId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultAlignments", x => x.ResultAlignmentId);
                    table.ForeignKey(
                        name: "FK_ResultAlignments_Alignments_AlignmentId",
                        column: x => x.AlignmentId,
                        principalSchema: "cred",
                        principalTable: "Alignments",
                        principalColumn: "AlignmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResultAlignments_Results_ResultId",
                        column: x => x.ResultId,
                        principalSchema: "cred",
                        principalTable: "Results",
                        principalColumn: "ResultId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RubricCriterionLevelAlignments",
                schema: "cred",
                columns: table => new
                {
                    RubricCriterionLevelAlignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RubricCriterionLevelId = table.Column<int>(type: "int", nullable: false),
                    AlignmentId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RubricCriterionLevelAlignments", x => x.RubricCriterionLevelAlignmentId);
                    table.ForeignKey(
                        name: "FK_RubricCriterionLevelAlignments_Alignments_AlignmentId",
                        column: x => x.AlignmentId,
                        principalSchema: "cred",
                        principalTable: "Alignments",
                        principalColumn: "AlignmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RubricCriterionLevelAlignments_RubricCriterionLevels_RubricCriterionLevelId",
                        column: x => x.RubricCriterionLevelId,
                        principalSchema: "cred",
                        principalTable: "RubricCriterionLevels",
                        principalColumn: "RubricCriterionLevelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Artifacts",
                schema: "cred",
                columns: table => new
                {
                    ArtifactId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsPdf = table.Column<bool>(type: "bit", nullable: false),
                    IsUrl = table.Column<bool>(type: "bit", nullable: false),
                    MediaType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameContainsTranscript = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClrId = table.Column<int>(type: "int", nullable: true),
                    AssertionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClrIssuedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClrName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EvidenceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artifacts", x => x.ArtifactId);
                    table.ForeignKey(
                        name: "FK_Artifacts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Artifacts_Clrs_ClrId",
                        column: x => x.ClrId,
                        principalSchema: "cred",
                        principalTable: "Clrs",
                        principalColumn: "ClrId");
                });

            migrationBuilder.CreateTable(
                name: "ClrAchievements",
                schema: "cred",
                columns: table => new
                {
                    ClrAchievementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClrId = table.Column<int>(type: "int", nullable: false),
                    AchievementId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClrAchievements", x => x.ClrAchievementId);
                    table.ForeignKey(
                        name: "FK_ClrAchievements_Achievements_AchievementId",
                        column: x => x.AchievementId,
                        principalSchema: "cred",
                        principalTable: "Achievements",
                        principalColumn: "AchievementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClrAchievements_Clrs_ClrId",
                        column: x => x.ClrId,
                        principalSchema: "cred",
                        principalTable: "Clrs",
                        principalColumn: "ClrId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClrAssertions",
                schema: "cred",
                columns: table => new
                {
                    ClrAssertionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClrId = table.Column<int>(type: "int", nullable: false),
                    AssertionId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClrAssertions", x => x.ClrAssertionId);
                    table.ForeignKey(
                        name: "FK_ClrAssertions_Assertions_AssertionId",
                        column: x => x.AssertionId,
                        principalSchema: "cred",
                        principalTable: "Assertions",
                        principalColumn: "AssertionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClrAssertions_Clrs_ClrId",
                        column: x => x.ClrId,
                        principalSchema: "cred",
                        principalTable: "Clrs",
                        principalColumn: "ClrId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClrEndorsements",
                schema: "cred",
                columns: table => new
                {
                    ClrEndorsementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClrId = table.Column<int>(type: "int", nullable: false),
                    EndorsementId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClrEndorsements", x => x.ClrEndorsementId);
                    table.ForeignKey(
                        name: "FK_ClrEndorsements_Clrs_ClrId",
                        column: x => x.ClrId,
                        principalSchema: "cred",
                        principalTable: "Clrs",
                        principalColumn: "ClrId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClrEndorsements_Endorsements_EndorsementId",
                        column: x => x.EndorsementId,
                        principalSchema: "cred",
                        principalTable: "Endorsements",
                        principalColumn: "EndorsementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CredentialOffers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShareRequestId = table.Column<int>(type: "int", nullable: false),
                    OfferId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfferContents = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfferPayload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QrCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClrId = table.Column<int>(type: "int", nullable: false),
                    CredentialDefinitionId = table.Column<int>(type: "int", nullable: true),
                    CredentialSchemaId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CredentialOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CredentialOffers_Clrs_ClrId",
                        column: x => x.ClrId,
                        principalSchema: "cred",
                        principalTable: "Clrs",
                        principalColumn: "ClrId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CredentialOffers_CredentialDefinitions_CredentialDefinitionId",
                        column: x => x.CredentialDefinitionId,
                        principalTable: "CredentialDefinitions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CredentialOffers_CredentialSchema_CredentialSchemaId",
                        column: x => x.CredentialSchemaId,
                        principalTable: "CredentialSchema",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CredentialOffers_ShareRequests_ShareRequestId",
                        column: x => x.ShareRequestId,
                        principalTable: "ShareRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClrForeignKey = table.Column<int>(type: "int", nullable: false),
                    DisplayCount = table.Column<int>(type: "int", nullable: false),
                    Nickname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CredentialRequestId = table.Column<int>(type: "int", nullable: true),
                    LinkId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShareTypeId = table.Column<int>(type: "int", nullable: false),
                    RecipientId = table.Column<int>(type: "int", nullable: true),
                    AccessKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UseCount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UniqueExternalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shares_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Shares_Clrs_ClrForeignKey",
                        column: x => x.ClrForeignKey,
                        principalSchema: "cred",
                        principalTable: "Clrs",
                        principalColumn: "ClrId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Shares_CredentialRequests_CredentialRequestId",
                        column: x => x.CredentialRequestId,
                        principalTable: "CredentialRequests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Shares_Recipients_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "Recipients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Shares_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SmartResumes",
                schema: "idatafy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClrId = table.Column<int>(type: "int", nullable: false),
                    SmartResumeUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsReady = table.Column<bool>(type: "bit", nullable: false),
                    HasError = table.Column<bool>(type: "bit", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartResumes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartResumes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmartResumes_Clrs_ClrId",
                        column: x => x.ClrId,
                        principalSchema: "cred",
                        principalTable: "Clrs",
                        principalColumn: "ClrId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EvidenceArtifacts",
                schema: "cred",
                columns: table => new
                {
                    EvidenceArtifactId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EvidenceId = table.Column<int>(type: "int", nullable: false),
                    ArtifactId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvidenceArtifacts", x => x.EvidenceArtifactId);
                    table.ForeignKey(
                        name: "FK_EvidenceArtifacts_Artifacts_ArtifactId",
                        column: x => x.ArtifactId,
                        principalSchema: "cred",
                        principalTable: "Artifacts",
                        principalColumn: "ArtifactId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EvidenceArtifacts_Evidence_EvidenceId",
                        column: x => x.EvidenceId,
                        principalSchema: "cred",
                        principalTable: "Evidence",
                        principalColumn: "EvidenceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ETSCreditTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommerceTransactionId = table.Column<int>(type: "int", nullable: true),
                    ShareId = table.Column<int>(type: "int", nullable: true),
                    CredentialRequestId = table.Column<int>(type: "int", nullable: true),
                    IsDebit = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsAdjustment = table.Column<bool>(type: "bit", nullable: false),
                    AdjustmentUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ETSCreditTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ETSCreditTransactions_CommerceTransactions_CommerceTransactionId",
                        column: x => x.CommerceTransactionId,
                        principalTable: "CommerceTransactions",
                        principalColumn: "TransactionId");
                    table.ForeignKey(
                        name: "FK_ETSCreditTransactions_CredentialRequests_CredentialRequestId",
                        column: x => x.CredentialRequestId,
                        principalTable: "CredentialRequests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ETSCreditTransactions_Shares_ShareId",
                        column: x => x.ShareId,
                        principalTable: "Shares",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Recipient = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SendAttempts = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ShareId = table.Column<int>(type: "int", nullable: true),
                    ProofRequestId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_ProofRequests_ProofRequestId",
                        column: x => x.ProofRequestId,
                        principalTable: "ProofRequests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Messages_Shares_ShareId",
                        column: x => x.ShareId,
                        principalTable: "Shares",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmailVerifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerificationString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfferId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfferContents = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfferPayload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailVerificationCredentialQrCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    MessageId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ValidUntil = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailVerifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailVerifications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmailVerifications_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "ConnectionRequestSteps",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "Initiated" },
                    { 1, "PendingAgent" },
                    { 2, "StartingInvitation" },
                    { 3, "RequestingInvitation" },
                    { 4, "InvitationGenerated" },
                    { 5, "InvitationAccepted" },
                    { 6, "InvitationCompleted" },
                    { 13, "Error" }
                });

            migrationBuilder.InsertData(
                table: "CredentialRequestSteps",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "Initiated" },
                    { 1, "PendingAgent" },
                    { 2, "PendingSchema" },
                    { 3, "PendingCredentialDefinition" },
                    { 4, "ReadyToSend" },
                    { 5, "SendingOffer" },
                    { 6, "OfferSent" },
                    { 7, "OfferAccepted" },
                    { 8, "CheckingRevocationStatus" },
                    { 9, "CredentialIsRevoked" },
                    { 10, "CredentialIsStillValid" },
                    { 11, "PendingSchemaEndorsement" },
                    { 12, "PendingCredentialDefinitionEndorsement" },
                    { 13, "Error" },
                    { 14, "ErrorWritingSchema" },
                    { 15, "ErrorWritingCredentialDefinition" }
                });

            migrationBuilder.InsertData(
                table: "ProofRequestSteps",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Created" },
                    { 2, "WaitingForAgentContext" },
                    { 3, "InvitationLinkRequested" },
                    { 4, "InvitationLinkReceived" },
                    { 5, "ProofReceived" },
                    { 6, "RequestedRelationship" },
                    { 7, "CreatedRelationship" },
                    { 8, "ReceivingProofResponse" },
                    { 9, "Deleted" }
                });

            migrationBuilder.InsertData(
                table: "ShareTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Email" },
                    { 2, "Pdf" },
                    { 3, "Wallet" }
                });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Pending" },
                    { 2, "Accepted" },
                    { 3, "Used" },
                    { 4, "Expired" },
                    { 5, "Rejected" },
                    { 6, "Created" },
                    { 7, "Deleted" },
                    { 8, "Visible" },
                    { 9, "Hidden" },
                    { 10, "Submitted" },
                    { 11, "Active" },
                    { 12, "Sent" },
                    { 13, "Error" },
                    { 14, "WaitingForScoreReport" },
                    { 15, "ReadyForVerification" },
                    { 16, "Verified" },
                    { 17, "Unused" },
                    { 18, "Success" },
                    { 19, "NeedsEndorsement" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AchievementAlignments_AchievementId",
                schema: "cred",
                table: "AchievementAlignments",
                column: "AchievementId");

            migrationBuilder.CreateIndex(
                name: "IX_AchievementAlignments_AlignmentId",
                schema: "cred",
                table: "AchievementAlignments",
                column: "AlignmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AchievementAssociations_AchievementId",
                schema: "cred",
                table: "AchievementAssociations",
                column: "AchievementId");

            migrationBuilder.CreateIndex(
                name: "IX_AchievementAssociations_AssociationId",
                schema: "cred",
                table: "AchievementAssociations",
                column: "AssociationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AchievementEndorsements_AchievementId",
                schema: "cred",
                table: "AchievementEndorsements",
                column: "AchievementId");

            migrationBuilder.CreateIndex(
                name: "IX_AchievementEndorsements_EndorsementId",
                schema: "cred",
                table: "AchievementEndorsements",
                column: "EndorsementId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Achievements_CriteriaId",
                schema: "cred",
                table: "Achievements",
                column: "CriteriaId",
                unique: true,
                filter: "[CriteriaId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Achievements_IssuerProfileId",
                schema: "cred",
                table: "Achievements",
                column: "IssuerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Artifacts_ClrId",
                schema: "cred",
                table: "Artifacts",
                column: "ClrId");

            migrationBuilder.CreateIndex(
                name: "IX_Artifacts_UserId",
                schema: "cred",
                table: "Artifacts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AssertionEndorsements_AssertionId",
                schema: "cred",
                table: "AssertionEndorsements",
                column: "AssertionId");

            migrationBuilder.CreateIndex(
                name: "IX_AssertionEndorsements_EndorsementId",
                schema: "cred",
                table: "AssertionEndorsements",
                column: "EndorsementId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssertionEvidence_AssertionId",
                schema: "cred",
                table: "AssertionEvidence",
                column: "AssertionId");

            migrationBuilder.CreateIndex(
                name: "IX_AssertionEvidence_EvidenceId",
                schema: "cred",
                table: "AssertionEvidence",
                column: "EvidenceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assertions_AchievementId",
                schema: "cred",
                table: "Assertions",
                column: "AchievementId",
                unique: true,
                filter: "[AchievementId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Assertions_ParentAssertionId",
                schema: "cred",
                table: "Assertions",
                column: "ParentAssertionId");

            migrationBuilder.CreateIndex(
                name: "IX_Assertions_RecipientId",
                schema: "cred",
                table: "Assertions",
                column: "RecipientId",
                unique: true,
                filter: "[RecipientId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Assertions_SourceId",
                schema: "cred",
                table: "Assertions",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Assertions_VerificationId",
                schema: "cred",
                table: "Assertions",
                column: "VerificationId",
                unique: true,
                filter: "[VerificationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_SourceForeignKey",
                table: "Authorizations",
                column: "SourceForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_UserId",
                table: "Authorizations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BadgrAssertions_BadgrBackpackId",
                schema: "cred",
                table: "BadgrAssertions",
                column: "BadgrBackpackId");

            migrationBuilder.CreateIndex(
                name: "IX_BadgrAssertions_ImageObjectId",
                schema: "cred",
                table: "BadgrAssertions",
                column: "ImageObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_BadgrBackpacks_ParentCredentialPackageId",
                schema: "cred",
                table: "BadgrBackpacks",
                column: "ParentCredentialPackageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_StatusId",
                table: "Candidates",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_UserId",
                table: "Candidates",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateVerifications_CandidateId",
                table: "CandidateVerifications",
                column: "CandidateId",
                unique: true,
                filter: "[CandidateId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateVerifications_UserId",
                table: "CandidateVerifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClrAchievements_AchievementId",
                schema: "cred",
                table: "ClrAchievements",
                column: "AchievementId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClrAchievements_ClrId",
                schema: "cred",
                table: "ClrAchievements",
                column: "ClrId");

            migrationBuilder.CreateIndex(
                name: "IX_ClrAssertions_AssertionId",
                schema: "cred",
                table: "ClrAssertions",
                column: "AssertionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClrAssertions_ClrId",
                schema: "cred",
                table: "ClrAssertions",
                column: "ClrId");

            migrationBuilder.CreateIndex(
                name: "IX_ClrEndorsements_ClrId",
                schema: "cred",
                table: "ClrEndorsements",
                column: "ClrId");

            migrationBuilder.CreateIndex(
                name: "IX_ClrEndorsements_EndorsementId",
                schema: "cred",
                table: "ClrEndorsements",
                column: "EndorsementId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clrs_AuthorizationForeignKey",
                schema: "cred",
                table: "Clrs",
                column: "AuthorizationForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_Clrs_CredentialPackageId",
                schema: "cred",
                table: "Clrs",
                column: "CredentialPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Clrs_LearnerId",
                schema: "cred",
                table: "Clrs",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Clrs_ParentClrSetId",
                schema: "cred",
                table: "Clrs",
                column: "ParentClrSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Clrs_ParentCredentialPackageId",
                schema: "cred",
                table: "Clrs",
                column: "ParentCredentialPackageId",
                unique: true,
                filter: "[ParentCredentialPackageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Clrs_ParentVerifiableCredentialId",
                schema: "cred",
                table: "Clrs",
                column: "ParentVerifiableCredentialId");

            migrationBuilder.CreateIndex(
                name: "IX_Clrs_PublisherId",
                schema: "cred",
                table: "Clrs",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_Clrs_VerificationId",
                schema: "cred",
                table: "Clrs",
                column: "VerificationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClrSets_ParentCredentialPackageId",
                schema: "cred",
                table: "ClrSets",
                column: "ParentCredentialPackageId",
                unique: true,
                filter: "[ParentCredentialPackageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClrSets_ParentVerifiableCredentialId",
                schema: "cred",
                table: "ClrSets",
                column: "ParentVerifiableCredentialId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionRequests_AgentContextId",
                table: "ConnectionRequests",
                column: "AgentContextId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionRequests_ThreadId",
                table: "ConnectionRequests",
                column: "ThreadId",
                unique: true,
                filter: "[ThreadId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionRequests_UserId",
                table: "ConnectionRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionRequests_WalletRelationshipId",
                table: "ConnectionRequests",
                column: "WalletRelationshipId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialDefinitions_AgentContextId",
                table: "CredentialDefinitions",
                column: "AgentContextId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialDefinitions_CredentialSchemaId",
                table: "CredentialDefinitions",
                column: "CredentialSchemaId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialOffers_ClrId",
                table: "CredentialOffers",
                column: "ClrId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialOffers_CredentialDefinitionId",
                table: "CredentialOffers",
                column: "CredentialDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialOffers_CredentialSchemaId",
                table: "CredentialOffers",
                column: "CredentialSchemaId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialOffers_ShareRequestId",
                table: "CredentialOffers",
                column: "ShareRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialPackages_AuthorizationForeignKey",
                schema: "cred",
                table: "CredentialPackages",
                column: "AuthorizationForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialPackages_ScoreReportId",
                schema: "cred",
                table: "CredentialPackages",
                column: "ScoreReportId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialPackages_UserId",
                schema: "cred",
                table: "CredentialPackages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialRequests_AgentContextId",
                table: "CredentialRequests",
                column: "AgentContextId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialRequests_CredentialDefinitionId",
                table: "CredentialRequests",
                column: "CredentialDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialRequests_CredentialPackageId",
                table: "CredentialRequests",
                column: "CredentialPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialRequests_CredentialSchemaId",
                table: "CredentialRequests",
                column: "CredentialSchemaId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialRequests_UserId",
                table: "CredentialRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialRequests_WalletRelationshipId",
                table: "CredentialRequests",
                column: "WalletRelationshipId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialRequests_WalletRelationshipModelId",
                table: "CredentialRequests",
                column: "WalletRelationshipModelId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialSubjects_ParentVerifiableCredentialId",
                schema: "cred",
                table: "CredentialSubjects",
                column: "ParentVerifiableCredentialId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialSubjects_UserId",
                schema: "cred",
                table: "CredentialSubjects",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscoveryDocumentModel_SourceForeignKey",
                schema: "cred",
                table: "DiscoveryDocumentModel",
                column: "SourceForeignKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailVerifications_MessageId",
                table: "EmailVerifications",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailVerifications_UserId",
                table: "EmailVerifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Endorsements_EndorsementClaimId",
                schema: "cred",
                table: "Endorsements",
                column: "EndorsementClaimId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Endorsements_IssuerId",
                schema: "cred",
                table: "Endorsements",
                column: "IssuerId");

            migrationBuilder.CreateIndex(
                name: "IX_Endorsements_VerificationId",
                schema: "cred",
                table: "Endorsements",
                column: "VerificationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ETSCreditTransactions_CommerceTransactionId",
                table: "ETSCreditTransactions",
                column: "CommerceTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_ETSCreditTransactions_CredentialRequestId",
                table: "ETSCreditTransactions",
                column: "CredentialRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ETSCreditTransactions_ShareId",
                table: "ETSCreditTransactions",
                column: "ShareId");

            migrationBuilder.CreateIndex(
                name: "IX_EvidenceArtifacts_ArtifactId",
                schema: "cred",
                table: "EvidenceArtifacts",
                column: "ArtifactId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EvidenceArtifacts_EvidenceId",
                schema: "cred",
                table: "EvidenceArtifacts",
                column: "EvidenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Folio_StatusId",
                table: "Folio",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Folio_UserId",
                table: "Folio",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LoginProofRequests_UserId",
                table: "LoginProofRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ProofRequestId",
                table: "Messages",
                column: "ProofRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ShareId",
                table: "Messages",
                column: "ShareId");

            migrationBuilder.CreateIndex(
                name: "IX_NameVerifications_VerificationId",
                table: "NameVerifications",
                column: "VerificationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileEndorsements_EndorsementId",
                schema: "cred",
                table: "ProfileEndorsements",
                column: "EndorsementId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfileEndorsements_ProfileId",
                schema: "cred",
                table: "ProfileEndorsements",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_ParentProfileId",
                schema: "cred",
                table: "Profiles",
                column: "ParentProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_VerificationId",
                schema: "cred",
                table: "Profiles",
                column: "VerificationId",
                unique: true,
                filter: "[VerificationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProofRequests_CredentialSchemaId",
                table: "ProofRequests",
                column: "CredentialSchemaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProofRequests_UserId",
                table: "ProofRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProofResponses_ProofRequestId",
                table: "ProofResponses",
                column: "ProofRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProvisioningToken_AgentContextId",
                table: "ProvisioningToken",
                column: "AgentContextId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipients_UserId",
                table: "Recipients",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultAlignments_AlignmentId",
                schema: "cred",
                table: "ResultAlignments",
                column: "AlignmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResultAlignments_ResultId",
                schema: "cred",
                table: "ResultAlignments",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultDescriptionAlignments_AlignmentId",
                schema: "cred",
                table: "ResultDescriptionAlignments",
                column: "AlignmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResultDescriptionAlignments_ResultDescriptionId",
                schema: "cred",
                table: "ResultDescriptionAlignments",
                column: "ResultDescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultDescriptions_AchievementId",
                schema: "cred",
                table: "ResultDescriptions",
                column: "AchievementId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_AssertionId",
                schema: "cred",
                table: "Results",
                column: "AssertionId");

            migrationBuilder.CreateIndex(
                name: "IX_Revocations_SourceId",
                table: "Revocations",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Revocations_UserId",
                table: "Revocations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RubricCriterionLevelAlignments_AlignmentId",
                schema: "cred",
                table: "RubricCriterionLevelAlignments",
                column: "AlignmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RubricCriterionLevelAlignments_RubricCriterionLevelId",
                schema: "cred",
                table: "RubricCriterionLevelAlignments",
                column: "RubricCriterionLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_RubricCriterionLevels_ResultDescriptionId",
                schema: "cred",
                table: "RubricCriterionLevels",
                column: "ResultDescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ShareRequests_CredentialPackageId",
                table: "ShareRequests",
                column: "CredentialPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_ShareRequests_UserId",
                table: "ShareRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Shares_ClrForeignKey",
                table: "Shares",
                column: "ClrForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_Shares_CredentialRequestId",
                table: "Shares",
                column: "CredentialRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Shares_RecipientId",
                table: "Shares",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Shares_StatusId",
                table: "Shares",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Shares_UserId",
                table: "Shares",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartResumes_ClrId",
                schema: "idatafy",
                table: "SmartResumes",
                column: "ClrId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SmartResumes_UserId",
                schema: "idatafy",
                table: "SmartResumes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StreetAddressVerifications_VerificationId",
                table: "StreetAddressVerifications",
                column: "VerificationId");

            migrationBuilder.CreateIndex(
                name: "IX_TestCategories_TestId",
                table: "TestCategories",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_FolioId",
                table: "Tests",
                column: "FolioId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_ScoreReportId",
                table: "Tests",
                column: "ScoreReportId");

            migrationBuilder.CreateIndex(
                name: "UIX_Test",
                table: "Tests",
                columns: new[] { "CandidateId", "TestCode", "TestDate" },
                unique: true,
                filter: "[CandidateId] IS NOT NULL AND [TestCode] IS NOT NULL AND [TestDate] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddresses_UserId",
                table: "UserAddresses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCredits_PaymentRequestId",
                table: "UserCredits",
                column: "PaymentRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCredits_StatusId",
                table: "UserCredits",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCredits_UserId",
                table: "UserCredits",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEmails_UserId",
                table: "UserEmails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPaymentRequests_StatusId",
                table: "UserPaymentRequests",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPaymentRequests_UserId",
                table: "UserPaymentRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPhoneNumbers_UserId",
                table: "UserPhoneNumbers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VerifiableCredentials_ParentCredentialPackageId",
                schema: "cred",
                table: "VerifiableCredentials",
                column: "ParentCredentialPackageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WalletRelationships_AgentContextId",
                table: "WalletRelationships",
                column: "AgentContextId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletRelationships_UserId",
                table: "WalletRelationships",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AchievementAlignments",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "AchievementAssociations",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "AchievementEndorsements",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "ApplicationSettings");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AssertionEndorsements",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "AssertionEvidence",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "BadgrAssertions",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.DropTable(
                name: "ClrAchievements",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "ClrAssertions",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "ClrEndorsements",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "CommerceAddresses");

            migrationBuilder.DropTable(
                name: "ConnectionRequests");

            migrationBuilder.DropTable(
                name: "ConnectionRequestSteps");

            migrationBuilder.DropTable(
                name: "CredentialOffers");

            migrationBuilder.DropTable(
                name: "CredentialRequestSteps");

            migrationBuilder.DropTable(
                name: "CredentialSubjects",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "DIDs",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "DiscoveryDocumentModel",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "EmailVerifications");

            migrationBuilder.DropTable(
                name: "ETSCreditTransactions");

            migrationBuilder.DropTable(
                name: "EvidenceArtifacts",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "GivenNames");

            migrationBuilder.DropTable(
                name: "HttpClientLogs");

            migrationBuilder.DropTable(
                name: "IdentityCertificates");

            migrationBuilder.DropTable(
                name: "Keys");

            migrationBuilder.DropTable(
                name: "LoginProofRequests");

            migrationBuilder.DropTable(
                name: "NameVerifications");

            migrationBuilder.DropTable(
                name: "ProfileEndorsements",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "ProofRequestSteps");

            migrationBuilder.DropTable(
                name: "ProofResponses");

            migrationBuilder.DropTable(
                name: "ProvisioningToken");

            migrationBuilder.DropTable(
                name: "ResultAlignments",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "ResultDescriptionAlignments",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "Revocations");

            migrationBuilder.DropTable(
                name: "RubricCriterionLevelAlignments",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "ScoreReportsCandidates");

            migrationBuilder.DropTable(
                name: "ShareTypes");

            migrationBuilder.DropTable(
                name: "SmartResumes",
                schema: "idatafy");

            migrationBuilder.DropTable(
                name: "StreetAddresses");

            migrationBuilder.DropTable(
                name: "StreetAddressVerifications");

            migrationBuilder.DropTable(
                name: "Surnames");

            migrationBuilder.DropTable(
                name: "TestCategories");

            migrationBuilder.DropTable(
                name: "UserAddresses");

            migrationBuilder.DropTable(
                name: "UserCredits");

            migrationBuilder.DropTable(
                name: "UserEmails");

            migrationBuilder.DropTable(
                name: "UserPhoneNumbers");

            migrationBuilder.DropTable(
                name: "VerityThreads");

            migrationBuilder.DropTable(
                name: "Associations",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "BadgrBackpacks",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "BadgrImageDType");

            migrationBuilder.DropTable(
                name: "ShareRequests");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "CommerceTransactions");

            migrationBuilder.DropTable(
                name: "Artifacts",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "Evidence",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "Endorsements",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "Results",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "Alignments",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "RubricCriterionLevels",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "CandidateVerifications");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "UserPaymentRequests");

            migrationBuilder.DropTable(
                name: "ProofRequests");

            migrationBuilder.DropTable(
                name: "Shares");

            migrationBuilder.DropTable(
                name: "EndorsementClaims",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "Assertions",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "ResultDescriptions",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "Candidates");

            migrationBuilder.DropTable(
                name: "Folio");

            migrationBuilder.DropTable(
                name: "Clrs",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "CredentialRequests");

            migrationBuilder.DropTable(
                name: "Recipients");

            migrationBuilder.DropTable(
                name: "Identities",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "Achievements",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "ClrSets",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "CredentialDefinitions");

            migrationBuilder.DropTable(
                name: "WalletRelationships");

            migrationBuilder.DropTable(
                name: "Criteria",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "Profiles",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "VerifiableCredentials",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "CredentialSchema");

            migrationBuilder.DropTable(
                name: "AgentContexts");

            migrationBuilder.DropTable(
                name: "Verifications",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "CredentialPackages",
                schema: "cred");

            migrationBuilder.DropTable(
                name: "Authorizations");

            migrationBuilder.DropTable(
                name: "ScoreReports");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Sources");
        }
    }
}
