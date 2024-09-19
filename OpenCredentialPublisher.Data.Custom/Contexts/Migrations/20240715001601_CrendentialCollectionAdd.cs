using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenCredentialPublisher.Data.Custom.Migrations
{
    /// <inheritdoc />
    public partial class CrendentialCollectionAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CredentialCollection",
                schema: "cred2",
                columns: table => new
                {
                    CredentialCollectionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CredentialCollection", x => x.CredentialCollectionId);
                    table.ForeignKey(
                        name: "FK_CredentialCollection_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CredentialCollectionVerifiableCredential",
                schema: "cred2",
                columns: table => new
                {
                    CredentialCollectionId = table.Column<long>(type: "bigint", nullable: false),
                    VerifiableCredentialId = table.Column<long>(type: "bigint", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CredentialCollectionVerifiableCredential", x => new { x.CredentialCollectionId, x.VerifiableCredentialId });
                    table.ForeignKey(
                        name: "FK_CredentialCollectionVerifiableCredential_CredentialCollection_CredentialCollectionId",
                        column: x => x.CredentialCollectionId,
                        principalSchema: "cred2",
                        principalTable: "CredentialCollection",
                        principalColumn: "CredentialCollectionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CredentialCollectionVerifiableCredential_VerifiableCredential_VerifiableCredentialId",
                        column: x => x.VerifiableCredentialId,
                        principalSchema: "cred2",
                        principalTable: "VerifiableCredential",
                        principalColumn: "VerifiableCredentialId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CredentialCollection_UserId",
                schema: "cred2",
                table: "CredentialCollection",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialCollectionVerifiableCredential_VerifiableCredentialId",
                schema: "cred2",
                table: "CredentialCollectionVerifiableCredential",
                column: "VerifiableCredentialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CredentialCollectionVerifiableCredential",
                schema: "cred2");

            migrationBuilder.DropTable(
                name: "CredentialCollection",
                schema: "cred2");
        }
    }
}
