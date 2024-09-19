using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using OpenCredentialPublisher.Data.Custom.EFModels;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;

namespace OpenCredentialPublisher.Data.Custom.EntityTypeConfigurations
{
    public class AuthorizationConfiguration : IEntityTypeConfiguration<Authorization>
    {
        public void Configure(EntityTypeBuilder<Authorization> builder)
        {
            builder.ToTable(nameof(Authorization), EntityTypeConfigurationConstants.SCHEMA_V2);

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()")
                .HasMaxLength(40)
                .IsRequired();

            builder.Property(a => a.AccessToken)
                .IsRequired(false);

            builder.Property(a => a.AuthorizationCode)
                .IsRequired(false);

            builder.Property(a => a.CodeVerifier)
                .IsRequired(false);

            builder.Property(a => a.Endpoint)
                .IsRequired(false);

            builder.Property(a => a.Method)
                .IsRequired(false);

            builder.Property(a => a.Payload)
                .IsRequired(false);

            builder.Property(a => a.RefreshToken)
                .IsRequired(false);

            // Configure Scopes property with ValueConverter and ValueComparer
            var stringListConverter = new ValueConverter<List<string>, string>(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

            var stringListComparer = new ValueComparer<List<string>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList());

            builder.Property(a => a.Scopes)
                .HasConversion(stringListConverter)
                .Metadata.SetValueComparer(stringListComparer);

            builder.Property(a => a.ValidTo)
                .IsRequired();

            builder.Property(a => a.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(a => a.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            builder.Property(a => a.ModifiedAt)
                .IsRequired(false);

            builder.Property(a => a.UserId)
                .HasMaxLength(450)
                .IsRequired();

            builder.HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Source)
                .WithMany()
                .HasForeignKey(a => a.SourceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(a => !a.IsDeleted);
        }
    }
}
