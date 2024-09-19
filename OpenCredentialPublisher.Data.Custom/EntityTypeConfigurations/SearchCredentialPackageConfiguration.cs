using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Data.Custom.EntityTypeConfigurations
{
    public class SearchCredentialPackageConfiguration : IEntityTypeConfiguration<SearchCredentialPackage>
    {
        public void Configure(EntityTypeBuilder<SearchCredentialPackage> builder)
        {
            builder.ToView($"{nameof(SearchCredentialPackage)}View", EntityTypeConfigurationConstants.SCHEMA_V2)
                .HasKey(scp => scp.CredentialPackageId);

            builder.HasMany(s => s.Issuers)
                .WithOne()
                .HasForeignKey(i => i.CredentialPackageId)
                .IsRequired();

            builder.HasMany(s => s.AchievementTypes)
                .WithOne()
                .HasForeignKey(c => c.CredentialPackageId)
                .IsRequired();
        }
    }
}
