using System;

namespace OpenCredentialPublisher.Data.Custom.Contexts.SqlObjects.Views
{
    /*
    In your specific version of the migration for the up/down you would have:

        protected override void Up(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.Sql($"exec('{SearchCredentialPackageView.GetDropSql().Replace("'", "''")}')");
           migrationBuilder.Sql($"exec('{SearchCredentialPackageView.GetCreateSql(SearchCredentialPackageView.VersionNumber.V20240731134030).Replace("'", "''")}')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.Sql($"exec('{SearchCredentialPackageView.GetDropSql().Replace("'", "''")}')");
           migrationBuilder.Sql($"exec('{SearchCredentialPackageView.GetCreateSqlPrevious(SearchCredentialPackageView.VersionNumber.V20240731134030).Replace("'", "''")}')");
        }
    */

    public static class SearchCredentialPackageView
    {
        // View version numbers
        public enum VersionNumber
        {
            V20240731134030,
            V20240813193820
        }

        private static readonly string ViewName = $"{EntityTypeConfigurationConstants.SCHEMA_V2}.{nameof(SearchCredentialPackageView)}";

        //
        public static string GetCreateSqlPrevious(VersionNumber versionNumber)
        {
            return versionNumber switch
            {
                VersionNumber.V20240731134030 => "",
                VersionNumber.V20240813193820 => V20240731134030,
                _ => throw new ApplicationException("Unknown Version Number")
            };
        }

        // Get the view create statement for the version number
        public static string GetCreateSql(VersionNumber versionNumber) =>
            versionNumber switch
            {
                VersionNumber.V20240731134030 => "",
                VersionNumber.V20240813193820 => V20240813193820,
                _ => throw new ApplicationException("Unknown Version Number")
            };

        public static string GetDropSql() => $"DROP VIEW IF EXISTS {ViewName}";

        private static string V20240813193820 => $@"
            CREATE VIEW {ViewName}
            AS
            SELECT cp.[CredentialPackageId]
	            ,cp.[Name]
	            ,COALESCE(p.[Name], p.[Id]) AS [IssuerName]
	            ,COALESCE(vc.[ImageUrl],p.[ImageUrl]) AS [EffectiveImageUrl]
	            ,cp.[UserId] AS [OwnerUserId]
	            ,COALESCE(ac.AchievementCount,0) AS [AchievementCount]
	            ,COALESCE(directShares.[ShareCount], 0) + COALESCE(collectionShares.[ShareCount], 0) AS [ShareCount]
	            ,COALESCE(vc.[AwardedDate], vc.[ValidFromDate]) AS [EffectiveAt]
	            ,vc.[ValidUntilDate] AS [ExpiresAt]
	            ,cp.[CreatedAt]
	            ,vc.[IsVerified]
                ,vc.[IsRevoked]
                ,vc.[RevokedReason]
	            ,vc.[Json]
                ,vc.[VerifiableCredentialId]
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
            ";

        private static string V20240731134030 => $@"
            CREATE VIEW {ViewName}
            AS
            SELECT cp.[CredentialPackageId]
	            ,cp.[Name]
	            ,COALESCE(p.[Name], p.[Id]) AS [IssuerName]
	            ,COALESCE(vc.[ImageUrl],p.[ImageUrl]) AS [EffectiveImageUrl]
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
            ";
    }
}
