using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenCredentialPublisher.Data.Custom.Migrations
{
    /// <inheritdoc />
    public partial class SearchCredentialPackageViewUpdate4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS [cred2].[SearchCredentialPackageView]");

            migrationBuilder.Sql(@"CREATE VIEW [cred2].[SearchCredentialPackageView]
AS
SELECT cp.[CredentialPackageId]
	,cp.[Name]
	,COALESCE(p.[Name], p.[Id]) AS [IssuerName]
	,p.[ImageUrl] AS [IssuerImageUrl]
	,cp.[UserId] AS [OwnerUserId]
	,COALESCE(ac.AchievementCount,0) AS [AchievementCount]
	,COALESCE(directShares.[ShareCount], 0) + COALESCE(collectionShares.[ShareCount], 0) AS [ShareCount]
	,COALESCE(vc.[AwardedDate], vc.[ValidFromDate]) AS [EffectiveAt]
	,vc.[ValidUntilDate] AS [ExpiresAt]
	,cp.[CreatedAt]
	,vc.[IsVerified]
	,vc.[Json]
FROM [cred2].[CredentialPackage] cp(NOLOCK)
JOIN [cred2].[VerifiableCredential] vc(NOLOCK) ON cp.CredentialPackageId = vc.CredentialPackageId
	AND vc.ParentVerifiableCredentialId IS NULL
JOIN [cred2].[Profile] p(NOLOCK) ON vc.IssuerProfileId = p.ProfileId
LEFT JOIN 
(
	SELECT vc.ParentVerifiableCredentialId
		,COUNT(*) AS [AchievementCount]
	FROM [cred2].[Achievement] a(NOLOCK)
	JOIN [cred2].[VerifiableCredential] vc (NOLOCK) ON a.VerifiableCredentialId = vc.VerifiableCredentialId
	WHERE a.IsDeleted = 0 AND vc.IsDeleted = 0 
	GROUP BY vc.ParentVerifiableCredentialId
) AS ac ON vc.[VerifiableCredentialId] = ac.[ParentVerifiableCredentialId]
LEFT JOIN 
(
	SELECT vc.ParentVerifiableCredentialId
		,COUNT(svc.ShareId) AS [ShareCount]
	FROM [cred2].[ShareVerifiableCredential] svc(NOLOCK)
	JOIN [cred2].[VerifiableCredential] vc (NOLOCK) ON svc.VerifiableCredentialId = vc.VerifiableCredentialId
	WHERE vc.IsDeleted = 0 
	AND svc.IsDeleted = 0 
	GROUP BY vc.ParentVerifiableCredentialId
) AS directShares ON vc.[VerifiableCredentialId] = directShares.[ParentVerifiableCredentialId]
LEFT JOIN 
(
	SELECT vc.ParentVerifiableCredentialId
		,COUNT(scc.ShareId) AS [ShareCount]
	FROM [cred2].[ShareCredentialCollection] scc(NOLOCK)
	JOIN [cred2].[CredentialCollectionVerifiableCredential] ccvc(NOLOCK) ON scc.[CredentialCollectionId] = ccvc.[CredentialCollectionId]
	JOIN [cred2].[VerifiableCredential] vc (NOLOCK) ON ccvc.VerifiableCredentialId = vc.VerifiableCredentialId
	WHERE scc.IsDeleted = 0 
	AND ccvc.IsDeleted = 0
	AND vc.IsDeleted = 0 
	GROUP BY vc.ParentVerifiableCredentialId
) AS collectionShares ON vc.[VerifiableCredentialId] = collectionShares.[ParentVerifiableCredentialId]
WHERE cp.IsDeleted = 0
	AND vc.IsDeleted = 0
	AND p.IsDeleted = 0;
");

        }

        /// <inheritdoc />
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS [cred2].[SearchCredentialPackageView]");

            migrationBuilder.Sql(@"CREATE VIEW [cred2].[SearchCredentialPackageView]
                                    AS
                                    
SELECT cp.[CredentialPackageId]
	,cp.[Name]
	,COALESCE(p.[Name], p.[Id]) AS [IssuerName]
	,p.[ImageUrl] AS [IssuerImageUrl]
	,cp.[UserId] AS [OwnerUserId]
	,COALESCE(ac.AchievementCount,0) AS [AchievementCount]
	,COALESCE(directShares.[ShareCount], 0) + COALESCE(collectionShares.[ShareCount], 0) AS [ShareCount]
	,COALESCE(vc.[AwardedDate], vc.[ValidFromDate]) AS [EffectiveAt]
	,vc.[ValidUntilDate] AS [ExpiresAt]
	,cp.[CreatedAt]
	,vc.[Json]
FROM [cred2].[CredentialPackage] cp(NOLOCK)
JOIN [cred2].[VerifiableCredential] vc(NOLOCK) ON cp.CredentialPackageId = vc.CredentialPackageId
	AND vc.ParentVerifiableCredentialId IS NULL
JOIN [cred2].[Profile] p(NOLOCK) ON vc.IssuerProfileId = p.ProfileId
LEFT JOIN 
(
	SELECT vc.ParentVerifiableCredentialId
		,COUNT(*) AS [AchievementCount]
	FROM [cred2].[Achievement] a(NOLOCK)
	JOIN [cred2].[VerifiableCredential] vc (NOLOCK) ON a.VerifiableCredentialId = vc.VerifiableCredentialId
	WHERE a.IsDeleted = 0 AND vc.IsDeleted = 0 
	GROUP BY vc.ParentVerifiableCredentialId
) AS ac ON vc.[VerifiableCredentialId] = ac.[ParentVerifiableCredentialId]
LEFT JOIN 
(
	SELECT vc.ParentVerifiableCredentialId
		,COUNT(svc.ShareId) AS [ShareCount]
	FROM [cred2].[ShareVerifiableCredential] svc(NOLOCK)
	JOIN [cred2].[VerifiableCredential] vc (NOLOCK) ON svc.VerifiableCredentialId = vc.VerifiableCredentialId
	WHERE vc.IsDeleted = 0 
	AND svc.IsDeleted = 0 
	GROUP BY vc.ParentVerifiableCredentialId
) AS directShares ON vc.[VerifiableCredentialId] = directShares.[ParentVerifiableCredentialId]
LEFT JOIN 
(
	SELECT vc.ParentVerifiableCredentialId
		,COUNT(scc.ShareId) AS [ShareCount]
	FROM [cred2].[ShareCredentialCollection] scc(NOLOCK)
	JOIN [cred2].[CredentialCollectionVerifiableCredential] ccvc(NOLOCK) ON scc.[CredentialCollectionId] = ccvc.[CredentialCollectionId]
	JOIN [cred2].[VerifiableCredential] vc (NOLOCK) ON ccvc.VerifiableCredentialId = vc.VerifiableCredentialId
	WHERE scc.IsDeleted = 0 
	AND ccvc.IsDeleted = 0
	AND vc.IsDeleted = 0 
	GROUP BY vc.ParentVerifiableCredentialId
) AS collectionShares ON vc.[VerifiableCredentialId] = collectionShares.[ParentVerifiableCredentialId]
WHERE cp.IsDeleted = 0
	AND vc.IsDeleted = 0
	AND p.IsDeleted = 0;
");

        }

    }
}
