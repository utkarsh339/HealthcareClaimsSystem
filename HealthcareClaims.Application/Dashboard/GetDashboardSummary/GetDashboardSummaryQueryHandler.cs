using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Application.Dashboard.Common;
using HealthcareClaims.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.Dashboard.GetDashboardSummary;

public sealed class GetDashboardSummaryQueryHandler
    : IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryResponse>
{
    private readonly IApplicationDbContext _context;

    public GetDashboardSummaryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardSummaryResponse> Handle(
        GetDashboardSummaryQuery request,
        CancellationToken cancellationToken)
    {
        var totalPatients = await _context.Patients
            .AsNoTracking()
            .CountAsync(cancellationToken);

        var totalProviders = await _context.Providers
            .AsNoTracking()
            .CountAsync(cancellationToken);

        var activeInsurancePolicies = await _context.InsurancePolicies
            .AsNoTracking()
            .CountAsync(policy => policy.IsActive, cancellationToken);

        var priorAuthorizationBreakdown = await _context.PriorAuthorizations
            .AsNoTracking()
            .GroupBy(priorAuthorization => priorAuthorization.Status)
            .Select(group => new
            {
                Status = group.Key,
                Count = group.Count()
            })
            .ToDictionaryAsync(item => item.Status, item => item.Count, cancellationToken);

        var claimBreakdown = await _context.Claims
            .AsNoTracking()
            .GroupBy(claim => claim.Status)
            .Select(group => new
            {
                Status = group.Key,
                Count = group.Count()
            })
            .ToDictionaryAsync(item => item.Status, item => item.Count, cancellationToken);

        var claimAmounts = await _context.Claims
            .AsNoTracking()
            .GroupBy(_ => 1)
            .Select(group => new
            {
                TotalBilledAmount = group.Sum(claim => claim.BilledAmount),
                TotalApprovedAmount = group.Sum(claim => claim.ApprovedAmount)
            })
            .FirstOrDefaultAsync(cancellationToken);

        var paymentBreakdown = await _context.Payments
            .AsNoTracking()
            .GroupBy(payment => payment.Status)
            .Select(group => new
            {
                Status = group.Key,
                Count = group.Count()
            })
            .ToDictionaryAsync(item => item.Status, item => item.Count, cancellationToken);

        var totalPaymentAmount = await _context.Payments
            .AsNoTracking()
            .GroupBy(_ => 1)
            .Select(group => group.Sum(payment => payment.Amount))
            .FirstOrDefaultAsync(cancellationToken);

        var priorAuthorizationStatusBreakdown = Enum.GetValues<PriorAuthorizationStatus>()
            .Select(status => new StatusCountResponse
            {
                Status = status.ToString(),
                Count = priorAuthorizationBreakdown.GetValueOrDefault(status)
            })
            .ToList();

        var claimStatusBreakdown = Enum.GetValues<ClaimStatus>()
            .Select(status => new StatusCountResponse
            {
                Status = status.ToString(),
                Count = claimBreakdown.GetValueOrDefault(status)
            })
            .ToList();

        var paymentStatusBreakdown = Enum.GetValues<PaymentStatus>()
            .Select(status => new StatusCountResponse
            {
                Status = status.ToString(),
                Count = paymentBreakdown.GetValueOrDefault(status)
            })
            .ToList();

        return new DashboardSummaryResponse
        {
            TotalPatients = totalPatients,
            TotalProviders = totalProviders,
            ActiveInsurancePolicies = activeInsurancePolicies,
            PriorAuthorizations = new PriorAuthorizationDashboardResponse
            {
                TotalCount = priorAuthorizationStatusBreakdown.Sum(item => item.Count),
                StatusBreakdown = priorAuthorizationStatusBreakdown
            },
            Claims = new ClaimDashboardResponse
            {
                TotalCount = claimStatusBreakdown.Sum(item => item.Count),
                TotalBilledAmount = claimAmounts?.TotalBilledAmount ?? 0m,
                TotalApprovedAmount = claimAmounts?.TotalApprovedAmount ?? 0m,
                StatusBreakdown = claimStatusBreakdown
            },
            Payments = new PaymentDashboardResponse
            {
                TotalCount = paymentStatusBreakdown.Sum(item => item.Count),
                TotalAmount = totalPaymentAmount,
                StatusBreakdown = paymentStatusBreakdown
            }
        };
    }
}
