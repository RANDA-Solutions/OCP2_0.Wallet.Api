using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenCredentialPublisher.Data.Custom.Migrations
{
    /// <inheritdoc />
    public partial class VcIsChild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsChild",
                schema: "cred2",
                table: "VerifiableCredential",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsChild",
                schema: "cred2",
                table: "VerifiableCredential");
        }
    }
}
