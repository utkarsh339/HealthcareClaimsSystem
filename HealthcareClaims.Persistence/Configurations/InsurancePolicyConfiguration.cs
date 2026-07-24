using HealthcareClaims.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthcareClaims.Persistence.Configurations;

public class InsurancePolicyConfiguration : IEntityTypeConfiguration<InsurancePolicy>
{
    public void Configure(EntityTypeBuilder<InsurancePolicy> builder)
    {
        builder.ToTable("InsurancePolicies");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PolicyNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.InsuranceCompany)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.PlanName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.CoveragePercentage)
            .HasPrecision(5, 2);

        builder.Property(x => x.DeductibleAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.OutOfPocketMaximum)
            .HasPrecision(18, 2);

        builder.HasOne(x => x.Patient)
            .WithMany()
            .HasForeignKey(x => x.PatientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}