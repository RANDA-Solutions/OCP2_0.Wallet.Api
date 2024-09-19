using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Data.Custom.EntityTypeConfigurations
{
    public class SearchCredentialCollectionConfiguration : IEntityTypeConfiguration<SearchCredentialCollection>
    {
        public void Configure(EntityTypeBuilder<SearchCredentialCollection> builder)
        {
            builder.ToView($"{nameof(SearchCredentialCollection)}View", EntityTypeConfigurationConstants.SCHEMA_V2)
                .HasKey(scp => scp.CredentialCollectionId);
        }
    }
}
