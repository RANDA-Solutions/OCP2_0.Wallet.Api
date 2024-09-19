using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenCredentialPublisher.Data.Custom.Migrations
{
    /// <inheritdoc />
    public partial class CredentialsSearchView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE VIEW [cred].[SearchCredentialPackagesView]
AS
SELECT	cp.Id AS [CredentialPackageId],
		cp.TypeId,
		cp.[UserId] AS [OwnerUserId],
		cp.CreatedAt AS [PublishedAt],
		DATEPART(YEAR,cp.CreatedAt) AS [PublishedAtYear],
		c.PublisherName,
		cp.AssertionsCount, 
		vc.CredentialsCount AS [CredentialsCount],
		vc.[Json]
FROM cred.[CredentialPackages] cp (NOLOCK)
JOIN cred.[VerifiableCredentials] vc (NOLOCK) ON cp.Id = vc.ParentCredentialPackageId
JOIN cred.[ClrSets] cs (NOLOCK) ON vc.Id = cs.ParentVerifiableCredentialId
JOIN cred.[Clrs] c (NOLOCK) ON cs.Id = c.ParentClrSetId
WHERE cp.IsDeleted = 0
AND vc.IsDeleted = 0
AND c.IsDeleted = 0
UNION
SELECT	cp.Id AS [CredentialPackageId],
		cp.TypeId,
		cp.[UserId] AS [OwnerUserId],
		cp.CreatedAt AS [PublishedAt],
		DATEPART(YEAR,cp.CreatedAt) AS [PublishedAtYear],
		c.PublisherName,
		cp.AssertionsCount, 
		vc.CredentialsCount AS [CredentialsCount],
		vc.[Json]
FROM cred.[CredentialPackages] cp (NOLOCK)
JOIN cred.[VerifiableCredentials] vc (NOLOCK) ON cp.Id = vc.ParentCredentialPackageId
JOIN cred.[Clrs] c (NOLOCK) ON vc.Id = c.ParentVerifiableCredentialId
WHERE cp.IsDeleted = 0
AND vc.IsDeleted = 0
AND c.IsDeleted = 0
UNION
SELECT	cp.Id AS [CredentialPackageId], 
		cp.TypeId,
		cp.[UserId] AS [OwnerUserId],
		cp.CreatedAt AS [PublishedAt],
		DATEPART(YEAR,cp.CreatedAt) AS [PublishedAtYear],
		c.PublisherName,
		cp.AssertionsCount, 
		0 AS [CredentialsCount],
		cs.[Json]
FROM cred.[CredentialPackages] cp (NOLOCK)
JOIN cred.[ClrSets] cs (NOLOCK) ON cp.Id = cs.ParentCredentialPackageId
JOIN cred.[Clrs] c (NOLOCK) ON cs.Id = c.ParentClrSetId
WHERE cp.IsDeleted = 0
AND cs.IsDeleted = 0
AND c.IsDeleted = 0 
UNION 
SELECT	cp.Id AS [CredentialPackageId],
		cp.TypeId,
		cp.[UserId] AS [OwnerUserId],
		cp.CreatedAt AS [PublishedAt],
		DATEPART(YEAR,cp.CreatedAt) AS [PublishedAtYear],
		c.PublisherName,
		cp.AssertionsCount, 
		0 AS [CredentialsCount],	
		c.[Json]
FROM cred.[CredentialPackages] cp (NOLOCK)
JOIN cred.[Clrs] c (NOLOCK) ON cp.Id = c.ParentCredentialPackageId
WHERE cp.IsDeleted = 0
AND c.IsDeleted = 0;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW [cred].[SearchCredentialPackagesView]");
        }
    }
}
