using HealthcareClaims.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }

    DbSet<Patient> Patients { get; }

    DbSet<Provider> Providers { get; }

    DbSet<InsurancePolicy> InsurancePolicies { get; }

    DbSet<PriorAuthorization> PriorAuthorizations { get; }

    DbSet<Claim> Claims { get; }

    DbSet<Payment> Payments { get; }

    DbSet<AuditLog> AuditLogs { get; }

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}