using HealthcareClaims.Application.Dashboard.Common;
using MediatR;

namespace HealthcareClaims.Application.Dashboard.GetClaimsStatistics;

public sealed record GetClaimsStatisticsQuery : IRequest<ClaimsStatisticsResponse>;
