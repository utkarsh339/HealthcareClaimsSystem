using HealthcareClaims.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthcareClaims.Persistence.Configurations;

public class ProviderConfiguration : IEntityTypeConfiguration<Provider>
{
    public void Configure(EntityTypeBuilder<Provider> builder)
    {
        builder.ToTable("Providers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProviderName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ProviderType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.LicenseNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.ContactPerson)
            .HasMaxLength(100);

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(x => x.Email)
            .HasMaxLength(150);

        builder.Property(x => x.Address)
            .HasMaxLength(500);
    }
}