using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Data.Custom.EntityTypeConfigurations
{
    public class SearchCredentialConfiguration : IEntityTypeConfiguration<SearchCredential>
    {
        public void Configure(EntityTypeBuilder<SearchCredential> builder)
        {
            builder.ToView($"{nameof(SearchCredential)}View", EntityTypeConfigurationConstants.SCHEMA_V2)
                .HasKey(scp => scp.VerifiableCredentialId);

        }
    }
}