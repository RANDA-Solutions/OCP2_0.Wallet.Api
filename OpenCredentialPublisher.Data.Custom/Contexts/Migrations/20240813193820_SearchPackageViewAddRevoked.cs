using Microsoft.EntityFrameworkCore.Migrations;
using OpenCredentialPublisher.Data.Custom.Contexts.SqlObjects.Views;

#nullable disable

namespace OpenCredentialPublisher.Data.Custom.Migrations
{
    /// <inheritdoc />
    public partial class SearchPackageViewAddRevoked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"exec('{SearchCredentialPackageView.GetDropSql().Replace("'", "''")}')");
            migrationBuilder.Sql($"exec('{SearchCredentialPackageView.GetCreateSql(SearchCredentialPackageView.VersionNumber.V20240813193820).Replace("'", "''")}')");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"exec('{SearchCredentialPackageView.GetDropSql().Replace("'", "''")}')");
            migrationBuilder.Sql($"exec('{SearchCredentialPackageView.GetCreateSqlPrevious(SearchCredentialPackageView.VersionNumber.V20240813193820).Replace("'", "''")}')");

        }
    }
}
