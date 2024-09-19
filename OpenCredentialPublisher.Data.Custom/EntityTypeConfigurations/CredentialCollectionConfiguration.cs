using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenCredentialPublisher.Data.Custom.EFModels;
using OpenCredentialPublisher.Data.Models;

namespace OpenCredentialPublisher.Data.Custom.EntityTypeConfigurations
{
    public class CredentialCollectionConfiguration : IEntityTypeConfiguration<CredentialCollection>
    {
        public void Configure(EntityTypeBuilder<CredentialCollection> builder)
        {
            builder.ToTable(nameof(CredentialCollection), EntityTypeConfigurationConstants.SCHEMA_V2);
            builder.HasKey(e => e.CredentialCollectionId);

            builder.Property(e => e.CredentialCollectionId)
                .IsRequired()
                .ValueGeneratedOnAdd(); // Configuring as identity column

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            // Configure properties
            builder.Property(e => e.Name).IsRequired();

            builder.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            builder.Property(e => e.ModifiedAt)
                .IsRequired(false);

            // Configure many-to-many relationship
            // Configure many-to-many relationship
            builder
                .HasMany(cc => cc.CredentialCollectionVerifiableCredentials)
                .WithOne(ccvc => ccvc.CredentialCollection)
                .HasForeignKey(ccvc => ccvc.CredentialCollectionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(x => !x.IsDeleted);

        }
    }
}
