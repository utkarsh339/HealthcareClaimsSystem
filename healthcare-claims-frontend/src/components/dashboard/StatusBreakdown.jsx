function StatusBreakdown({ title, items = [], total }) {
  const calculatedTotal = items.reduce((sum, item) => sum + item.count, 0);
  const safeTotal = total ?? calculatedTotal;

  return (
    <section className="dashboard-panel">
      <div className="dashboard-panel-header">
        <div>
          <h2>{title}</h2>
          <p>{safeTotal} total</p>
        </div>
      </div>

      <div className="dashboard-status-list">
        {items.map((item) => {
          const percentage = safeTotal > 0 ? (item.count / safeTotal) * 100 : 0;

          return (
            <div className="dashboard-status-row" key={item.status}>
              <div className="dashboard-status-meta">
                <span>{item.status}</span>
                <strong>{item.count}</strong>
              </div>
              <div className="dashboard-progress-track" aria-hidden="true">
                <div
                  className="dashboard-progress-bar"
                  style={{ width: `${percentage}%` }}
                />
              </div>
            </div>
          );
        })}

        {items.length === 0 && (
          <p className="dashboard-empty-state">No status data available.</p>
        )}
      </div>
    </section>
  );
}

export default StatusBreakdown;
