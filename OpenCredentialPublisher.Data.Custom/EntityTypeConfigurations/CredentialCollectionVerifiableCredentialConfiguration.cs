using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Data.Custom.EntityTypeConfigurations
{
    public class CredentialCollectionVerifiableCredentialConfiguration : IEntityTypeConfiguration<CredentialCollectionVerifiableCredential>
    {
        public void Configure(EntityTypeBuilder<CredentialCollectionVerifiableCredential> builder)
        {
            builder.ToTable(nameof(CredentialCollectionVerifiableCredential), EntityTypeConfigurationConstants.SCHEMA_V2);
            builder.HasKey(ccvc => new { ccvc.CredentialCollectionId, ccvc.VerifiableCredentialId });

            builder.Property(ccvc => ccvc.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");


            builder.Property(ccvc => ccvc.ModifiedAt)
                .IsRequired(false);

            builder.Property(ccvc => ccvc.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            // Explicitly define the foreign keys
            builder.HasOne(ccvc => ccvc.CredentialCollection)
                .WithMany(cc => cc.CredentialCollectionVerifiableCredentials)
                .HasForeignKey(ccvc => ccvc.CredentialCollectionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ccvc => ccvc.VerifiableCredential)
                .WithMany(vc => vc.CredentialCollectionVerifiableCredentials)
                .HasForeignKey(ccvc => ccvc.VerifiableCredentialId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(ccvc => !ccvc.IsDeleted);
        }
    }
}