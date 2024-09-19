using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenCredentialPublisher.Data.Custom.EFModels;
using OpenCredentialPublisher.Data.Models;

namespace OpenCredentialPublisher.Data.Custom.EntityTypeConfigurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable(nameof(Message), EntityTypeConfigurationConstants.SCHEMA_V2);
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(m => m.Recipient)
                .IsRequired();

            builder.Property(m => m.Body)
                .IsRequired();

            builder.Property(m => m.Subject)
                .IsRequired();

            builder.Property(m => m.SendAttempts)
                .HasDefaultValue(0);

            builder.Property(m => m.StatusId)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(m => m.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            builder.Property(m => m.ModifiedAt)
                .IsRequired(false);

            builder.Property(m => m.IsDeleted)
                .HasDefaultValue(false);

            builder.HasOne<StatusModel>()
                .WithMany()
                .HasForeignKey(m => m.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(m => !m.IsDeleted);
        }
    }
}
