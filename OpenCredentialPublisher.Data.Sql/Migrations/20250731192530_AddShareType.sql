BEGIN TRANSACTION;
GO

ALTER TABLE [cred2].[Share] ADD [ShareType] nvarchar(30) NOT NULL DEFAULT N'email';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250731192530_AddShareType', N'8.0.18');
GO

COMMIT;
GO