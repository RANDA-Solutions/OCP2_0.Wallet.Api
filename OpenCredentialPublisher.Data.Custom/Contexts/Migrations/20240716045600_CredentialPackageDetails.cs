using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenCredentialPublisher.Data.Custom.Migrations
{
    /// <inheritdoc />
    public partial class CredentialPackageDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Achievement_VerifiableCredentialId",
                schema: "cred2",
                table: "Achievement");

            migrationBuilder.CreateIndex(
                name: "IX_Achievement_VerifiableCredentialId",
                schema: "cred2",
                table: "Achievement",
                column: "VerifiableCredentialId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Achievement_VerifiableCredentialId",
                schema: "cred2",
                table: "Achievement");

            migrationBuilder.CreateIndex(
                name: "IX_Achievement_VerifiableCredentialId",
                schema: "cred2",
                table: "Achievement",
                column: "VerifiableCredentialId");
        }
    }
}
