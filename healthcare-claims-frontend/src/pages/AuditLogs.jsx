import { getErrorMessage } from "../utils_errorMessage";
import { useMemo, useState } from "react";
import { useGetAuditLogsQuery, useLazyGetAuditLogByIdQuery } from "../features/auditLogs/auditLogsApi";

const ACTION_LABELS = {
  Create: "Create",
  Update: "Update",
  Delete: "Delete",
  Login: "Login",
  Logout: "Logout",
  Approve: "Approve",
  Reject: "Reject",
  Submit: "Submit",
  PaymentProcessed: "Payment Processed",
};

function formatDate(value) {
  if (!value) return "—";
  return new Intl.DateTimeFormat("en-IN", {
    day: "2-digit",
    month: "short",
    year: "numeric",
    hour: "2-digit",
    minute: "2-digit",
  }).format(new Date(value));
}

function formatAction(action) {
  return ACTION_LABELS[action] || action || "Unknown";
}

function AuditLogs() {
  const {
    data: auditLogs = [],
    isLoading,
    isFetching,
    isError,
    error,
    refetch,
  } = useGetAuditLogsQuery();
  const [getAuditLogById] = useLazyGetAuditLogByIdQuery();

  const [search, setSearch] = useState("");
  const [actionFilter, setActionFilter] = useState("All");
  const [entityFilter, setEntityFilter] = useState("All");
  const [selectedLog, setSelectedLog] = useState(null);
  const [detailError, setDetailError] = useState("");
  const [isDetailLoading, setIsDetailLoading] = useState(false);

  const actions = useMemo(
    () => [...new Set(auditLogs.map((log) => log.action).filter(Boolean))].sort(),
    [auditLogs],
  );

  const entities = useMemo(
    () => [...new Set(auditLogs.map((log) => log.entityName).filter(Boolean))].sort(),
    [auditLogs],
  );

  const filteredLogs = useMemo(() => {
    const query = search.trim().toLowerCase();

    return auditLogs.filter((log) => {
      const matchesAction = actionFilter === "All" || log.action === actionFilter;
      const matchesEntity = entityFilter === "All" || log.entityName === entityFilter;
      const matchesSearch = !query || [
        log.userEmail,
        log.entityName,
        log.entityId,
        log.description,
        log.ipAddress,
        formatAction(log.action),
        formatDate(log.createdAt),
      ].some((value) => String(value ?? "").toLowerCase().includes(query));

      return matchesAction && matchesEntity && matchesSearch;
    });
  }, [auditLogs, search, actionFilter, entityFilter]);

  const openDetails = async (id) => {
    setDetailError("");
    setIsDetailLoading(true);
    try {
      const log = await getAuditLogById(id).unwrap();
      setSelectedLog(log);
    } catch (requestError) {
      setDetailError(getErrorMessage(requestError, "Unable to load the audit log."));
    } finally {
      setIsDetailLoading(false);
    }
  };

  if (isLoading) {
    return (
      <div className="page-state">
        <div className="page-spinner" />
        <h2>Loading audit logs...</h2>
      </div>
    );
  }

  if (isError) {
    return (
      <div className="page-state">
        <h2>Unable to load audit logs</h2>
        <p>{getErrorMessage(error, "The Audit Logs API could not be reached.")}</p>
        <button type="button" className="button-primary" onClick={refetch}>Retry</button>
      </div>
    );
  }

  return (
    <section className="patients-page">
      <div className="patients-header">
        <div>
          <div className="page-eyebrow">Security & Compliance</div>
          <h1>Audit Logs</h1>
          <p>Review immutable business and authentication activity recorded by the system.</p>
        </div>
        <div className="audit-summary-card">
          <strong>{auditLogs.length}</strong>
          <span>Total events</span>
        </div>
      </div>

      {detailError && <div className="page-alert" role="alert">{detailError}</div>}

      <div className="patients-toolbar audit-toolbar">
        <input
          type="search"
          value={search}
          onChange={(event) => setSearch(event.target.value)}
          placeholder="Search user, entity, description, IP address..."
          aria-label="Search audit logs"
        />
        <select value={actionFilter} onChange={(event) => setActionFilter(event.target.value)} aria-label="Filter by action">
          <option value="All">All actions</option>
          {actions.map((action) => <option key={action} value={action}>{formatAction(action)}</option>)}
        </select>
        <select value={entityFilter} onChange={(event) => setEntityFilter(event.target.value)} aria-label="Filter by entity">
          <option value="All">All entities</option>
          {entities.map((entity) => <option key={entity} value={entity}>{entity}</option>)}
        </select>
        <span>{filteredLogs.length} event{filteredLogs.length === 1 ? "" : "s"}</span>
        {isFetching && <span className="sync-status">Refreshing...</span>}
      </div>

      <div className="patients-table-card">
        {filteredLogs.length === 0 ? (
          <div className="empty-state">
            <h2>{auditLogs.length === 0 ? "No audit logs yet" : "No matching audit logs"}</h2>
            <p>
              {auditLogs.length === 0
                ? "Audit activity will appear here as users authenticate and perform business operations."
                : "Try changing the search text or filters."}
            </p>
          </div>
        ) : (
          <div className="table-wrapper">
            <table className="patients-table">
              <thead>
                <tr>
                  <th>Timestamp</th>
                  <th>User</th>
                  <th>Action</th>
                  <th>Entity</th>
                  <th>Description</th>
                  <th>IP Address</th>
                  <th className="actions-column">Details</th>
                </tr>
              </thead>
              <tbody>
                {filteredLogs.map((log) => (
                  <tr key={log.id}>
                    <td>{formatDate(log.createdAt)}</td>
                    <td>
                      <div className="patient-name">{log.userEmail || "System"}</div>
                      {log.userId && <div className="patient-secondary">{log.userId}</div>}
                    </td>
                    <td><span className="status-pill">{formatAction(log.action)}</span></td>
                    <td>
                      <div className="patient-name">{log.entityName || "—"}</div>
                      {log.entityId && <div className="patient-secondary">{log.entityId}</div>}
                    </td>
                    <td>{log.description || "—"}</td>
                    <td>{log.ipAddress || "—"}</td>
                    <td className="actions-column">
                      <button type="button" className="button-link" onClick={() => openDetails(log.id)}>
                        View
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>

      {(selectedLog || isDetailLoading) && (
        <div className="modal-backdrop" role="presentation" onMouseDown={(event) => { if (event.target === event.currentTarget) setSelectedLog(null); }}>
          <div className="patient-modal" role="dialog" aria-modal="true" aria-labelledby="audit-modal-title">
            <div className="patient-modal-header">
              <div>
                <div className="page-eyebrow">Audit Event</div>
                <h2 id="audit-modal-title">Event Details</h2>
              </div>
              <button type="button" className="modal-close" onClick={() => setSelectedLog(null)} aria-label="Close">×</button>
            </div>

            {isDetailLoading ? (
              <div className="empty-state"><div className="page-spinner" /><p>Loading event...</p></div>
            ) : (
              <div className="audit-detail-grid">
                <div><span>Action</span><strong>{formatAction(selectedLog.action)}</strong></div>
                <div><span>Timestamp</span><strong>{formatDate(selectedLog.createdAt)}</strong></div>
                <div><span>User</span><strong>{selectedLog.userEmail || "System"}</strong></div>
                <div><span>IP Address</span><strong>{selectedLog.ipAddress || "—"}</strong></div>
                <div><span>Entity</span><strong>{selectedLog.entityName || "—"}</strong></div>
                <div><span>Entity ID</span><strong>{selectedLog.entityId || "—"}</strong></div>
                <div className="audit-detail-wide"><span>Description</span><p>{selectedLog.description || "No description provided."}</p></div>
                <div className="audit-detail-wide"><span>Audit Log ID</span><p>{selectedLog.id}</p></div>
              </div>
            )}
          </div>
        </div>
      )}
    </section>
  );
}

export default AuditLogs;
