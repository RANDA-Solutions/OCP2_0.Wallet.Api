using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenCredentialPublisher.Data.Custom.Migrations
{
    /// <inheritdoc />
    public partial class Clr2Associations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Association",
                schema: "cred2",
                columns: table => new
                {
                    AssociationId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceVerifiableCredentialId = table.Column<long>(type: "bigint", nullable: false),
                    TargetVerifiableCredentialId = table.Column<long>(type: "bigint", nullable: false),
                    AssociationType = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Unspecified"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Association", x => x.AssociationId);
                    table.ForeignKey(
                        name: "FK_Association_VerifiableCredential_SourceVerifiableCredentialId",
                        column: x => x.SourceVerifiableCredentialId,
                        principalSchema: "cred2",
                        principalTable: "VerifiableCredential",
                        principalColumn: "VerifiableCredentialId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Association_VerifiableCredential_TargetVerifiableCredentialId",
                        column: x => x.TargetVerifiableCredentialId,
                        principalSchema: "cred2",
                        principalTable: "VerifiableCredential",
                        principalColumn: "VerifiableCredentialId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Association_SourceVerifiableCredentialId",
                schema: "cred2",
                table: "Association",
                column: "SourceVerifiableCredentialId");

            migrationBuilder.CreateIndex(
                name: "IX_Association_TargetVerifiableCredentialId",
                schema: "cred2",
                table: "Association",
                column: "TargetVerifiableCredentialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Association",
                schema: "cred2");
        }
    }
}
