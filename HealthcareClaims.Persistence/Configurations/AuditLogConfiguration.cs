using HealthcareClaims.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthcareClaims.Persistence.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserEmail)
            .HasMaxLength(256);

        builder.Property(x => x.Action)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.EntityName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.IpAddress)
            .HasMaxLength(45);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasIndex(x => x.CreatedAt);

        builder.HasIndex(x => x.EntityName);

        builder.HasIndex(x => x.UserId);

        builder.HasIndex(x => new
        {
            x.EntityName,
            x.EntityId
        });
    }
}