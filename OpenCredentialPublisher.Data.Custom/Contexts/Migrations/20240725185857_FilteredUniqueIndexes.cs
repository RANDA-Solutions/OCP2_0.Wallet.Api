using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenCredentialPublisher.Data.Custom.Migrations
{
    /// <inheritdoc />
    public partial class FilteredUniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create the filtered unique indexes
            migrationBuilder.Sql(
                @"CREATE UNIQUE INDEX UIX_Achievement_Id_IsDeleted_0 
                ON [cred2].[Achievement] (Id)
                WHERE IsDeleted = 0");

            migrationBuilder.Sql(
                @"CREATE UNIQUE INDEX UIX_CredentialPackage_Id_IsDeleted_0 
                ON [cred2].[CredentialPackage] (Id) 
                WHERE IsDeleted = 0");

            migrationBuilder.Sql(
                @"CREATE UNIQUE INDEX UIX_Profile_Id_IsDeleted_0 
                  ON [cred2].[Profile] (Id)
                  WHERE IsDeleted = 0");

            migrationBuilder.Sql(
                @"CREATE UNIQUE INDEX UIX_VerifiableCredential_Id_IsDeleted_0
                  ON [cred2].[VerifiableCredential] (Id)
                  WHERE IsDeleted = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the filtered unique indexes
            migrationBuilder.Sql("DROP INDEX IF EXISTS UIX_Achievement_Id_IsDeleted_0 ON [cred2].[Achievement]");
            migrationBuilder.Sql("DROP INDEX IF EXISTS UIX_CredentialPackage_Id_IsDeleted_0 ON [cred2].[CredentialPackage]");
            migrationBuilder.Sql("DROP INDEX IF EXISTS UIX_Profile_Id_IsDeleted_0 ON [cred2].[Profile]");
            migrationBuilder.Sql("DROP INDEX IF EXISTS UIX_VerifiableCredential_Id_IsDeleted_0 ON [cred2].[VerifiableCredential]");
        }
    }
}
