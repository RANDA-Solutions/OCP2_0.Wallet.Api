using Microsoft.EntityFrameworkCore.Migrations;
using OpenCredentialPublisher.Data.Custom.Contexts.SqlObjects.Views;

#nullable disable

namespace OpenCredentialPublisher.Data.Custom.Migrations
{
    /// <inheritdoc />
    public partial class ViewsFromClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"exec('{SearchCredentialCollectionView.GetDropSql().Replace("'", "''")}')");
            migrationBuilder.Sql($"exec('{SearchCredentialCollectionView.GetCreateSql(SearchCredentialCollectionView.VersionNumber.V20240731134030).Replace("'", "''")}')");

            migrationBuilder.Sql($"exec('{SearchCredentialPackageAchievementTypeView.GetDropSql().Replace("'", "''")}')");
            migrationBuilder.Sql($"exec('{SearchCredentialPackageAchievementTypeView.GetCreateSql(SearchCredentialPackageAchievementTypeView.VersionNumber.V20240731134030).Replace("'", "''")}')");

            migrationBuilder.Sql($"exec('{SearchCredentialPackageIssuerView.GetDropSql().Replace("'", "''")}')");
            migrationBuilder.Sql($"exec('{SearchCredentialPackageIssuerView.GetCreateSql(SearchCredentialPackageIssuerView.VersionNumber.V20240731134030).Replace("'", "''")}')");

            migrationBuilder.Sql($"exec('{SearchCredentialPackageView.GetDropSql().Replace("'", "''")}')");
            migrationBuilder.Sql($"exec('{SearchCredentialPackageView.GetCreateSql(SearchCredentialPackageView.VersionNumber.V20240731134030).Replace("'", "''")}')");

            migrationBuilder.Sql($"exec('{SearchCredentialView.GetDropSql().Replace("'", "''")}')");
            migrationBuilder.Sql($"exec('{SearchCredentialView.GetCreateSql(SearchCredentialView.VersionNumber.V20240731134030).Replace("'", "''")}')");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"exec('{SearchCredentialCollectionView.GetDropSql().Replace("'", "''")}')");
            migrationBuilder.Sql($"exec('{SearchCredentialCollectionView.GetCreateSqlPrevious(SearchCredentialCollectionView.VersionNumber.V20240731134030).Replace("'", "''")}')");

            migrationBuilder.Sql($"exec('{SearchCredentialPackageAchievementTypeView.GetDropSql().Replace("'", "''")}')");
            migrationBuilder.Sql($"exec('{SearchCredentialPackageAchievementTypeView.GetCreateSqlPrevious(SearchCredentialPackageAchievementTypeView.VersionNumber.V20240731134030).Replace("'", "''")}')");

            migrationBuilder.Sql($"exec('{SearchCredentialPackageIssuerView.GetDropSql().Replace("'", "''")}')");
            migrationBuilder.Sql($"exec('{SearchCredentialPackageIssuerView.GetCreateSqlPrevious(SearchCredentialPackageIssuerView.VersionNumber.V20240731134030).Replace("'", "''")}')");

            migrationBuilder.Sql($"exec('{SearchCredentialPackageView.GetDropSql().Replace("'", "''")}')");
            migrationBuilder.Sql($"exec('{SearchCredentialPackageView.GetCreateSqlPrevious(SearchCredentialPackageView.VersionNumber.V20240731134030).Replace("'", "''")}')");

            migrationBuilder.Sql($"exec('{SearchCredentialView.GetDropSql().Replace("'", "''")}')");
            migrationBuilder.Sql($"exec('{SearchCredentialView.GetCreateSqlPrevious(SearchCredentialView.VersionNumber.V20240731134030).Replace("'", "''")}')");

        }
    }
}
