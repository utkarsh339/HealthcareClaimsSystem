using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Application.Dashboard.Common;
using HealthcareClaims.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.Dashboard.GetClaimsStatistics;

public sealed class GetClaimsStatisticsQueryHandler
    : IRequestHandler<GetClaimsStatisticsQuery, ClaimsStatisticsResponse>
{
    private readonly IApplicationDbContext _context;

    public GetClaimsStatisticsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ClaimsStatisticsResponse> Handle(
        GetClaimsStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        var statusCounts = await _context.Claims
            .AsNoTracking()
            .GroupBy(claim => claim.Status)
            .Select(group => new
            {
                Status = group.Key,
                Count = group.Count()
            })
            .ToDictionaryAsync(item => item.Status, item => item.Count, cancellationToken);

        var statusBreakdown = Enum.GetValues<ClaimStatus>()
            .Select(status => new StatusCountResponse
            {
                Status = status.ToString(),
                Count = statusCounts.GetValueOrDefault(status)
            })
            .ToList();

        var amounts = await _context.Claims
            .AsNoTracking()
            .GroupBy(_ => 1)
            .Select(group => new
            {
                TotalBilledAmount = group.Sum(claim => claim.BilledAmount),
                TotalApprovedAmount = group.Sum(claim => claim.ApprovedAmount)
            })
            .FirstOrDefaultAsync(cancellationToken);

        return new ClaimsStatisticsResponse
        {
            TotalClaims = statusBreakdown.Sum(item => item.Count),
            TotalBilledAmount = amounts?.TotalBilledAmount ?? 0m,
            TotalApprovedAmount = amounts?.TotalApprovedAmount ?? 0m,
            StatusBreakdown = statusBreakdown
        };
    }
}
