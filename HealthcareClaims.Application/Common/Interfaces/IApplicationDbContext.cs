using HealthcareClaims.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }

    DbSet<Patient> Patients { get; }

    DbSet<Provider> Providers { get; }

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}