using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Data.Custom.EntityTypeConfigurations
{
    public class SetupConfiguration : IEntityTypeConfiguration<Setup>
    {
        public void Configure(EntityTypeBuilder<Setup> builder)
        {
            builder.ToTable(nameof(Setup), EntityTypeConfigurationConstants.SCHEMA_V2);

            builder.HasKey(e => e.SetupId);

            builder.Property(e => e.SetupId)
                .IsRequired()
                .ValueGeneratedOnAdd(); // Configuring as identity column

            builder.Property(n => n.AccessCode)
                .HasMaxLength(10);

            builder.Property(n => n.UserId)
                .HasMaxLength(450);

            builder.Property(n => n.MessageId)
                .IsRequired(false);

            builder.Property(n => n.IsDeleted)
                .HasDefaultValue(false);

            builder.HasOne<Message>()
                .WithMany()
                .HasForeignKey(n => n.MessageId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            builder.HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();


            builder
                .Property(n => n.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            builder
                .Property(n => n.ModifiedAt)
                .IsRequired(false);

            // Configuring the optional relationship with Message
            builder.HasOne(sl => sl.Message)
                .WithMany()
                .HasForeignKey(sl => sl.MessageId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
