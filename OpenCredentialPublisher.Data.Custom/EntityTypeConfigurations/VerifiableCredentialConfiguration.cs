using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Data.Custom.EntityTypeConfigurations
{
    public class VerifiableCredentialConfiguration : IEntityTypeConfiguration<VerifiableCredential>
    {
        public void Configure(EntityTypeBuilder<VerifiableCredential> builder)
        {
            builder.ToTable(nameof(VerifiableCredential), EntityTypeConfigurationConstants.SCHEMA_V2);

            builder.HasKey(e => e.VerifiableCredentialId);
            builder.Property(e => e.VerifiableCredentialId)
                .IsRequired()
                .ValueGeneratedOnAdd(); // Configuring as identity column

            builder.Property(e => e.Id)
                .IsRequired()
                .HasMaxLength(450);


            // Configure properties
            builder.Property(e => e.Name).IsRequired(false);
            builder.Property(e => e.Description).IsRequired(false);
            builder.Property(e => e.ValidFromDate)
                .IsRequired()
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");
            builder.Property(e => e.AwardedDate).IsRequired(false);
            builder.Property(e => e.ValidUntilDate).IsRequired(false);
            builder.Property(e => e.IssuerProfileId).IsRequired();
            builder.Property(e => e.CredentialPackageId).IsRequired();
            builder.Property(e => e.Json).IsRequired();
            builder.Property(e => e.Type).IsRequired();
            builder.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(e => e.IsRevoked)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(e => e.RevokedReason).IsRequired(false);

            builder.Property(e => e.IsVerified)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(e => e.ParentVerifiableCredentialId)
                .IsRequired(false);

            builder.HasOne(vc => vc.ParentVerifiableCredential)
                .WithMany()
                .HasForeignKey(vc => vc.ParentVerifiableCredentialId)
                .OnDelete(DeleteBehavior.Restrict); // Define delete behavior as needed

            builder.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            builder.Property(e => e.ModifiedAt)
                .IsRequired(false);

            builder.Property(e => e.ImageUrl)
                .IsRequired(false);

            builder.HasOne(e => e.IssuerProfile)
                .WithMany() // Assuming a one-to-many relationship
                .HasForeignKey(e => e.IssuerProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(vc => vc.CredentialPackage)
                .WithMany(cp => cp.VerifiableCredentials)
                .HasForeignKey(vc => vc.CredentialPackageId)
                .OnDelete(DeleteBehavior.Restrict); // Define delete behavior as needed

            builder.HasOne(e => e.Achievement)
                .WithOne(a => a.VerifiableCredential)
                .HasForeignKey<Achievement>(a => a.VerifiableCredentialId)
                .OnDelete(DeleteBehavior.Restrict); // Define delete behavior as needed

            // Configure many-to-many relationship
            builder
                .HasMany(vc => vc.CredentialCollectionVerifiableCredentials)
                .WithOne(ccvc => ccvc.VerifiableCredential)
                .HasForeignKey(ccvc => ccvc.VerifiableCredentialId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.Evidences)
                .WithOne(aa => aa.VerifiableCredential)
                .HasForeignKey(aa => aa.VerifiableCredentialId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasMany(vc => vc.Results)
                .WithOne(r => r.VerifiableCredential)
                .HasForeignKey(r => r.VerifiableCredentialId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Ignore(e => e.EffectiveAt);
            builder.Ignore(e => e.EffectiveImageUrl);

            builder.HasQueryFilter(x => !x.IsDeleted);

        }
    }
}