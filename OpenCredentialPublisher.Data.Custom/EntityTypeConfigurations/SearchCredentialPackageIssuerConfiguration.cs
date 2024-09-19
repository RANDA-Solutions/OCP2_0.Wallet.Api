using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Data.Custom.EntityTypeConfigurations
{
    public class SearchCredentialPackageIssuerConfiguration : IEntityTypeConfiguration<SearchCredentialPackageIssuer>
    {
        public void Configure(EntityTypeBuilder<SearchCredentialPackageIssuer> builder)
        {
            builder.ToView($"{nameof(SearchCredentialPackageIssuer)}View", EntityTypeConfigurationConstants.SCHEMA_V2)
                .HasKey(i => i.CredentialPackageId);
        }
    }
}