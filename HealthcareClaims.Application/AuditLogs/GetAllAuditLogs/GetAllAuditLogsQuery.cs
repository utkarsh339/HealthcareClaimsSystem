using HealthcareClaims.Application.AuditLogs.Common;
using MediatR;

namespace HealthcareClaims.Application.AuditLogs.GetAllAuditLogs;

public record GetAllAuditLogsQuery : IRequest<IReadOnlyList<AuditLogResponse>>;