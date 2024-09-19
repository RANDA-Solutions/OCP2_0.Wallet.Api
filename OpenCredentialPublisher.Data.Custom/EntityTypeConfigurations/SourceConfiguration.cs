using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Data.Custom.EntityTypeConfigurations
{
    public class SourceConfiguration : IEntityTypeConfiguration<Source>
    {
        public void Configure(EntityTypeBuilder<Source> builder)
        {
            builder.ToTable(nameof(Source), EntityTypeConfigurationConstants.SCHEMA_V2);

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(s => s.ClientId)
                .IsRequired()
                .HasMaxLength(1000); // Set appropriate max length

            builder.Property(s => s.ClientSecret)
                .IsRequired()
                .HasMaxLength(1000); // Set appropriate max length

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(1000); // Set appropriate max length

            builder.Property(s => s.Scope)
                .IsRequired(false);

            builder.Property(s => s.Url)
                .IsRequired();

            builder.Property(s => s.IsDeletable)
                .IsRequired();

            builder.Property(s => s.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(s => s.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            builder.Property(s => s.ModifiedAt)
                .IsRequired(false);

            builder.HasMany(s => s.Authorizations)
                .WithOne(a => a.Source)
                .HasForeignKey(a => a.SourceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(s => !s.IsDeleted);
        }
    }
}
