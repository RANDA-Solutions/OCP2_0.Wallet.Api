using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenCredentialPublisher.Data.Custom.Migrations
{
    /// <inheritdoc />
    public partial class V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cred2");

            migrationBuilder.CreateTable(
                name: "Profile",
                schema: "cred2",
                columns: table => new
                {
                    ProfileId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTimestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2024, 7, 12, 17, 36, 47, 446, DateTimeKind.Unspecified).AddTicks(1246), new TimeSpan(0, 0, 0, 0, 0))),
                    ModifiedTimestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2024, 7, 12, 17, 36, 47, 446, DateTimeKind.Unspecified).AddTicks(1625), new TimeSpan(0, 0, 0, 0, 0)))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profile", x => x.ProfileId);
                    table.UniqueConstraint("AK_Profile_Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CredentialPackage",
                schema: "cred2",
                columns: table => new
                {
                    CredentialPackageId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AwardedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IssuanceDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ExpiresDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IssuerProfileId = table.Column<long>(type: "bigint", nullable: false),
                    CreateTimestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2024, 7, 12, 17, 36, 47, 445, DateTimeKind.Unspecified).AddTicks(4432), new TimeSpan(0, 0, 0, 0, 0))),
                    ModifiedTimestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2024, 7, 12, 17, 36, 47, 445, DateTimeKind.Unspecified).AddTicks(4767), new TimeSpan(0, 0, 0, 0, 0))),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CredentialPackage", x => x.CredentialPackageId);
                    table.UniqueConstraint("AK_CredentialPackage_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CredentialPackage_Profile_IssuerProfileId",
                        column: x => x.IssuerProfileId,
                        principalSchema: "cred2",
                        principalTable: "Profile",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VerifiableCredential",
                schema: "cred2",
                columns: table => new
                {
                    VerifiableCredentialId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuanceDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreateTimestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2024, 7, 12, 17, 36, 47, 446, DateTimeKind.Unspecified).AddTicks(3707), new TimeSpan(0, 0, 0, 0, 0))),
                    ModifiedTimestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2024, 7, 12, 17, 36, 47, 446, DateTimeKind.Unspecified).AddTicks(4032), new TimeSpan(0, 0, 0, 0, 0))),
                    Json = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CredentialPackageId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerifiableCredential", x => x.VerifiableCredentialId);
                    table.UniqueConstraint("AK_VerifiableCredential_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VerifiableCredential_CredentialPackage_CredentialPackageId",
                        column: x => x.CredentialPackageId,
                        principalSchema: "cred2",
                        principalTable: "CredentialPackage",
                        principalColumn: "CredentialPackageId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CredentialPackage_IssuerProfileId",
                schema: "cred2",
                table: "CredentialPackage",
                column: "IssuerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_VerifiableCredential_CredentialPackageId",
                schema: "cred2",
                table: "VerifiableCredential",
                column: "CredentialPackageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VerifiableCredential",
                schema: "cred2");

            migrationBuilder.DropTable(
                name: "CredentialPackage",
                schema: "cred2");

            migrationBuilder.DropTable(
                name: "Profile",
                schema: "cred2");
        }
    }
}
