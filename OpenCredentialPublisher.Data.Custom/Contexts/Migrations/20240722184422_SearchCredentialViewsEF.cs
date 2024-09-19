using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenCredentialPublisher.Data.Custom.Migrations
{
    /// <inheritdoc />
    public partial class SearchCredentialViewsEF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW [cred2].[SearchCredentialView]
AS
SELECT cp.[CredentialPackageId],
	vc.[VerifiableCredentialId],
	ISNULL(a.[AchievementType],N'Unspecified') AS [AchievementType],
	COALESCE(p.[Name],p.[Id]) AS [IssuerName],
    cp.[UserId] AS [OwnerUserId],
    COALESCE(vc.[AwardedDate],vc.[ValidFromDate]) AS [EffectiveAt],
    DATEPART(YEAR,COALESCE(vc.[AwardedDate],vc.[ValidFromDate])) AS [EffectiveAtYear],
    cp.[CreatedAt],
	vc.[Json]
FROM [cred2].[VerifiableCredential] vc (NOLOCK) 
JOIN [cred2].[CredentialPackage] cp (NOLOCK) ON cp.CredentialPackageId=vc.CredentialPackageId AND vc.ParentVerifiableCredentialId IS NOT NULL
JOIN [cred2].[Profile] p (NOLOCK) ON vc.IssuerProfileId=p.ProfileId
LEFT JOIN [cred2].[Achievement] a (NOLOCK) ON vc.VerifiableCredentialId = a.VerifiableCredentialId
WHERE cp.IsDeleted=0
AND vc.IsDeleted=0
AND p.IsDeleted=0;");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW [cred2].[SearchCredentialView]");
        }
    }
}
