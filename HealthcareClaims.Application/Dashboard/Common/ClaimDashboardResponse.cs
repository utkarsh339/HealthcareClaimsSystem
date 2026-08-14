namespace HealthcareClaims.Application.Dashboard.Common;

public class ClaimDashboardResponse
{
    public IReadOnlyList<StatusCountResponse> StatusBreakdown { get; set; } = [];

    public int TotalCount { get; set; }

    public decimal TotalBilledAmount { get; set; }

    public decimal TotalApprovedAmount { get; set; }
}
