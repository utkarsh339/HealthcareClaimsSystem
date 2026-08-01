using HealthcareClaims.Domain.Entities;
using HealthcareClaims.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthcareClaims.Persistence.Configurations;

public class ClaimConfiguration : IEntityTypeConfiguration<Claim>
{
    public void Configure(EntityTypeBuilder<Claim> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.ClaimNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(c => c.ClaimNumber)
            .IsUnique();

        builder.Property(c => c.DiagnosisCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.ProcedureCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.BilledAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(c => c.ApprovedAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(c => c.Status)
            .HasConversion<string>();

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.HasOne(c => c.Patient)
            .WithMany(p => p.Claims)
            .HasForeignKey(c => c.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Provider)
            .WithMany(p => p.Claims)
            .HasForeignKey(c => c.ProviderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.InsurancePolicy)
            .WithMany(i => i.Claims)
            .HasForeignKey(c => c.InsurancePolicyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.PriorAuthorization)
            .WithMany(pa => pa.Claims)
            .HasForeignKey(c => c.PriorAuthorizationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}