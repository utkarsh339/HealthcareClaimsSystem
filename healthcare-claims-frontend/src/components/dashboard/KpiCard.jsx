function KpiCard({ label, value, description }) {
  return (
    <article className="dashboard-kpi-card">
      <p className="dashboard-kpi-label">{label}</p>
      <p className="dashboard-kpi-value">{value}</p>
      {description && <p className="dashboard-kpi-description">{description}</p>}
    </article>
  );
}

export default KpiCard;
