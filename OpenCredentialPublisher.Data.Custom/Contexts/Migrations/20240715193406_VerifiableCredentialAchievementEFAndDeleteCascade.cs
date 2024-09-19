using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenCredentialPublisher.Data.Custom.Migrations
{
    /// <inheritdoc />
    public partial class VerifiableCredentialAchievementEFAndDeleteCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Achievement_VerifiableCredentialId",
                schema: "cred2",
                table: "Achievement");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "cred2",
                table: "CredentialCollection",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Achievement_VerifiableCredentialId",
                schema: "cred2",
                table: "Achievement",
                column: "VerifiableCredentialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Achievement_VerifiableCredentialId",
                schema: "cred2",
                table: "Achievement");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "cred2",
                table: "CredentialCollection",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.CreateIndex(
                name: "IX_Achievement_VerifiableCredentialId",
                schema: "cred2",
                table: "Achievement",
                column: "VerifiableCredentialId",
                unique: true);
        }
    }
}
