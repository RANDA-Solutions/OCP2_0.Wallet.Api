using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenCredentialPublisher.Data.Custom.Migrations
{
    /// <inheritdoc />
    public partial class AddShareType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShareType",
                schema: "cred2",
                table: "Share",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShareType",
                schema: "cred2",
                table: "Share");
        }
    }
}
