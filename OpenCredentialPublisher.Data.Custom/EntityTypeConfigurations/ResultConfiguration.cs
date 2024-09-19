using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Data.Custom.EntityTypeConfigurations
{
    public class ResultConfiguration : IEntityTypeConfiguration<Result>
    {
        public void Configure(EntityTypeBuilder<Result> builder)
        {
            builder.ToTable(nameof(Result), EntityTypeConfigurationConstants.SCHEMA_V2);
            builder.HasKey(e => e.ResultId);

            builder.Property(e => e.ResultId)
                .IsRequired()
                .ValueGeneratedOnAdd(); // Configuring as identity column

            builder.HasOne(s => s.VerifiableCredential)
                .WithMany(vc => vc.Results)
                .HasForeignKey(s => s.VerifiableCredentialId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.Property(n => n.ResultDescriptionName)
                .HasMaxLength(256);

            builder.Property(n => n.ResultDescriptionType)
                .HasMaxLength(256);

            builder.Property(n => n.Status)
                .HasMaxLength(100);

            builder.Property(n => n.Value)
                .HasMaxLength(256);

            builder.Property(n => n.IsDeleted)
                .HasDefaultValue(false);

            builder
                .Property(n => n.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            builder
                .Property(n => n.ModifiedAt)
                .IsRequired(false);

            builder.HasQueryFilter(r => !r.IsDeleted && !r.VerifiableCredential.IsDeleted);
        }
    }
}
