using System;

namespace OpenCredentialPublisher.Data.Custom.Contexts.SqlObjects.Views
{

    /*
    In your specific version of the migration for the up/down you would have:

        protected override void Up(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.Sql($"exec('{SearchCredentialPackageIssuerView.GetDropSql().Replace("'", "''")}')");
           migrationBuilder.Sql($"exec('{SearchCredentialPackageIssuerView.GetCreateSql(SearchCredentialPackageIssuerView.VersionNumber.V20240731134030).Replace("'", "''")}')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.Sql($"exec('{SearchCredentialPackageIssuerView.GetDropSql().Replace("'", "''")}')");
           migrationBuilder.Sql($"exec('{SearchCredentialPackageIssuerView.GetCreateSqlPrevious(SearchCredentialPackageIssuerView.VersionNumber.V20240731134030).Replace("'", "''")}')");
        }
    */

    public static class SearchCredentialPackageIssuerView
    {
        // View version numbers
        public enum VersionNumber
        {
            V20240731134030
        }

        private static readonly string ViewName = $"{EntityTypeConfigurationConstants.SCHEMA_V2}.{nameof(SearchCredentialPackageIssuerView)}";

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
	        SELECT DISTINCT
		        cp.[CredentialPackageId],
		        COALESCE(p.[Name], p.[Id]) AS[IssuerName],
		        COALESCE(vc.[ValidFromDate], vc.[AwardedDate]) AS[EffectiveAt],
		        DATEPART(YEAR, COALESCE(vc.[ValidFromDate], vc.[AwardedDate])) AS[EffectiveAtYear]
	        FROM[cred2].[CredentialPackage] cp(NOLOCK)
	        JOIN[cred2].[VerifiableCredential] vc(NOLOCK) ON cp.CredentialPackageId = vc.CredentialPackageId
	        JOIN[cred2].[Profile] p(NOLOCK) ON vc.IssuerProfileId = p.ProfileId
	        WHERE cp.IsDeleted = 0
	        AND vc.IsDeleted = 0
	        AND p.IsDeleted = 0;
        ";
    }
}
