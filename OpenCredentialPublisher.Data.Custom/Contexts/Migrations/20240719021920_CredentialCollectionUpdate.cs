using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenCredentialPublisher.Data.Custom.Migrations
{
    /// <inheritdoc />
    public partial class CredentialCollectionUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "cred2",
                table: "CredentialCollection",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "cred2",
                table: "CredentialCollection",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.Sql(@"CREATE VIEW [cred2].[SearchCredentialCollectionView]
AS
SELECT cc.[CredentialCollectionId],
	cc.[UserId] AS [OwnerUserId], 
	cc.[Name],
	cc.[Description],
	CAST(0 AS INT) AS [ShareCount],
	cc.[CreatedAt]
FROM [cred2].[CredentialCollection] cc (NOLOCK) 
WHERE cc.IsDeleted=0;");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "cred2",
                table: "CredentialCollection");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "cred2",
                table: "CredentialCollection",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
