namespace HealthcareClaims.Application.Dashboard.Common;

public class DashboardSummaryResponse
{
    public int TotalPatients { get; set; }

    public int TotalProviders { get; set; }

    public int ActiveInsurancePolicies { get; set; }

    public PriorAuthorizationDashboardResponse PriorAuthorizations { get; set; } = new();

    public ClaimDashboardResponse Claims { get; set; } = new();

    public PaymentDashboardResponse Payments { get; set; } = new();
}
