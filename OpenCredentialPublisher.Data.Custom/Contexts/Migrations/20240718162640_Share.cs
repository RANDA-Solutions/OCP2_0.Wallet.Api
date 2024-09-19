using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenCredentialPublisher.Data.Custom.Migrations
{
    /// <inheritdoc />
    public partial class Share : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.CreateTable(
                name: "Share",
                schema: "cred2",
                columns: table => new
                {
                    ShareId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShareSecureHash = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "0"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "0"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "0"),
                    AccessCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, defaultValue: "0"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Share", x => x.ShareId);
                    table.ForeignKey(
                        name: "FK_Share_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShareCredentialCollection",
                schema: "cred2",
                columns: table => new
                {
                    ShareId = table.Column<long>(type: "bigint", nullable: false),
                    CredentialCollectionId = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareCredentialCollection", x => new { x.ShareId, x.CredentialCollectionId });
                    table.ForeignKey(
                        name: "FK_ShareCredentialCollection_CredentialCollection_CredentialCollectionId",
                        column: x => x.CredentialCollectionId,
                        principalSchema: "cred2",
                        principalTable: "CredentialCollection",
                        principalColumn: "CredentialCollectionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShareCredentialCollection_Share_ShareId",
                        column: x => x.ShareId,
                        principalSchema: "cred2",
                        principalTable: "Share",
                        principalColumn: "ShareId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShareVerifiableCredential",
                schema: "cred2",
                columns: table => new
                {
                    ShareId = table.Column<long>(type: "bigint", nullable: false),
                    VerifiableCredentialId = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareVerifiableCredential", x => new { x.ShareId, x.VerifiableCredentialId });
                    table.ForeignKey(
                        name: "FK_ShareVerifiableCredential_Share_ShareId",
                        column: x => x.ShareId,
                        principalSchema: "cred2",
                        principalTable: "Share",
                        principalColumn: "ShareId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShareVerifiableCredential_VerifiableCredential_VerifiableCredentialId",
                        column: x => x.VerifiableCredentialId,
                        principalSchema: "cred2",
                        principalTable: "VerifiableCredential",
                        principalColumn: "VerifiableCredentialId",
                        onDelete: ReferentialAction.Restrict);
                });


            migrationBuilder.CreateIndex(
                name: "IX_Share_UserId",
                schema: "cred2",
                table: "Share",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShareCredentialCollection_CredentialCollectionId",
                schema: "cred2",
                table: "ShareCredentialCollection",
                column: "CredentialCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ShareVerifiableCredential_VerifiableCredentialId",
                schema: "cred2",
                table: "ShareVerifiableCredential",
                column: "VerifiableCredentialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShareCredentialCollection",
                schema: "cred2");

            migrationBuilder.DropTable(
                name: "ShareVerifiableCredential",
                schema: "cred2");

            migrationBuilder.DropTable(
                name: "Share",
                schema: "cred2");
        }
    }
}
