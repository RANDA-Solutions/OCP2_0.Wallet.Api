using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Data.Custom.EntityTypeConfigurations
{
    public class SearchCredentialPackageAchievementTypeConfiguration : IEntityTypeConfiguration<SearchCredentialPackageAchievementType>
    {
        public void Configure(EntityTypeBuilder<SearchCredentialPackageAchievementType> builder)
        {
            builder.ToView($"{nameof(SearchCredentialPackageAchievementType)}View", EntityTypeConfigurationConstants.SCHEMA_V2)
                .HasKey(ct => ct.CredentialPackageId);

        }
    }
}