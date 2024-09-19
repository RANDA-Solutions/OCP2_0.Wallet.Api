using System;

namespace OpenCredentialPublisher.Data.Custom.Contexts.SqlObjects.Views
{
    /*
    In your specific version of the migration for the up/down you would have:

        protected override void Up(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.Sql($"exec('{SearchCredentialView.GetDropSql().Replace("'", "''")}')");
           migrationBuilder.Sql($"exec('{SearchCredentialView.GetCreateSql(SearchCredentialView.VersionNumber.V20240731134030).Replace("'", "''")}')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.Sql($"exec('{SearchCredentialView.GetDropSql().Replace("'", "''")}')");
           migrationBuilder.Sql($"exec('{SearchCredentialView.GetCreateSqlPrevious(SearchCredentialView.VersionNumber.V20240731134030).Replace("'", "''")}')");
        }
    */
    public static class SearchCredentialView
    {
        // View version numbers
        public enum VersionNumber
        {
            V20240731134030
        }

        private static readonly string ViewName = $"{EntityTypeConfigurationConstants.SCHEMA_V2}.{nameof(SearchCredentialView)}";

        public static string GetCreateSqlPrevious(VersionNumber versionNumber)
        {
            return versionNumber switch
            {
                VersionNumber.V20240731134030 => "",
                _ => throw new ApplicationException("Unknown Version Number")
            };
        }


        // Get the view create statement for the version number
        public static string GetCreateSql(VersionNumber versionNumber) =>
            versionNumber switch
            {
                VersionNumber.V20240731134030 => V20240731134030,
                _ => throw new ApplicationException("Unknown Version Number")
            };

        public static string GetDropSql() => $"DROP VIEW IF EXISTS {ViewName}";

        private static string V20240731134030 => $@"
            CREATE VIEW {ViewName}
            AS
            SELECT 
	            cp.[CredentialPackageId],
	            vc.[VerifiableCredentialId],
	            ISNULL(a.[AchievementType],N'Unspecified') AS [AchievementType],
	            COALESCE(p.[Name],p.[Id]) AS [IssuerName],
                cp.[UserId] AS [OwnerUserId],
                COALESCE(vc.[AwardedDate],vc.[ValidFromDate]) AS [EffectiveAt],
                DATEPART(YEAR,COALESCE(vc.[AwardedDate],vc.[ValidFromDate])) AS [EffectiveAtYear],
                cp.[CreatedAt],
	            vc.[Json]
            FROM [cred2].[VerifiableCredential] vc (NOLOCK) 
            JOIN [cred2].[CredentialPackage] cp (NOLOCK) ON cp.CredentialPackageId=vc.CredentialPackageId AND vc.ParentVerifiableCredentialId IS NOT NULL
            JOIN [cred2].[Profile] p (NOLOCK) ON vc.IssuerProfileId=p.ProfileId
            LEFT JOIN [cred2].[Achievement] a (NOLOCK) ON vc.VerifiableCredentialId = a.VerifiableCredentialId
            WHERE cp.IsDeleted=0
            AND vc.IsDeleted=0
            AND p.IsDeleted=0;
            ";
    }
}
