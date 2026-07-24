using HealthcareClaims.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthcareClaims.Persistence.Configurations;

public class PriorAuthorizationConfiguration : IEntityTypeConfiguration<PriorAuthorization>
{
    public void Configure(EntityTypeBuilder<PriorAuthorization> builder)
    {
        builder.ToTable("PriorAuthorizations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AuthorizationNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.RequestedProcedureCode)
            .HasMaxLength(50);

        builder.Property(x => x.DiagnosisCode)
            .HasMaxLength(50);

        builder.Property(x => x.Status)
            .HasMaxLength(50);

        builder.Property(x => x.ReviewerComments)
            .HasMaxLength(1000);

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