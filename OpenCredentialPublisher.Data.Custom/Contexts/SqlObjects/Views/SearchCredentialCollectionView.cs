using System;

namespace OpenCredentialPublisher.Data.Custom.Contexts.SqlObjects.Views
{
    /* 
     In your specific verions of the migration for the up/down you would have:
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.Sql($"exec('{SearchCredentialCollectionView.GetDropSql().Replace("'", "''")}')");
           migrationBuilder.Sql($"exec('{SearchCredentialCollectionView.GetCreateSql(SearchCredentialCollectionView.VersionNumber.V20240731134030).Replace("'", "''")}')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.Sql($"exec('{SearchCredentialCollectionView.GetDropSql().Replace("'", "''")}')");
           migrationBuilder.Sql($"exec('{SearchCredentialCollectionView.GetCreateSqlPrevious(SearchCredentialCollectionView.VersionNumber.V20240731134030).Replace("'", "''")}')");
        }
    */ 
    public static class SearchCredentialCollectionView
    {
        // View version numbers
        public enum VersionNumber
        {
            V20240731134030
        }

        private static readonly string ViewName = $"{EntityTypeConfigurationConstants.SCHEMA_V2}.{nameof(SearchCredentialCollectionView)}";

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
            WHERE cc.IsDeleted = 0;
        ";
    }
}
