using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Data.Custom.EntityTypeConfigurations
{
    public class AchievementIdentityConfiguration : IEntityTypeConfiguration<AchievementIdentity>
    {
        public void Configure(EntityTypeBuilder<AchievementIdentity> builder)
        {
            builder.ToTable(nameof(AchievementIdentity), EntityTypeConfigurationConstants.SCHEMA_V2);

            builder.HasKey(e => e.AchievementIdentityId);
            builder.Property(e => e.AchievementIdentityId)
                .IsRequired()
                .ValueGeneratedOnAdd(); // Configuring as identity column;

            builder.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            // Configure properties
            builder.Property(e => e.Name).IsRequired(false);
            builder.Property(e => e.EmailAddress).IsRequired(false);

            builder.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            builder.Property(e => e.ModifiedAt)
                .IsRequired(false);

            builder.Ignore(e => e.DisplayName);

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}