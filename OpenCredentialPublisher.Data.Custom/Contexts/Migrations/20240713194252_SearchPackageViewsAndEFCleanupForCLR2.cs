using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenCredentialPublisher.Data.Custom.Migrations
{
    /// <inheritdoc />
    public partial class SearchPackageViewsAndEFCleanupForCLR2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CredentialPackage_Profile_IssuerProfileId",
                schema: "cred2",
                table: "CredentialPackage");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_CredentialPackage_IssuerProfileId",
                schema: "cred2",
                table: "CredentialPackage");

            migrationBuilder.DropColumn(
                name: "AwardedDate",
                schema: "cred2",
                table: "CredentialPackage");

            migrationBuilder.DropColumn(
                name: "ExpiresDate",
                schema: "cred2",
                table: "CredentialPackage");

            migrationBuilder.DropColumn(
                name: "IssuanceDate",
                schema: "cred2",
                table: "CredentialPackage");

            migrationBuilder.DropColumn(
                name: "IssuerProfileId",
                schema: "cred2",
                table: "CredentialPackage");

            migrationBuilder.RenameColumn(
                name: "IssuanceDate",
                schema: "cred2",
                table: "VerifiableCredential",
                newName: "ValidUntilDate");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedAt",
                schema: "cred2",
                table: "VerifiableCredential",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "AwardedDate",
                schema: "cred2",
                table: "VerifiableCredential",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "IssuerProfileId",
                schema: "cred2",
                table: "VerifiableCredential",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ValidFromDate",
                schema: "cred2",
                table: "VerifiableCredential",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedAt",
                schema: "cred2",
                table: "Profile",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "cred2",
                table: "CredentialPackage",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedAt",
                schema: "cred2",
                table: "CredentialPackage",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedAt",
                schema: "cred2",
                table: "Achievement",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    IssuerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuerImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AchievementCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AssertionCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notification_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_VerifiableCredential_IssuerProfileId",
                schema: "cred2",
                table: "VerifiableCredential",
                column: "IssuerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialPackage_UserId",
                schema: "cred2",
                table: "CredentialPackage",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserId",
                table: "Notification",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CredentialPackage_AspNetUsers_UserId",
                schema: "cred2",
                table: "CredentialPackage",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VerifiableCredential_Profile_IssuerProfileId",
                schema: "cred2",
                table: "VerifiableCredential",
                column: "IssuerProfileId",
                principalSchema: "cred2",
                principalTable: "Profile",
                principalColumn: "ProfileId",
                onDelete: ReferentialAction.Restrict);



            migrationBuilder.Sql("DROP VIEW [cred].[SearchCredentialPackagesView]");
            migrationBuilder.Sql("DROP VIEW [cred].[SearchCredentialPackageIssuersView]");
            migrationBuilder.Sql("DROP VIEW [cred].[SearchCredentialPackageCredentialTypesView]");

            migrationBuilder.Sql(@"CREATE VIEW [cred2].[SearchCredentialPackageView]
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
            migrationBuilder.Sql(@"CREATE VIEW [cred2].[SearchCredentialPackageIssuerView]
AS
SELECT DISTINCT 
	cp.[CredentialPackageId],
	COALESCE(p.[Name],p.[Id]) AS [IssuerName],
	COALESCE(vc.[ValidFromDate],vc.[AwardedDate]) AS [IssuedAt],
	DATEPART(YEAR,COALESCE(vc.[ValidFromDate],vc.[AwardedDate])) AS [IssuedAtYear]
FROM [cred2].[CredentialPackage] cp (NOLOCK) 
JOIN [cred2].[VerifiableCredential] vc (NOLOCK) ON cp.CredentialPackageId=vc.CredentialPackageId
JOIN [cred2].[Profile] p (NOLOCK) ON vc.IssuerProfileId=p.ProfileId
WHERE cp.IsDeleted=0
AND vc.IsDeleted=0
AND p.IsDeleted=0");
            migrationBuilder.Sql(@"CREATE VIEW [cred2].[SearchCredentialPackageAchievementTypeView]
AS
SELECT	DISTINCT
        vc.[CredentialPackageId],
		a.[AchievementType]
FROM [cred2].[VerifiableCredential] vc (NOLOCK) 
LEFT JOIN [cred2].[Achievement] a (NOLOCK) ON vc.VerifiableCredentialId=a.VerifiableCredentialId
WHERE vc.IsDeleted=0
AND a.IsDeleted=0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CredentialPackage_AspNetUsers_UserId",
                schema: "cred2",
                table: "CredentialPackage");

            migrationBuilder.DropForeignKey(
                name: "FK_VerifiableCredential_Profile_IssuerProfileId",
                schema: "cred2",
                table: "VerifiableCredential");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_VerifiableCredential_IssuerProfileId",
                schema: "cred2",
                table: "VerifiableCredential");

            migrationBuilder.DropIndex(
                name: "IX_CredentialPackage_UserId",
                schema: "cred2",
                table: "CredentialPackage");

            migrationBuilder.DropColumn(
                name: "AwardedDate",
                schema: "cred2",
                table: "VerifiableCredential");

            migrationBuilder.DropColumn(
                name: "IssuerProfileId",
                schema: "cred2",
                table: "VerifiableCredential");

            migrationBuilder.DropColumn(
                name: "ValidFromDate",
                schema: "cred2",
                table: "VerifiableCredential");

            migrationBuilder.RenameColumn(
                name: "ValidUntilDate",
                schema: "cred2",
                table: "VerifiableCredential",
                newName: "IssuanceDate");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedAt",
                schema: "cred2",
                table: "VerifiableCredential",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "SYSDATETIMEOFFSET()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedAt",
                schema: "cred2",
                table: "Profile",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "SYSDATETIMEOFFSET()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "cred2",
                table: "CredentialPackage",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedAt",
                schema: "cred2",
                table: "CredentialPackage",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "SYSDATETIMEOFFSET()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "AwardedDate",
                schema: "cred2",
                table: "CredentialPackage",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpiresDate",
                schema: "cred2",
                table: "CredentialPackage",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "IssuanceDate",
                schema: "cred2",
                table: "CredentialPackage",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "IssuerProfileId",
                schema: "cred2",
                table: "CredentialPackage",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedAt",
                schema: "cred2",
                table: "Achievement",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "SYSDATETIMEOFFSET()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AchievementCount = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    AssertionCount = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IssuerImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CredentialPackage_IssuerProfileId",
                schema: "cred2",
                table: "CredentialPackage",
                column: "IssuerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CredentialPackage_Profile_IssuerProfileId",
                schema: "cred2",
                table: "CredentialPackage",
                column: "IssuerProfileId",
                principalSchema: "cred2",
                principalTable: "Profile",
                principalColumn: "ProfileId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql(@"
CREATE VIEW [cred].[SearchCredentialPackagesView]
AS
SELECT	cp.Id AS [CredentialPackageId],
		cp.TypeId,
		cp.[UserId] AS [OwnerUserId],
		cp.CreatedAt,
		cp.AssertionsCount, 
		vc.CredentialsCount AS [CredentialsCount],
		vc.[Json]
FROM cred.[CredentialPackages] cp (NOLOCK)
JOIN cred.[VerifiableCredentials] vc (NOLOCK) ON cp.Id = vc.ParentCredentialPackageId
WHERE cp.IsDeleted = 0
AND vc.IsDeleted = 0
UNION
SELECT	cp.Id AS [CredentialPackageId], 
		cp.TypeId,
		cp.[UserId] AS [OwnerUserId],
		cp.CreatedAt,
		cp.AssertionsCount, 
		0 AS [CredentialsCount],
		cs.[Json]
FROM cred.[CredentialPackages] cp (NOLOCK)
JOIN cred.[ClrSets] cs (NOLOCK) ON cp.Id = cs.ParentCredentialPackageId
JOIN cred.[Clrs] c (NOLOCK) ON cs.Id = c.ParentClrSetId
WHERE cp.IsDeleted = 0
AND cs.IsDeleted = 0
AND c.IsDeleted = 0
UNION 
SELECT	cp.Id AS [CredentialPackageId],
		cp.TypeId,
		cp.[UserId] AS [OwnerUserId],
		cp.CreatedAt,
		cp.AssertionsCount, 
		0 AS [CredentialsCount],	
		c.[Json]
FROM cred.[CredentialPackages] cp (NOLOCK)
JOIN cred.[Clrs] c (NOLOCK) ON cp.Id = c.ParentCredentialPackageId
WHERE cp.IsDeleted = 0
AND c.IsDeleted = 0;");

            migrationBuilder.Sql(@"
CREATE VIEW [cred].[SearchCredentialPackageIssuersView]
AS
SELECT	cp.Id AS [CredentialPackageId],
		vc.IssuedOn,
		DATEPART(YEAR,vc.IssuedOn) AS [IssuedOnYear],
		N'' AS IssuerName
FROM cred.[CredentialPackages] cp (NOLOCK)
JOIN cred.[VerifiableCredentials] vc (NOLOCK) ON cp.Id = vc.ParentCredentialPackageId
UNION 
SELECT	cp.Id AS [CredentialPackageId], 
		c.IssuedOn,
		DATEPART(YEAR,c.IssuedOn) AS [IssuedOnYear],
		c.PublisherName AS [IssuerName]
FROM cred.[CredentialPackages] cp (NOLOCK)
JOIN cred.[ClrSets] cs (NOLOCK) ON cp.Id = cs.ParentCredentialPackageId
JOIN cred.[Clrs] c (NOLOCK) ON cs.Id = c.ParentClrSetId
WHERE cp.IsDeleted = 0
AND cs.IsDeleted = 0
AND c.IsDeleted = 0
UNION 
SELECT	cp.Id AS [CredentialPackageId],
		c.IssuedOn,
		DATEPART(YEAR,c.IssuedOn) AS [IssuedOnYear],
		c.PublisherName AS [IssuerName]
FROM cred.[CredentialPackages] cp (NOLOCK)
JOIN cred.[Clrs] c (NOLOCK) ON cp.Id = c.ParentCredentialPackageId
WHERE cp.IsDeleted = 0
AND c.IsDeleted = 0;");

            migrationBuilder.Sql(@"
CREATE VIEW [cred].[SearchCredentialPackageCredentialTypesView]
AS
SELECT	cp.Id AS [CredentialPackageId],
		N'VC Unknown' AS [CredentialType]
FROM cred.[CredentialPackages] cp (NOLOCK)
JOIN cred.[VerifiableCredentials] vc (NOLOCK) ON cp.Id = vc.ParentCredentialPackageId
UNION 
SELECT	cp.Id AS [CredentialPackageId],
		ISNULL(a.AchievementType,N'CS Unknown') AS [CredentialType]
FROM cred.[CredentialPackages] cp (NOLOCK)
JOIN cred.[ClrSets] cs (NOLOCK) ON cp.Id = cs.ParentCredentialPackageId
JOIN cred.[Clrs] c (NOLOCK) ON cs.Id = c.ParentClrSetId
LEFT JOIN cred.[ClrAchievements] ca (NOLOCK) ON c.ClrId = ca.ClrId AND ca.IsDeleted=0
LEFT JOIN cred.[Achievements] a (NOLOCK) ON ca.AchievementId=a.AchievementId AND a.IsDeleted=0
WHERE cp.IsDeleted = 0
AND cs.IsDeleted = 0
AND c.IsDeleted = 0
UNION 
SELECT	cp.Id AS [CredentialPackageId],
		ISNULL(a.AchievementType,N'C Unknown') AS [CredentialType]
FROM cred.[CredentialPackages] cp (NOLOCK)
JOIN cred.[Clrs] c (NOLOCK) ON cp.Id = c.ParentCredentialPackageId
LEFT JOIN cred.[ClrAchievements] ca (NOLOCK) ON c.ClrId = ca.ClrId AND ca.IsDeleted=0
LEFT JOIN cred.[Achievements] a (NOLOCK) ON ca.AchievementId=a.AchievementId AND a.IsDeleted=0
WHERE cp.IsDeleted = 0
AND c.IsDeleted = 0;");

        }
    }
}
