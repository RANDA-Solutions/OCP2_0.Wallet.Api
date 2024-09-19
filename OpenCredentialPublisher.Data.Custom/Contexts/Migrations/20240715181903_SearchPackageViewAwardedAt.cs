using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenCredentialPublisher.Data.Custom.Migrations
{
    /// <inheritdoc />
    public partial class SearchPackageViewAwardedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER VIEW [cred2].[SearchCredentialPackageView]
AS
SELECT cp.[CredentialPackageId],
    cp.[Name],
	COALESCE(p.[Name],p.[Id]) AS [IssuerName],
	p.[ImageUrl] AS [IssuerImageUrl],
    cp.[UserId] AS [OwnerUserId],
	cp.[AchievementCount],
	CAST(0 AS INT) AS [ShareCount],
    COALESCE(vc.[AwardedDate],vc.[ValidFromDate]) AS [AwardedAt],
    cp.[CreatedAt],
	vc.[Json]              
FROM [cred2].[CredentialPackage] cp (NOLOCK) 
JOIN [cred2].[VerifiableCredential] vc (NOLOCK) ON cp.CredentialPackageId=vc.CredentialPackageId AND vc.ParentVerifiableCredentialId IS NULL 
JOIN [cred2].[Profile] p (NOLOCK) ON vc.IssuerProfileId=p.ProfileId
WHERE cp.IsDeleted=0
AND vc.IsDeleted=0
AND p.IsDeleted=0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER VIEW [cred2].[SearchCredentialPackageView]
AS
SELECT cp.[CredentialPackageId],
    cp.[Name],
	COALESCE(p.[Name],p.[Id]) AS [IssuerName],
	p.[ImageUrl] AS [IssuerImageUrl],
    cp.[UserId] AS [OwnerUserId],
	cp.[AchievementCount],
	CAST(0 AS INT) AS [ShareCount],
    COALESCE(vc.[ValidFromDate],vc.[AwardedDate]) AS [PublishedAt],
    cp.[CreatedAt],
	vc.[Json]              
FROM [cred2].[CredentialPackage] cp (NOLOCK) 
JOIN [cred2].[VerifiableCredential] vc (NOLOCK) ON cp.CredentialPackageId=vc.CredentialPackageId AND vc.ParentVerifiableCredentialId IS NULL 
JOIN [cred2].[Profile] p (NOLOCK) ON vc.IssuerProfileId=p.ProfileId
WHERE cp.IsDeleted=0
AND vc.IsDeleted=0
AND p.IsDeleted=0");
        }
    }
}
