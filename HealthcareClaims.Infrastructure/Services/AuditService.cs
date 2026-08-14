using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Domain.Entities;
using HealthcareClaims.Domain.Enums;

namespace HealthcareClaims.Infrastructure.Services;

public class AuditService : IAuditService
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public AuditService(
        IApplicationDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task LogAsync(
        AuditAction action,
        string entityName,
        Guid? entityId,
        string description,
        Guid? userId = null,
        string? userEmail = null,
        string? ipAddress = null,
        CancellationToken cancellationToken = default)
    {
        var auditLog = new AuditLog
        {
            Id = Guid.NewGuid(),
            UserId = userId ?? _currentUser.UserId,
            UserEmail = userEmail ?? _currentUser.Email,
            Action = action,
            EntityName = entityName,
            EntityId = entityId,
            Description = description,
            IpAddress = ipAddress ?? _currentUser.IpAddress,
            CreatedAt = DateTime.UtcNow
        };

        _context.AuditLogs.Add(auditLog);
    }
}