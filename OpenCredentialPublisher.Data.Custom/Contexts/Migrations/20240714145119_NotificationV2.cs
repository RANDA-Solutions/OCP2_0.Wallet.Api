using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenCredentialPublisher.Data.Custom.Migrations
{
    /// <inheritdoc />
    public partial class NotificationV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "AssertionCount",
                table: "Notification");

            migrationBuilder.RenameTable(
                name: "Notification",
                newName: "Notification",
                newSchema: "cred2");

            migrationBuilder.AddColumn<long>(
                name: "NotificationId",
                schema: "cred2",
                table: "Notification",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                schema: "cred2",
                table: "Notification",
                column: "NotificationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                schema: "cred2",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "NotificationId",
                schema: "cred2",
                table: "Notification");

            migrationBuilder.RenameTable(
                name: "Notification",
                schema: "cred2",
                newName: "Notification");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Notification",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "AssertionCount",
                table: "Notification",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "Id");
        }
    }
}
