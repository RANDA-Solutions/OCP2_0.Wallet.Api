using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Data.Custom.EntityTypeConfigurations
{
    public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.ToTable(nameof(Profile), EntityTypeConfigurationConstants.SCHEMA_V2);

            builder.HasKey(e => e.ProfileId);
            builder.Property(e => e.ProfileId)
                .IsRequired()
                .ValueGeneratedOnAdd(); // Configuring as identity column;

            builder.Property(e => e.Id)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            // Configure properties
            builder.Property(e => e.Name).IsRequired(false);
            builder.Property(e => e.ImageUrl).IsRequired(false);
            builder.Property(e => e.Url).IsRequired(false);

            builder.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            builder.Property(e => e.ModifiedAt)
                .IsRequired(false);

            builder.HasQueryFilter(x => !x.IsDeleted);

        }
    }
}
