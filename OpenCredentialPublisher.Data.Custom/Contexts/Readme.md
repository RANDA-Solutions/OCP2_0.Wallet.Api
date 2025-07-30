# Entity Framework Migrations Guide

This guide provides step-by-step instructions on how to add Entity Framework (EF) migrations and generate idempotent migration scripts for the **OpenCredentialPublisher** solution. It ensures that the default project is set to `OpenCredentialPublisher.Data.Custom` and the startup project is set to `OpenCredentialPublisher.Wallet`.

## Prerequisites

- **.NET 8 SDK** installed.
- **Entity Framework Core CLI Tools** installed. You can install them globally using the following command:
```
dotnet tool install --global dotnet-ef
```
- Ensure that the `OpenCredentialPublisher.Data.Custom` and `OpenCredentialPublisher.Wallet` projects are part of your solution.

## Setting Up the Environment

Before adding migrations, ensure that the default project and startup project are correctly set.

### 1. Navigate to the Solution Directory

Open your terminal or command prompt and navigate to the root directory of your solution.

```
cd path/to/OpenCredentialPublisher
```

### 2. Add a New Migration

To add a new EF migration, use the `dotnet ef migrations add` command. Ensure that you specify the default project and startup project using the `--project` and `--startup-project` flags, respectively.

```
dotnet ef migrations add YourMigrationName --project OpenCredentialPublisher.Data.Custom --startup-project OpenCredentialPublisher.Wallet
```

**Parameters:**

- `YourMigrationName`: Replace with a meaningful name for your migration (e.g., `AddAssociationTable`).
- `--project`: Specifies the project that contains the `DbContext`. In this case, `OpenCredentialPublisher.Data.Custom`.
- `--startup-project`: Specifies the startup project. In this case, `OpenCredentialPublisher.Wallet`.

**Example:**

```
dotnet ef migrations add AddAssociationTable --project OpenCredentialPublisher.Data.Custom --startup-project OpenCredentialPublisher.Wallet
```

### 3. Apply the Migration to the Database

After adding the migration, apply it to your database using the `dotnet ef database update` command.

```
dotnet ef database update --project OpenCredentialPublisher.Data.Custom --startup-project OpenCredentialPublisher.Wallet
```

## Generating an Idempotent Migration Script

To create an idempotent SQL script that can be used to update the database schema, use the `dotnet ef migrations script` command with the `--idempotent` flag.

### 1. Generate the Script

Run the following command, specifying the default project and startup project:

```
dotnet ef migrations script --idempotent --output MigrationsScript.sql --project OpenCredentialPublisher.Data.Custom --startup-project OpenCredentialPublisher.Wallet
```

**Parameters:**

- `--idempotent`: Ensures that the generated script can be run multiple times without causing errors.
- `--output`: Specifies the output file for the generated script. You can name it as desired, e.g., `MigrationsScript.sql`.
- `--project`: Specifies the project that contains the `DbContext`.
- `--startup-project`: Specifies the startup project.

**Example:**

```
dotnet ef migrations script --idempotent --output MigrationsScript.sql --project OpenCredentialPublisher.Data.Custom --startup-project OpenCredentialPublisher.Wallet
```

### 2. Review the Generated Script

Open the generated `MigrationsScript.sql` file to review the SQL statements. This script can be executed against your database to apply the necessary schema changes.

## Best Practices

- **Consistent Naming:** Use descriptive names for your migrations to easily identify their purpose.

```
dotnet ef migrations add AddAssociationTable --project OpenCredentialPublisher.Data.Custom --startup-project OpenCredentialPublisher.Wallet
```

- **Version Control:** Commit your migration files to your version control system to keep track of schema changes.
  - Add your migration sql scripts to the OpenCredentialPublisher.Data.Sql project inside a folder with the name of the current sprint and name the sql script with the JIRA ticket name.
- **Test Migrations:** Before applying migrations to production databases, test them in a development or staging environment to ensure they work as expected.
- **Use Idempotent Scripts for Deployment:** When deploying to environments where migrations may need to be run multiple times, use idempotent scripts to avoid conflicts.

## Troubleshooting

- **Common Errors:**

  - *"No executable found matching command 'dotnet-ef'"*: Ensure that the EF Core CLI tools are installed globally.
  
	 ```
	dotnet tool install --global dotnet-ef
	 ```

  - *"Unable to find project."*: Verify that you are in the correct directory and that the project names specified in the commands match your solution's projects.

- **Ensure Database Connectivity:** Make sure that your connection strings are correctly configured in the `OpenCredentialPublisher.Wallet` project's configuration files.
- **Migration Conflicts:** If you encounter conflicts during migrations, consider rebasing your migrations or using the `--force` flag with caution.

## References

- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [Dotnet EF CLI Commands](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)

---

For further assistance or issues related to EF migrations, refer to the [Entity Framework Core GitHub Repository](https://github.com/dotnet/efcore) or consult the community forums.
