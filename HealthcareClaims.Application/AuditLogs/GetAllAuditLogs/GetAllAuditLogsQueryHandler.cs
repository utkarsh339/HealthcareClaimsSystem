using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Application.AuditLogs.Common;
using HealthcareClaims.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.AuditLogs.GetAllAuditLogs;

public class GetAllAuditLogsQueryHandler
    : IRequestHandler<GetAllAuditLogsQuery, IReadOnlyList<AuditLogResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetAllAuditLogsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<AuditLogResponse>> Handle(
        GetAllAuditLogsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.AuditLogs
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
.ThenByDescending(x => x.Id)
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
            .ToListAsync(cancellationToken);
    }
}