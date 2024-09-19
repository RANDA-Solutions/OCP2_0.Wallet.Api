using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenCredentialPublisher.Data.Custom.EFModels;
using OpenCredentialPublisher.Data.Models;

namespace OpenCredentialPublisher.Data.Custom.EntityTypeConfigurations
{
    public class CredentialPackageConfiguration : IEntityTypeConfiguration<CredentialPackage>
    {
        public void Configure(EntityTypeBuilder<CredentialPackage> builder)
        {
            builder.ToTable(nameof(CredentialPackage), EntityTypeConfigurationConstants.SCHEMA_V2);
            builder.HasKey(e => e.CredentialPackageId);

            builder.Property(e => e.CredentialPackageId)
                .IsRequired()
                .ValueGeneratedOnAdd(); // Configuring as identity column

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.Property(e => e.Id)
                .IsRequired()
                .HasMaxLength(450);


            // Configure properties
            builder.Property(e => e.Name).IsRequired(false);
            builder.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            builder.Property(e => e.ModifiedAt)
                .IsRequired(false);

            builder.HasMany(e => e.VerifiableCredentials) // Configure the collection navigation property
                .WithOne(vc => vc.CredentialPackage) // Configure the navigation property on the child
                .HasForeignKey(vc => vc.CredentialPackageId)
                .OnDelete(DeleteBehavior.Restrict); // Define delete behavior as needed

            builder.Ignore(vc => vc.ParentVerifiableCredential);
            builder.Ignore(vc => vc.ChildVerifiableCredentials);

            builder.HasQueryFilter(x => !x.IsDeleted);

        }
    }
}
