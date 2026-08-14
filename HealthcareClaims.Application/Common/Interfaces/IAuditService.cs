using HealthcareClaims.Domain.Enums;

namespace HealthcareClaims.Application.Common.Interfaces;

public interface IAuditService
{
    Task LogAsync(
        AuditAction action,
        string entityName,
        Guid? entityId,
        string description,
        Guid? userId = null,
        string? userEmail = null,
        string? ipAddress = null,
        CancellationToken cancellationToken = default);
}