using HealthcareClaims.Domain.Entities;
using HealthcareClaims.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthcareClaims.Persistence.Configurations;

public class PriorAuthorizationConfiguration : IEntityTypeConfiguration<PriorAuthorization>
{
    public void Configure(EntityTypeBuilder<PriorAuthorization> builder)
    {
        builder.ToTable("PriorAuthorizations");

        builder.HasKey(pa => pa.Id);

        builder.Property(pa => pa.DiagnosisCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(pa => pa.ProcedureCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(pa => pa.ServiceDescription)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(pa => pa.Status)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(40);

        builder.Property(pa => pa.ReviewNotes)
            .HasMaxLength(1000);

        builder.HasOne(pa => pa.Patient)
            .WithMany(p => p.PriorAuthorizations)
            .HasForeignKey(pa => pa.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pa => pa.Provider)
            .WithMany(p => p.PriorAuthorizations)
            .HasForeignKey(pa => pa.ProviderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pa => pa.InsurancePolicy)
            .WithMany(ip => ip.PriorAuthorizations)
            .HasForeignKey(pa => pa.InsurancePolicyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(pa => pa.PatientId);

        builder.HasIndex(pa => pa.ProviderId);

        builder.HasIndex(pa => pa.InsurancePolicyId);

        builder.HasIndex(pa => pa.Status);
    }
}