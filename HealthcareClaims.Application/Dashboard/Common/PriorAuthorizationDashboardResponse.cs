namespace HealthcareClaims.Application.Dashboard.Common;

public class PriorAuthorizationDashboardResponse
{
    public IReadOnlyList<StatusCountResponse> StatusBreakdown { get; set; } = [];

    public int TotalCount { get; set; }
}
