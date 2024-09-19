using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenCredentialPublisher.Data.Custom.Migrations
{
    /// <inheritdoc />
    public partial class SearchCredentialViewUpdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS [cred2].[SearchCredentialPackageView]");
            migrationBuilder.Sql("DROP VIEW IF EXISTS [cred2].[SearchCredentialCollectionView]");

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
	                                    SELECT svc.[VerifiableCredentialId]
		                                    ,COUNT(*) AS [ShareCount]
	                                    FROM [cred2].[ShareVerifiableCredential] svc(NOLOCK)
	                                    WHERE svc.IsDeleted = 0 
	                                    GROUP BY svc.[VerifiableCredentialId]
                                    ) AS directShares ON vc.[VerifiableCredentialId] = directShares.[VerifiableCredentialId]
                                    LEFT JOIN 
                                    (
	                                    SELECT ccvc.[VerifiableCredentialId]
		                                    ,COUNT(*) AS [ShareCount]
	                                    FROM [cred2].[ShareCredentialCollection] scc(NOLOCK)
	                                    JOIN [cred2].[CredentialCollectionVerifiableCredential] ccvc(NOLOCK) ON scc.[CredentialCollectionId] = ccvc.[CredentialCollectionId]
	                                    WHERE scc.IsDeleted = 0 AND ccvc.IsDeleted = 0
	                                    GROUP BY ccvc.[VerifiableCredentialId]
                                    ) AS collectionShares ON vc.[VerifiableCredentialId] = collectionShares.[VerifiableCredentialId]
                                    WHERE cp.IsDeleted = 0
	                                    AND vc.IsDeleted = 0
	                                    AND p.IsDeleted = 0;
");

            migrationBuilder.Sql(@"CREATE VIEW [cred2].[SearchCredentialCollectionView]
                                AS
                                SELECT cc.[CredentialCollectionId]
	                                ,cc.[UserId] AS [OwnerUserId]
	                                ,cc.[Name]
	                                ,cc.[Description]
	                                ,COALESCE(scc.[ShareCount], 0) AS [ShareCount]
	                                ,cc.[CreatedAt]
                                FROM [cred2].[CredentialCollection] cc(NOLOCK)
                                LEFT JOIN 
                                (
	                                SELECT [CredentialCollectionId], COUNT(*) AS [ShareCount]
	                                FROM [cred2].[ShareCredentialCollection] sccc (NOLOCK)
	                                WHERE sccc.IsDeleted = 0
	                                GROUP BY [CredentialCollectionId]
                                ) scc ON cc.[CredentialCollectionId] = scc.[CredentialCollectionId]
                                WHERE cc.IsDeleted = 0;");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW [cred2].[SearchCredentialPackageView]");
            migrationBuilder.Sql("DROP VIEW [cred2].[SearchCredentialCollectionView]");

        }
    }
}
