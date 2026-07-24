using HealthcareClaims.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthcareClaims.Persistence.Configurations;

public class ClaimConfiguration : IEntityTypeConfiguration<Claim>
{
    public void Configure(EntityTypeBuilder<Claim> builder)
    {
        builder.ToTable("Claims");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ClaimNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.TotalAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.ApprovedAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.PatientResponsibilityAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.DiagnosisCode)
            .HasMaxLength(50);

        builder.Property(x => x.ProcedureCode)
            .HasMaxLength(50);

        builder.Property(x => x.Status)
            .HasMaxLength(50);

        builder.HasOne(x => x.Patient)
            .WithMany()
            .HasForeignKey(x => x.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Provider)
            .WithMany()
            .HasForeignKey(x => x.ProviderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.InsurancePolicy)
            .WithMany()
            .HasForeignKey(x => x.InsurancePolicyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}