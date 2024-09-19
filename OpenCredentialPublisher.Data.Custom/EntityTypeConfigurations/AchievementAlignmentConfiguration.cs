using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Data.Custom.EntityTypeConfigurations
{
    public class AchievementAlignmentConfiguration : IEntityTypeConfiguration<AchievementAlignment>
    {
        public void Configure(EntityTypeBuilder<AchievementAlignment> builder)
        {
            builder.ToTable(nameof(AchievementAlignment), EntityTypeConfigurationConstants.SCHEMA_V2);

            builder.HasKey(e => e.AchievementAlignmentId);
            builder.Property(e => e.AchievementAlignmentId)
                .IsRequired()
                .ValueGeneratedOnAdd(); // Configuring as identity column

            builder.Property(e => e.AchievementId).IsRequired();

            builder.Property(e => e.Type).IsRequired();
            builder.Property(e => e.TargetCode).IsRequired(false);
            builder.Property(e => e.TargetDescription).IsRequired(false);
            builder.Property(e => e.TargetName).IsRequired();
            builder.Property(e => e.TargetFramework).IsRequired(false);
            builder.Property(e => e.TargetType).IsRequired(false);
            builder.Property(e => e.TargetUrl).IsRequired();

            builder.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            builder.Property(e => e.ModifiedAt)
                .IsRequired(false);

            builder.HasOne(e => e.Achievement)
                .WithMany(a => a.Alignments)
                .HasForeignKey(e => e.AchievementId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}