using HealthcareClaims.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthcareClaims.Persistence.Configurations;

public class InsurancePolicyConfiguration : IEntityTypeConfiguration<InsurancePolicy>
{
    public void Configure(EntityTypeBuilder<InsurancePolicy> builder)
    {
        builder.ToTable("InsurancePolicies");

        builder.HasKey(ip => ip.Id);

        builder.Property(ip => ip.InsuranceCompany)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(ip => ip.PolicyNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ip => ip.MemberId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ip => ip.GroupNumber)
            .HasMaxLength(100);

        builder.Property(ip => ip.PlanName)
            .HasMaxLength(150);

        builder.Property(ip => ip.IsActive)
            .HasDefaultValue(true);

        builder.HasOne(ip => ip.Patient)
            .WithMany(p => p.InsurancePolicies)
            .HasForeignKey(ip => ip.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ip => ip.PatientId);

        builder.HasIndex(ip => ip.PolicyNumber)
            .IsUnique();
    }
}