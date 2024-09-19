using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Data.Custom.EntityTypeConfigurations
{
    public class ShareVerifiableCredentialConfiguration : IEntityTypeConfiguration<ShareVerifiableCredential>
    {
        public void Configure(EntityTypeBuilder<ShareVerifiableCredential> builder)
        {
            builder.ToTable(nameof(ShareVerifiableCredential), EntityTypeConfigurationConstants.SCHEMA_V2);

            builder.HasKey(ccvc => new { ccvc.ShareId, ccvc.VerifiableCredentialId });

            builder.Property(ccvc => ccvc.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");


            builder.Property(ccvc => ccvc.ModifiedAt)
                .IsRequired(false);

            builder.Property(ccvc => ccvc.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            // Explicitly define the foreign keys
            builder.HasOne(svc => svc.VerifiableCredential)
                .WithMany(vc => vc.ShareVerifiableCredentials)
                .HasForeignKey(svc => svc.VerifiableCredentialId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(svc => svc.Share)
                .WithMany(s => s.ShareVerifiableCredentials)
                .HasForeignKey(svc => svc.ShareId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(ccvc => !ccvc.IsDeleted);
        }
    }
}