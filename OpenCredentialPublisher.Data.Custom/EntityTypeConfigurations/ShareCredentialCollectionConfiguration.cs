using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Data.Custom.EntityTypeConfigurations
{
    public class ShareCredentialCollectionConfiguration : IEntityTypeConfiguration<ShareCredentialCollection>
    {
        public void Configure(EntityTypeBuilder<ShareCredentialCollection> builder)
        {
            builder.ToTable(nameof(ShareCredentialCollection), EntityTypeConfigurationConstants.SCHEMA_V2);

            builder.HasKey(scc => new { scc.ShareId, scc.CredentialCollectionId });

            builder.Property(ccvc => ccvc.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");


            builder.Property(ccvc => ccvc.ModifiedAt)
                .IsRequired(false);

            builder.Property(ccvc => ccvc.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            // Explicitly define the foreign keys
            builder.HasOne(scc => scc.CredentialCollection)
                .WithMany(cc => cc.ShareCredentialCollections)
                .HasForeignKey(scc => scc.CredentialCollectionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(scc => scc.Share)
                .WithMany(s => s.ShareCredentialCollections)
                .HasForeignKey(scc => scc.ShareId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(ccvc => !ccvc.IsDeleted);
        }
    }
}