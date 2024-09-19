using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenCredentialPublisher.Data.Custom.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFolio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Folio_FolioId",
                table: "Tests");

            migrationBuilder.DropTable(
                name: "Folio");

            migrationBuilder.DropIndex(
                name: "IX_Tests_FolioId",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "FolioId",
                table: "Tests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FolioId",
                table: "Tests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Folio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Modified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Folio_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Folio_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tests_FolioId",
                table: "Tests",
                column: "FolioId");

            migrationBuilder.CreateIndex(
                name: "IX_Folio_StatusId",
                table: "Folio",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Folio_UserId",
                table: "Folio",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Folio_FolioId",
                table: "Tests",
                column: "FolioId",
                principalTable: "Folio",
                principalColumn: "Id");
        }
    }
}
