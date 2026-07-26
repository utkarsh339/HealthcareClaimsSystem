using HealthcareClaims.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}