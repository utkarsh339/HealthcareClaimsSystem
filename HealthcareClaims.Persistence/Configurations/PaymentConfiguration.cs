using HealthcareClaims.Domain.Entities;
using HealthcareClaims.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthcareClaims.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Amount)
            .HasPrecision(18, 2);

        builder.Property(p => p.PaymentReference)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.PaymentMethod)
            .HasConversion<string>();

        builder.Property(p => p.Status)
            .HasConversion<string>();

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.HasOne(p => p.Claim)
            .WithOne(c => c.Payment)
            .HasForeignKey<Payment>(p => p.ClaimId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}