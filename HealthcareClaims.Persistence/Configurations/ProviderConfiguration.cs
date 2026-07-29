using HealthcareClaims.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthcareClaims.Persistence.Configurations;

public class ProviderConfiguration : IEntityTypeConfiguration<Provider>
{
    public void Configure(EntityTypeBuilder<Provider> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.ProviderName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.ProviderType)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(p => p.NPI)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(p => p.NPI)
            .IsUnique();

        builder.Property(p => p.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(p => p.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.State)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.ZipCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.IsActive)
            .HasDefaultValue(true);

        builder.Property(p => p.CreatedAt)
            .IsRequired();
    }
}