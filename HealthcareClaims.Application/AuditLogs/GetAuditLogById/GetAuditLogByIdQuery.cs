using HealthcareClaims.Application.AuditLogs.Common;
using MediatR;

namespace HealthcareClaims.Application.AuditLogs.GetAuditLogById;

public record GetAuditLogByIdQuery(Guid Id)
    : IRequest<AuditLogResponse>;