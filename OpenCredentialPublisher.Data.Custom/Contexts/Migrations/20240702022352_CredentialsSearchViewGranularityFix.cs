using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenCredentialPublisher.Data.Custom.Migrations
{
    /// <inheritdoc />
    public partial class CredentialsSearchViewGranularityFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
ALTER VIEW [cred].[SearchCredentialPackagesView]
AS
SELECT	cp.Id AS [CredentialPackageId],
		cp.TypeId,
		cp.[UserId] AS [OwnerUserId],
		cp.CreatedAt,
		cp.AssertionsCount, 
		vc.CredentialsCount AS [CredentialsCount],
		vc.[Json]
FROM cred.[CredentialPackages] cp (NOLOCK)
JOIN cred.[VerifiableCredentials] vc (NOLOCK) ON cp.Id = vc.ParentCredentialPackageId
WHERE cp.IsDeleted = 0
AND vc.IsDeleted = 0
UNION
SELECT	cp.Id AS [CredentialPackageId], 
		cp.TypeId,
		cp.[UserId] AS [OwnerUserId],
		cp.CreatedAt,
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
		cp.CreatedAt,
		cp.AssertionsCount, 
		0 AS [CredentialsCount],	
		c.[Json]
FROM cred.[CredentialPackages] cp (NOLOCK)
JOIN cred.[Clrs] c (NOLOCK) ON cp.Id = c.ParentCredentialPackageId
WHERE cp.IsDeleted = 0
AND c.IsDeleted = 0;");

            migrationBuilder.Sql(@"
CREATE VIEW [cred].[SearchCredentialPackageIssuersView]
AS
SELECT	cp.Id AS [CredentialPackageId],
		vc.IssuedOn,
		DATEPART(YEAR,vc.IssuedOn) AS [IssuedOnYear],
		N'' AS IssuerName
FROM cred.[CredentialPackages] cp (NOLOCK)
JOIN cred.[VerifiableCredentials] vc (NOLOCK) ON cp.Id = vc.ParentCredentialPackageId
UNION 
SELECT	cp.Id AS [CredentialPackageId], 
		c.IssuedOn,
		DATEPART(YEAR,c.IssuedOn) AS [IssuedOnYear],
		c.PublisherName AS [IssuerName]
FROM cred.[CredentialPackages] cp (NOLOCK)
JOIN cred.[ClrSets] cs (NOLOCK) ON cp.Id = cs.ParentCredentialPackageId
JOIN cred.[Clrs] c (NOLOCK) ON cs.Id = c.ParentClrSetId
WHERE cp.IsDeleted = 0
AND cs.IsDeleted = 0
AND c.IsDeleted = 0
UNION 
SELECT	cp.Id AS [CredentialPackageId],
		c.IssuedOn,
		DATEPART(YEAR,c.IssuedOn) AS [IssuedOnYear],
		c.PublisherName AS [IssuerName]
FROM cred.[CredentialPackages] cp (NOLOCK)
JOIN cred.[Clrs] c (NOLOCK) ON cp.Id = c.ParentCredentialPackageId
WHERE cp.IsDeleted = 0
AND c.IsDeleted = 0;");

            migrationBuilder.Sql(@"
CREATE VIEW [cred].[SearchCredentialPackageCredentialTypesView]
AS
SELECT	cp.Id AS [CredentialPackageId],
		N'VC Unknown' AS [CredentialType]
FROM cred.[CredentialPackages] cp (NOLOCK)
JOIN cred.[VerifiableCredentials] vc (NOLOCK) ON cp.Id = vc.ParentCredentialPackageId
UNION 
SELECT	cp.Id AS [CredentialPackageId],
		ISNULL(a.AchievementType,N'CS Unknown') AS [CredentialType]
FROM cred.[CredentialPackages] cp (NOLOCK)
JOIN cred.[ClrSets] cs (NOLOCK) ON cp.Id = cs.ParentCredentialPackageId
JOIN cred.[Clrs] c (NOLOCK) ON cs.Id = c.ParentClrSetId
LEFT JOIN cred.[ClrAchievements] ca (NOLOCK) ON c.ClrId = ca.ClrId AND ca.IsDeleted=0
LEFT JOIN cred.[Achievements] a (NOLOCK) ON ca.AchievementId=a.AchievementId AND a.IsDeleted=0
WHERE cp.IsDeleted = 0
AND cs.IsDeleted = 0
AND c.IsDeleted = 0
UNION 
SELECT	cp.Id AS [CredentialPackageId],
		ISNULL(a.AchievementType,N'C Unknown') AS [CredentialType]
FROM cred.[CredentialPackages] cp (NOLOCK)
JOIN cred.[Clrs] c (NOLOCK) ON cp.Id = c.ParentCredentialPackageId
LEFT JOIN cred.[ClrAchievements] ca (NOLOCK) ON c.ClrId = ca.ClrId AND ca.IsDeleted=0
LEFT JOIN cred.[Achievements] a (NOLOCK) ON ca.AchievementId=a.AchievementId AND a.IsDeleted=0
WHERE cp.IsDeleted = 0
AND c.IsDeleted = 0;");


        }



        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
ALTER VIEW [cred].[SearchCredentialPackagesView]
AS
SELECT	cp.Id AS [CredentialPackageId],
		cp.TypeId,
		cp.[UserId] AS [OwnerUserId],
		cp.CreatedAt AS [PublishedAt],
		DATEPART(YEAR,cp.CreatedAt) AS [PublishedAtYear],
		c.PublisherName,
		a.AchievementType,
		cp.AssertionsCount, 
		vc.CredentialsCount AS [CredentialsCount],
		vc.[Json]
FROM cred.[CredentialPackages] cp (NOLOCK)
JOIN cred.[VerifiableCredentials] vc (NOLOCK) ON cp.Id = vc.ParentCredentialPackageId
JOIN cred.[ClrSets] cs (NOLOCK) ON vc.Id = cs.ParentVerifiableCredentialId
JOIN cred.[Clrs] c (NOLOCK) ON cs.Id = c.ParentClrSetId
JOIN cred.[ClrAchievements] ca (NOLOCK) ON c.ClrId = ca.ClrId
JOIN cred.[Achievements] a (NOLOCK) ON ca.AchievementId=a.AchievementId
WHERE cp.IsDeleted = 0
AND vc.IsDeleted = 0
AND c.IsDeleted = 0
AND ca.IsDeleted = 0
AND a.IsDeleted = 0
UNION
SELECT	cp.Id AS [CredentialPackageId],
		cp.TypeId,
		cp.[UserId] AS [OwnerUserId],
		cp.CreatedAt AS [PublishedAt],
		DATEPART(YEAR,cp.CreatedAt) AS [PublishedAtYear],
		c.PublisherName,
		a.AchievementType,
		cp.AssertionsCount, 
		vc.CredentialsCount AS [CredentialsCount],
		vc.[Json]
FROM cred.[CredentialPackages] cp (NOLOCK)
JOIN cred.[VerifiableCredentials] vc (NOLOCK) ON cp.Id = vc.ParentCredentialPackageId
JOIN cred.[Clrs] c (NOLOCK) ON vc.Id = c.ParentVerifiableCredentialId
JOIN cred.[ClrAchievements] ca (NOLOCK) ON c.ClrId = ca.ClrId
JOIN cred.[Achievements] a (NOLOCK) ON ca.AchievementId=a.AchievementId
WHERE cp.IsDeleted = 0
AND vc.IsDeleted = 0
AND c.IsDeleted = 0
AND ca.IsDeleted = 0
AND a.IsDeleted = 0
UNION
SELECT	cp.Id AS [CredentialPackageId], 
		cp.TypeId,
		cp.[UserId] AS [OwnerUserId],
		cp.CreatedAt AS [PublishedAt],
		DATEPART(YEAR,cp.CreatedAt) AS [PublishedAtYear],
		c.PublisherName,
		a.AchievementType,
		cp.AssertionsCount, 
		0 AS [CredentialsCount],
		cs.[Json]
FROM cred.[CredentialPackages] cp (NOLOCK)
JOIN cred.[ClrSets] cs (NOLOCK) ON cp.Id = cs.ParentCredentialPackageId
JOIN cred.[Clrs] c (NOLOCK) ON cs.Id = c.ParentClrSetId
JOIN cred.[ClrAchievements] ca (NOLOCK) ON c.ClrId = ca.ClrId
JOIN cred.[Achievements] a (NOLOCK) ON ca.AchievementId=a.AchievementId
WHERE cp.IsDeleted = 0
AND cs.IsDeleted = 0
AND c.IsDeleted = 0
AND ca.IsDeleted = 0
AND a.IsDeleted = 0
UNION 
SELECT	cp.Id AS [CredentialPackageId],
		cp.TypeId,
		cp.[UserId] AS [OwnerUserId],
		cp.CreatedAt AS [PublishedAt],
		DATEPART(YEAR,cp.CreatedAt) AS [PublishedAtYear],
		c.PublisherName,
		a.AchievementType,
		cp.AssertionsCount, 
		0 AS [CredentialsCount],	
		c.[Json]
FROM cred.[CredentialPackages] cp (NOLOCK)
JOIN cred.[Clrs] c (NOLOCK) ON cp.Id = c.ParentCredentialPackageId
JOIN cred.[ClrAchievements] ca (NOLOCK) ON c.ClrId = ca.ClrId
JOIN cred.[Achievements] a (NOLOCK) ON ca.AchievementId=a.AchievementId
WHERE cp.IsDeleted = 0
AND c.IsDeleted = 0
AND ca.IsDeleted = 0
AND a.IsDeleted = 0;");

            migrationBuilder.Sql("DROP VIEW [cred].[SearchCredentialPackageIssuersView]");
            migrationBuilder.Sql("DROP VIEW [cred].[SearchCredentialPackageCredentialTypesView]");
        }
    }
}
