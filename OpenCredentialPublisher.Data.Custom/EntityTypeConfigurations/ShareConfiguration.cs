using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenCredentialPublisher.Data.Custom.EFModels;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Shared.Utilities;

namespace OpenCredentialPublisher.Data.Custom.EntityTypeConfigurations
{
    public class ShareConfiguration : IEntityTypeConfiguration<Share>
    {
        public void Configure(EntityTypeBuilder<Share> builder)
        {
            builder.ToTable(nameof(Share), EntityTypeConfigurationConstants.SCHEMA_V2);
            builder.HasKey(e => e.ShareId);

            builder.Property(e => e.ShareId)
                .IsRequired()
                .ValueGeneratedOnAdd(); // Configuring as identity column

            builder.Property(n => n.UserId)
                .HasMaxLength(450)
                .IsRequired();

            builder.HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.Property(n => n.ShareSecureHash)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(n => n.Email)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(n => n.Description)
                .HasMaxLength(1000);

            builder.Property(n => n.AccessCode)
                .HasMaxLength(10)
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

            builder.Ignore(s => s.TotalCredentialCount);

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
