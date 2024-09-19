using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenCredentialPublisher.Data.Custom.Migrations
{
    /// <inheritdoc />
    public partial class ModelCleanupForCLR2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateTimestamp",
                schema: "cred2",
                table: "VerifiableCredential");

            migrationBuilder.DropColumn(
                name: "ModifiedTimestamp",
                schema: "cred2",
                table: "VerifiableCredential");

            migrationBuilder.DropColumn(
                name: "CreateTimestamp",
                schema: "cred2",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "ModifiedTimestamp",
                schema: "cred2",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "CreateTimestamp",
                schema: "cred2",
                table: "CredentialPackage");

            migrationBuilder.DropColumn(
                name: "ModifiedTimestamp",
                schema: "cred2",
                table: "CredentialPackage");

            migrationBuilder.DropColumn(
                name: "CreateTimestamp",
                schema: "cred2",
                table: "Achievement");

            migrationBuilder.DropColumn(
                name: "ModifiedTimestamp",
                schema: "cred2",
                table: "Achievement");

            migrationBuilder.RenameColumn(
                name: "Image",
                schema: "cred2",
                table: "Profile",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                table: "Notifications",
                newName: "ModifiedAt");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                schema: "cred2",
                table: "VerifiableCredential",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "cred2",
                table: "VerifiableCredential",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedAt",
                schema: "cred2",
                table: "VerifiableCredential",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                schema: "cred2",
                table: "Profile",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "cred2",
                table: "Profile",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedAt",
                schema: "cred2",
                table: "Profile",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                schema: "cred2",
                table: "CredentialPackage",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedAt",
                schema: "cred2",
                table: "CredentialPackage",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                schema: "cred2",
                table: "Achievement",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "cred2",
                table: "Achievement",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedAt",
                schema: "cred2",
                table: "Achievement",
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
                table: "VerifiableCredential");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "cred2",
                table: "VerifiableCredential");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                schema: "cred2",
                table: "VerifiableCredential");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "cred2",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "cred2",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                schema: "cred2",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "cred2",
                table: "CredentialPackage");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                schema: "cred2",
                table: "CredentialPackage");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "cred2",
                table: "Achievement");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "cred2",
                table: "Achievement");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                schema: "cred2",
                table: "Achievement");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                schema: "cred2",
                table: "Profile",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "Notifications",
                newName: "ModifiedOn");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreateTimestamp",
                schema: "cred2",
                table: "VerifiableCredential",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedTimestamp",
                schema: "cred2",
                table: "VerifiableCredential",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreateTimestamp",
                schema: "cred2",
                table: "Profile",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedTimestamp",
                schema: "cred2",
                table: "Profile",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreateTimestamp",
                schema: "cred2",
                table: "CredentialPackage",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedTimestamp",
                schema: "cred2",
                table: "CredentialPackage",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreateTimestamp",
                schema: "cred2",
                table: "Achievement",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedTimestamp",
                schema: "cred2",
                table: "Achievement",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");
        }
    }
}
