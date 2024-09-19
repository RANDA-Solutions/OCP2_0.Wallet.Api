using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Data.Custom.EntityTypeConfigurations
{
    public class AchievementConfiguration : IEntityTypeConfiguration<Achievement>
    {
        public void Configure(EntityTypeBuilder<Achievement> builder)
        {
            builder.ToTable(nameof(Achievement), EntityTypeConfigurationConstants.SCHEMA_V2);

            builder.HasKey(e => e.AchievementId);
            builder.Property(e => e.AchievementId)
                .IsRequired()
                .ValueGeneratedOnAdd(); // Configuring as identity column

            builder.Property(e => e.Id)
                .IsRequired()
                .HasMaxLength(450);

            // Configure properties
            builder.Property(e => e.AchievementType)
                .IsRequired()
                .HasDefaultValue(Achievement.UNSPECIFIED_ACHIEVEMENT_TYPE);

            builder.Property(e => e.Name).IsRequired(false);
            builder.Property(e => e.Description).IsRequired(false);
            builder.Property(e => e.HumanCode).IsRequired(false);
            builder.Property(e => e.FieldOfStudy).IsRequired(false);
            builder.Property(e => e.LicenseNumber).IsRequired(false);
            builder.Property(e => e.ImageUrl).IsRequired(false);
            builder.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(e => e.CreatorProfileId).IsRequired(false);
            builder.Property(e => e.SourceProfileId).IsRequired(false);

            builder.Property(e => e.VerifiableCredentialId).IsRequired();

            builder.Property(e => e.Type).IsRequired();

            builder.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            builder.Property(e => e.ModifiedAt)
                .IsRequired(false);

            builder.HasOne(e => e.Creator)
                .WithMany() // Assuming a one-to-many relationship
                .HasForeignKey(e => e.CreatorProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Source)
                .WithMany() // Assuming a one-to-many relationship
                .HasForeignKey(e => e.SourceProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.Alignments)
                .WithOne(aa => aa.Achievement)
                .HasForeignKey(aa => aa.AchievementId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(a => a.Identifier)
                .WithOne(ai => ai.Achievement)
                .HasForeignKey<AchievementIdentity>(ai => ai.AchievementId)
                .OnDelete(DeleteBehavior.Restrict); // Define delete behavior as needed


            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
