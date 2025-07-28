BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250424221354_Clr2Associations'
)
BEGIN
    CREATE TABLE [cred2].[Association] (
        [AssociationId] bigint NOT NULL IDENTITY,
        [SourceVerifiableCredentialId] bigint NOT NULL,
        [TargetVerifiableCredentialId] bigint NOT NULL,
        [AssociationType] nvarchar(max) NOT NULL DEFAULT N'Unspecified',
        [CreatedAt] datetimeoffset NOT NULL DEFAULT (SYSDATETIMEOFFSET()),
        [ModifiedAt] datetimeoffset NULL,
        [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit),
        CONSTRAINT [PK_Association] PRIMARY KEY ([AssociationId]),
        CONSTRAINT [FK_Association_VerifiableCredential_SourceVerifiableCredentialId] FOREIGN KEY ([SourceVerifiableCredentialId]) REFERENCES [cred2].[VerifiableCredential] ([VerifiableCredentialId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Association_VerifiableCredential_TargetVerifiableCredentialId] FOREIGN KEY ([TargetVerifiableCredentialId]) REFERENCES [cred2].[VerifiableCredential] ([VerifiableCredentialId]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250424221354_Clr2Associations'
)
BEGIN
    CREATE INDEX [IX_Association_SourceVerifiableCredentialId] ON [cred2].[Association] ([SourceVerifiableCredentialId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250424221354_Clr2Associations'
)
BEGIN
    CREATE INDEX [IX_Association_TargetVerifiableCredentialId] ON [cred2].[Association] ([TargetVerifiableCredentialId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250424221354_Clr2Associations'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250424221354_Clr2Associations', N'8.0.6');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250428211841_VcIsChild'
)
BEGIN
    ALTER TABLE [cred2].[VerifiableCredential] ADD [IsChild] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250428211841_VcIsChild'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250428211841_VcIsChild', N'8.0.6');
END;
GO

COMMIT;
GO

