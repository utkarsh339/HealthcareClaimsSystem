using HealthcareClaims.Domain.Enums;

namespace HealthcareClaims.Application.AuditLogs.Common;

public class AuditLogResponse
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public string? UserEmail { get; set; }

    public AuditAction Action { get; set; }

    public string EntityName { get; set; } = string.Empty;

    public Guid? EntityId { get; set; }

    public string Description { get; set; } = string.Empty;

    public string? IpAddress { get; set; }

    public DateTime CreatedAt { get; set; }
}