using HealthcareClaims.Application.Dashboard.Common;
using MediatR;

namespace HealthcareClaims.Application.Dashboard.GetDashboardSummary;

public sealed record GetDashboardSummaryQuery : IRequest<DashboardSummaryResponse>;
