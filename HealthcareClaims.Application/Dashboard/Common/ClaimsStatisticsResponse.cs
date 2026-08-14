namespace HealthcareClaims.Application.Dashboard.Common;

public class ClaimsStatisticsResponse
{
    public int TotalClaims { get; set; }

    public decimal TotalBilledAmount { get; set; }

    public decimal TotalApprovedAmount { get; set; }

    public IReadOnlyList<StatusCountResponse> StatusBreakdown { get; set; } = [];
}
