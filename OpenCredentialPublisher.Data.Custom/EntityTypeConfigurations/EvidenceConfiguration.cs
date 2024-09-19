using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Data.Custom.EntityTypeConfigurations
{
    public class EvidenceConfiguration : IEntityTypeConfiguration<Evidence>
    {
        public void Configure(EntityTypeBuilder<Evidence> builder)
        {
            builder.ToTable(nameof(Evidence), EntityTypeConfigurationConstants.SCHEMA_V2);

            builder.HasKey(e => e.EvidenceId);
            builder.Property(e => e.EvidenceId)
                .IsRequired()
                .ValueGeneratedOnAdd(); // Configuring as identity column

            builder.Property(e => e.EvidenceUrl)
                .IsRequired(false);

            // Configure properties
            builder.Property(e => e.Type)
                .IsRequired();

            builder.Property(e => e.Name).IsRequired(false);

            builder.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);


            builder.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            builder.Property(e => e.ModifiedAt)
                .IsRequired(false);


            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
