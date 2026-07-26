using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Persistence.Context;

public class ApplicationDbContext
    : DbContext,
      IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Patient> Patients => Set<Patient>();

    public DbSet<InsurancePolicy> InsurancePolicies => Set<InsurancePolicy>();

    public DbSet<Provider> Providers => Set<Provider>();

    public DbSet<Claim> Claims => Set<Claim>();

    public DbSet<PriorAuthorization> PriorAuthorizations => Set<PriorAuthorization>();

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}