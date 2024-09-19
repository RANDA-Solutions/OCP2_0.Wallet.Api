using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenCredentialPublisher.Data.Custom.Migrations
{
    /// <inheritdoc />
    public partial class CredentialCollectionVerifiableCredentialCreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedAt",
                schema: "cred2",
                table: "CredentialCollectionVerifiableCredential");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                schema: "cred2",
                table: "CredentialCollectionVerifiableCredential",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "SYSDATETIMEOFFSET()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "cred2",
                table: "CredentialCollectionVerifiableCredential");

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedAt",
                schema: "cred2",
                table: "CredentialCollectionVerifiableCredential",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "SYSDATETIMEOFFSET()");
        }
    }
}
