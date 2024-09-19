using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OpenCredentialPublisher.Data.Custom.EntityTypeConfigurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            builder.Property(e => e.ModifiedAt)
                .IsRequired(false);

            // does not use soft deletes
            builder.Ignore(u => u.IsDeleted);
        }
    }
}
