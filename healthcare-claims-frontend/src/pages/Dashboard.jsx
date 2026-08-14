import KpiCard from "../components/dashboard/KpiCard";
import StatusBreakdown from "../components/dashboard/StatusBreakdown";
import AmountCard from "../components/dashboard/AmountCard";
import {
  useGetClaimsStatisticsQuery,
  useGetDashboardSummaryQuery,
} from "../features/dashboard/dashboardApi";

function Dashboard() {
  const {
    data: summary,
    isLoading: isSummaryLoading,
    isFetching: isSummaryFetching,
    isError: isSummaryError,
    refetch: refetchSummary,
  } = useGetDashboardSummaryQuery();

  const {
    data: claimsStatistics,
    isLoading: isClaimsLoading,
    isFetching: isClaimsFetching,
    isError: isClaimsError,
    refetch: refetchClaims,
  } = useGetClaimsStatisticsQuery();

  const isLoading = isSummaryLoading || isClaimsLoading;
  const isFetching = isSummaryFetching || isClaimsFetching;
  const isError = isSummaryError || isClaimsError;

  if (isLoading) {
    return (
      <div className="dashboard-state">
        <div className="dashboard-spinner" aria-hidden="true" />
        <h2>Loading dashboard...</h2>
        <p>Fetching the latest healthcare claims metrics.</p>
      </div>
    );
  }

  if (isError || !summary || !claimsStatistics) {
    const handleRetry = () => {
      refetchSummary();
      refetchClaims();
    };

    return (
      <div className="dashboard-state dashboard-error-state">
        <h2>Unable to load dashboard</h2>
        <p>
          The dashboard data could not be retrieved from the API. Check that the
          backend is running and that your session has DashboardReadAccess.
        </p>
        <button type="button" onClick={handleRetry}>
          Retry
        </button>
      </div>
    );
  }

  const claims = summary.claims ?? {};
  const priorAuthorizations = summary.priorAuthorizations ?? {};
  const payments = summary.payments ?? {};

  return (
    <div className="dashboard-page">
      <div className="dashboard-page-header">
        <div>
          <p className="dashboard-eyebrow">Overview</p>
          <h1>Healthcare Claims Dashboard</h1>
          <p className="dashboard-subtitle">
            Monitor patients, providers, authorizations, claims and payments.
          </p>
        </div>

        <div className="dashboard-refresh-status">
          {isFetching ? "Refreshing..." : "Live data"}
        </div>
      </div>

      <section className="dashboard-kpi-grid" aria-label="Key metrics">
        <KpiCard
          label="Total Patients"
          value={summary.totalPatients}
          description="Patients in the system"
        />
        <KpiCard
          label="Total Providers"
          value={summary.totalProviders}
          description="Registered providers"
        />
        <KpiCard
          label="Active Policies"
          value={summary.activeInsurancePolicies}
          description="Currently active policies"
        />
        <KpiCard
          label="Total Claims"
          value={claims.totalCount ?? claimsStatistics.totalClaims}
          description="Claims across all statuses"
        />
      </section>

      <section className="dashboard-amount-grid" aria-label="Financial metrics">
        <AmountCard
          label="Total Billed"
          amount={claims.totalBilledAmount ?? claimsStatistics.totalBilledAmount}
          description="Total billed claim amount"
        />
        <AmountCard
          label="Total Approved"
          amount={
            claims.totalApprovedAmount ?? claimsStatistics.totalApprovedAmount
          }
          description="Total approved claim amount"
        />
        <AmountCard
          label="Total Payments"
          amount={payments.totalAmount}
          description={`${payments.totalCount ?? 0} payment records`}
        />
      </section>

      <section className="dashboard-section-grid">
        <StatusBreakdown
          title="Claims by Status"
          items={claimsStatistics.statusBreakdown ?? claims.statusBreakdown}
          total={claimsStatistics.totalClaims}
        />

        <StatusBreakdown
          title="Prior Authorizations"
          items={priorAuthorizations.statusBreakdown}
          total={priorAuthorizations.totalCount}
        />

        <StatusBreakdown
          title="Payments by Status"
          items={payments.statusBreakdown}
          total={payments.totalCount}
        />
      </section>
    </div>
  );
}

export default Dashboard;
