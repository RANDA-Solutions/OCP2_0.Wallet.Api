using System;

namespace OpenCredentialPublisher.Data.Custom.Contexts.SqlObjects.Views
{
    /*
    In your specific version of the migration for the up/down you would have:

        protected override void Up(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.Sql($"exec('{SearchCredentialPackageAchievementTypeView.GetDropSql().Replace("'", "''")}')");
           migrationBuilder.Sql($"exec('{SearchCredentialPackageAchievementTypeView.GetCreateSql(SearchCredentialPackageAchievementTypeView.VersionNumber.V20240731134030).Replace("'", "''")}')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.Sql($"exec('{SearchCredentialPackageAchievementTypeView.GetDropSql().Replace("'", "''")}')");
           migrationBuilder.Sql($"exec('{SearchCredentialPackageAchievementTypeView.GetCreateSqlPrevious(SearchCredentialPackageAchievementTypeView.VersionNumber.V20240731134030).Replace("'", "''")}')");
        }
    */
    public static class SearchCredentialPackageAchievementTypeView
    {
        // View version numbers
        public enum VersionNumber
        {
            V20240731134030
        }

        private static readonly string ViewName = $"{EntityTypeConfigurationConstants.SCHEMA_V2}.{nameof(SearchCredentialPackageAchievementTypeView)}";

        //
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
            SELECT	DISTINCT
                    vc.[CredentialPackageId],
		            a.[AchievementType]
            FROM [cred2].[VerifiableCredential] vc (NOLOCK) 
            LEFT JOIN [cred2].[Achievement] a (NOLOCK) ON vc.VerifiableCredentialId=a.VerifiableCredentialId
            WHERE vc.IsDeleted=0
            AND a.IsDeleted=0;
        ";
    }
}
