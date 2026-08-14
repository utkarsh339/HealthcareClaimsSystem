namespace HealthcareClaims.Application.Dashboard.Common;

public class PaymentDashboardResponse
{
    public IReadOnlyList<StatusCountResponse> StatusBreakdown { get; set; } = [];

    public int TotalCount { get; set; }

    public decimal TotalAmount { get; set; }
}
