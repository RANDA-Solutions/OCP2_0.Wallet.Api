using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenCredentialPublisher.Data.Custom.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAltKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_VerifiableCredential_Id",
                schema: "cred2",
                table: "VerifiableCredential");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Profile_Id",
                schema: "cred2",
                table: "Profile");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CredentialPackage_Id",
                schema: "cred2",
                table: "CredentialPackage");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Achievement_Id",
                schema: "cred2",
                table: "Achievement");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_VerifiableCredential_Id",
                schema: "cred2",
                table: "VerifiableCredential",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Profile_Id",
                schema: "cred2",
                table: "Profile",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CredentialPackage_Id",
                schema: "cred2",
                table: "CredentialPackage",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Achievement_Id",
                schema: "cred2",
                table: "Achievement",
                column: "Id");
        }
    }
}
