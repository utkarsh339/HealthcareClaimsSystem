using HealthcareClaims.Application.AuditLogs.Common;
using HealthcareClaims.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.AuditLogs.GetAuditLogById;

public class GetAuditLogByIdQueryHandler
    : IRequestHandler<GetAuditLogByIdQuery, AuditLogResponse>
{
    private readonly IApplicationDbContext _context;

    public GetAuditLogByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AuditLogResponse> Handle(
        GetAuditLogByIdQuery request,
        CancellationToken cancellationToken)
    {
        var auditLog = await _context.AuditLogs
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => new AuditLogResponse
            {
                Id = x.Id,
                UserId = x.UserId,
                UserEmail = x.UserEmail,
                Action = x.Action,
                EntityName = x.EntityName,
                EntityId = x.EntityId,
                Description = x.Description,
                IpAddress = x.IpAddress,
                CreatedAt = x.CreatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (auditLog is null)
        {
            throw new KeyNotFoundException("Audit log not found.");
        }

        return auditLog;
    }
}