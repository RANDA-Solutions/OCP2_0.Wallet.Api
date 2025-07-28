using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Data.Custom.EntityTypeConfigurations
{
    public class AssociationConfiguration : IEntityTypeConfiguration<Association>
    {
        public void Configure(EntityTypeBuilder<Association> builder)
        {
            builder.ToTable(nameof(Association), EntityTypeConfigurationConstants.SCHEMA_V2);

            builder.HasKey(e => e.AssociationId);
            builder.Property(e => e.AssociationId)
                .IsRequired()
                .ValueGeneratedOnAdd(); // Configuring as identity column

            // Configure properties
            builder.Property(e => e.AssociationType)
                .IsRequired()
                .HasDefaultValue("Unspecified");

            builder.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(e => e.SourceVerifiableCredentialId).IsRequired();

            builder.HasOne(e => e.SourceVerifiableCredential)
                .WithMany(e => e.SourceAssociations) // Assuming a one-to-many relationship
                .HasForeignKey(e => e.SourceVerifiableCredentialId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.TargetVerifiableCredentialId).IsRequired();
            builder.HasOne(e => e.TargetVerifiableCredential)
                .WithMany(e => e.TargetAssociations) // Assuming a one-to-many relationship
                .HasForeignKey(e => e.TargetVerifiableCredentialId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            builder.Property(e => e.ModifiedAt)
                .IsRequired(false);

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
