using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenCredentialPublisher.Data.Custom.Migrations
{
    /// <inheritdoc />
    public partial class SetupLinkAndOtherModelCleanup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authorization_AspNetUsers_UserId",
                schema: "cred2",
                table: "Authorization");

            migrationBuilder.DropForeignKey(
                name: "FK_Authorization_Source_SourceForeignKey",
                schema: "cred2",
                table: "Authorization");

            migrationBuilder.DropTable(
                name: "LoginLink",
                schema: "cred2");

            migrationBuilder.DropIndex(
                name: "IX_Authorization_SourceForeignKey",
                schema: "cred2",
                table: "Authorization");

            migrationBuilder.DropColumn(
                name: "SourceForeignKey",
                schema: "cred2",
                table: "Authorization");

            migrationBuilder.DropPrimaryKey(name: "PK_Source", table: "Source", schema: "cred2");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                schema: "cred2",
                table: "Source",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "cred2",
                table: "Source",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedAt",
                schema: "cred2",
                table: "Source",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "cred2",
                table: "Source",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                schema: "cred2",
                table: "Source",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "SYSDATETIMEOFFSET()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "ClientSecret",
                schema: "cred2",
                table: "Source",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                schema: "cred2",
                table: "Source",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "cred2",
                table: "Source",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.DropPrimaryKey(name: "PK_Message", table: "Message", schema: "cred2");

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                schema: "cred2",
                table: "Message",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SendAttempts",
                schema: "cred2",
                table: "Message",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Recipient",
                schema: "cred2",
                table: "Message",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedAt",
                schema: "cred2",
                table: "Message",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "cred2",
                table: "Message",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                schema: "cred2",
                table: "Message",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "SYSDATETIMEOFFSET()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Body",
                schema: "cred2",
                table: "Message",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "cred2",
                table: "Message",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.DropPrimaryKey(name: "PK_Authorization", table: "Authorization", schema: "cred2");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "cred2",
                table: "Authorization",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedAt",
                schema: "cred2",
                table: "Authorization",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "cred2",
                table: "Authorization",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                schema: "cred2",
                table: "Authorization",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "SYSDATETIMEOFFSET()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                schema: "cred2",
                table: "Authorization",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValueSql: "NEWID()",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<long>(
                name: "SourceId",
                schema: "cred2",
                table: "Authorization",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(name: "PK_Source", table: "Source", schema: "cred2", column: "Id");
            migrationBuilder.AddPrimaryKey(name: "PK_Authorization", table: "Authorization", schema: "cred2", column: "Id");
            migrationBuilder.AddPrimaryKey(name: "PK_Message", table: "Message", schema: "cred2", column: "Id");

            migrationBuilder.CreateTable(
                name: "SetupLink",
                schema: "cred2",
                columns: table => new
                {
                    LoginLinkId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Token = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AccessCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Claimed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    MessageId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ValidUntil = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetupLink", x => x.LoginLinkId);
                    table.ForeignKey(
                        name: "FK_SetupLink_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SetupLink_Message_MessageId",
                        column: x => x.MessageId,
                        principalSchema: "cred2",
                        principalTable: "Message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Message_StatusId",
                schema: "cred2",
                table: "Message",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Authorization_SourceId",
                schema: "cred2",
                table: "Authorization",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_SetupLink_MessageId",
                schema: "cred2",
                table: "SetupLink",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_SetupLink_UserId",
                schema: "cred2",
                table: "SetupLink",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authorization_AspNetUsers_UserId",
                schema: "cred2",
                table: "Authorization",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Authorization_Source_SourceId",
                schema: "cred2",
                table: "Authorization",
                column: "SourceId",
                principalSchema: "cred2",
                principalTable: "Source",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Statuses_StatusId",
                schema: "cred2",
                table: "Message",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authorization_AspNetUsers_UserId",
                schema: "cred2",
                table: "Authorization");

            migrationBuilder.DropForeignKey(
                name: "FK_Authorization_Source_SourceId",
                schema: "cred2",
                table: "Authorization");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Statuses_StatusId",
                schema: "cred2",
                table: "Message");

            migrationBuilder.DropTable(
                name: "SetupLink",
                schema: "cred2");

            migrationBuilder.DropIndex(
                name: "IX_Message_StatusId",
                schema: "cred2",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Authorization_SourceId",
                schema: "cred2",
                table: "Authorization");

            migrationBuilder.DropColumn(
                name: "SourceId",
                schema: "cred2",
                table: "Authorization");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                schema: "cred2",
                table: "Source",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "cred2",
                table: "Source",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                schema: "cred2",
                table: "Source",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "cred2",
                table: "Source",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "cred2",
                table: "Source",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.AlterColumn<string>(
                name: "ClientSecret",
                schema: "cred2",
                table: "Source",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                schema: "cred2",
                table: "Source",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "cred2",
                table: "Source",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                schema: "cred2",
                table: "Message",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "SendAttempts",
                schema: "cred2",
                table: "Message",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Recipient",
                schema: "cred2",
                table: "Message",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                schema: "cred2",
                table: "Message",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "cred2",
                table: "Message",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "cred2",
                table: "Message",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.AlterColumn<string>(
                name: "Body",
                schema: "cred2",
                table: "Message",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "cred2",
                table: "Message",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "cred2",
                table: "Authorization",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                schema: "cred2",
                table: "Authorization",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "cred2",
                table: "Authorization",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "cred2",
                table: "Authorization",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                schema: "cred2",
                table: "Authorization",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40,
                oldDefaultValueSql: "NEWID()");

            migrationBuilder.AddColumn<int>(
                name: "SourceForeignKey",
                schema: "cred2",
                table: "Authorization",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LoginLink",
                schema: "cred2",
                columns: table => new
                {
                    LoginLinkId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Claimed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Code = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    MessageId = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ReturnUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ValidUntil = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginLink", x => x.LoginLinkId);
                    table.ForeignKey(
                        name: "FK_LoginLink_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LoginLink_Message_MessageId",
                        column: x => x.MessageId,
                        principalSchema: "cred2",
                        principalTable: "Message",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authorization_SourceForeignKey",
                schema: "cred2",
                table: "Authorization",
                column: "SourceForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_LoginLink_MessageId",
                schema: "cred2",
                table: "LoginLink",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_LoginLink_UserId",
                schema: "cred2",
                table: "LoginLink",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authorization_AspNetUsers_UserId",
                schema: "cred2",
                table: "Authorization",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Authorization_Source_SourceForeignKey",
                schema: "cred2",
                table: "Authorization",
                column: "SourceForeignKey",
                principalSchema: "cred2",
                principalTable: "Source",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
