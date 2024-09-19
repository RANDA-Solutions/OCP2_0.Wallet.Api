using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenCredentialPublisher.Data.Custom.Migrations
{
    /// <inheritdoc />
    public partial class AchievementParentVC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedTimestamp",
                schema: "cred2",
                table: "VerifiableCredential",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2024, 7, 12, 18, 43, 3, 633, DateTimeKind.Unspecified).AddTicks(2115), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTimestamp",
                schema: "cred2",
                table: "VerifiableCredential",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2024, 7, 12, 18, 43, 3, 633, DateTimeKind.Unspecified).AddTicks(1797), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<long>(
                name: "ParentVerifiableCredentialId",
                schema: "cred2",
                table: "VerifiableCredential",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedTimestamp",
                schema: "cred2",
                table: "Profile",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2024, 7, 12, 18, 43, 3, 632, DateTimeKind.Unspecified).AddTicks(9719), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTimestamp",
                schema: "cred2",
                table: "Profile",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2024, 7, 12, 18, 43, 3, 632, DateTimeKind.Unspecified).AddTicks(9417), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedTimestamp",
                schema: "cred2",
                table: "CredentialPackage",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2024, 7, 12, 18, 43, 3, 632, DateTimeKind.Unspecified).AddTicks(3446), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTimestamp",
                schema: "cred2",
                table: "CredentialPackage",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2024, 7, 12, 18, 43, 3, 632, DateTimeKind.Unspecified).AddTicks(2956), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateTable(
                name: "Achievement",
                schema: "cred2",
                columns: table => new
                {
                    AchievementId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AchievementType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FieldOfStudy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenseNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorProfileId = table.Column<long>(type: "bigint", nullable: false),
                    SourceProfileId = table.Column<long>(type: "bigint", nullable: false),
                    VerifiableCredentialId = table.Column<long>(type: "bigint", nullable: false),
                    CreateTimestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedTimestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievement", x => x.AchievementId);
                    table.UniqueConstraint("AK_Achievement_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Achievement_Profile_CreatorProfileId",
                        column: x => x.CreatorProfileId,
                        principalSchema: "cred2",
                        principalTable: "Profile",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Achievement_Profile_SourceProfileId",
                        column: x => x.SourceProfileId,
                        principalSchema: "cred2",
                        principalTable: "Profile",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Achievement_VerifiableCredential_VerifiableCredentialId",
                        column: x => x.VerifiableCredentialId,
                        principalSchema: "cred2",
                        principalTable: "VerifiableCredential",
                        principalColumn: "VerifiableCredentialId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VerifiableCredential_ParentVerifiableCredentialId",
                schema: "cred2",
                table: "VerifiableCredential",
                column: "ParentVerifiableCredentialId");

            migrationBuilder.CreateIndex(
                name: "IX_Achievement_CreatorProfileId",
                schema: "cred2",
                table: "Achievement",
                column: "CreatorProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Achievement_SourceProfileId",
                schema: "cred2",
                table: "Achievement",
                column: "SourceProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Achievement_VerifiableCredentialId",
                schema: "cred2",
                table: "Achievement",
                column: "VerifiableCredentialId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VerifiableCredential_VerifiableCredential_ParentVerifiableCredentialId",
                schema: "cred2",
                table: "VerifiableCredential",
                column: "ParentVerifiableCredentialId",
                principalSchema: "cred2",
                principalTable: "VerifiableCredential",
                principalColumn: "VerifiableCredentialId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VerifiableCredential_VerifiableCredential_ParentVerifiableCredentialId",
                schema: "cred2",
                table: "VerifiableCredential");

            migrationBuilder.DropTable(
                name: "Achievement",
                schema: "cred2");

            migrationBuilder.DropIndex(
                name: "IX_VerifiableCredential_ParentVerifiableCredentialId",
                schema: "cred2",
                table: "VerifiableCredential");

            migrationBuilder.DropColumn(
                name: "ParentVerifiableCredentialId",
                schema: "cred2",
                table: "VerifiableCredential");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedTimestamp",
                schema: "cred2",
                table: "VerifiableCredential",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2024, 7, 12, 18, 43, 3, 633, DateTimeKind.Unspecified).AddTicks(2115), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTimestamp",
                schema: "cred2",
                table: "VerifiableCredential",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2024, 7, 12, 18, 43, 3, 633, DateTimeKind.Unspecified).AddTicks(1797), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedTimestamp",
                schema: "cred2",
                table: "Profile",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2024, 7, 12, 18, 43, 3, 632, DateTimeKind.Unspecified).AddTicks(9719), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTimestamp",
                schema: "cred2",
                table: "Profile",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2024, 7, 12, 18, 43, 3, 632, DateTimeKind.Unspecified).AddTicks(9417), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedTimestamp",
                schema: "cred2",
                table: "CredentialPackage",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2024, 7, 12, 18, 43, 3, 632, DateTimeKind.Unspecified).AddTicks(3446), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTimestamp",
                schema: "cred2",
                table: "CredentialPackage",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2024, 7, 12, 18, 43, 3, 632, DateTimeKind.Unspecified).AddTicks(2956), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValueSql: "GETUTCDATE()");
        }
    }
}
