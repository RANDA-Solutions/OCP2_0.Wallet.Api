using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenCredentialPublisher.Data.Custom.EFModels;
using OpenCredentialPublisher.Data.Models;

namespace OpenCredentialPublisher.Data.Custom.EntityTypeConfigurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable(nameof(Notification), EntityTypeConfigurationConstants.SCHEMA_V2);
            builder.HasKey(e => e.NotificationId);

            builder.Property(e => e.NotificationId)
                .IsRequired()
                .ValueGeneratedOnAdd(); // Configuring as identity column

            builder.Property(e => e.UserId)
                .HasMaxLength(450);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.Property(n => n.IsDeleted)
                .HasDefaultValue(false);

            builder
                .Property(n => n.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            builder
                .Property(n => n.ModifiedAt)
                .IsRequired(false);

            builder.Property(n => n.AchievementCount)
                .HasDefaultValue(0);


            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
