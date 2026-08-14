import { useEffect, useMemo, useState } from "react";
import { STATUS_LABELS } from "./ClaimForm";

export const CLAIM_STATUS_TRANSITIONS = Object.freeze({
  Draft: ["Submitted", "Cancelled"],
  Submitted: ["UnderReview", "Cancelled"],
  UnderReview: ["Approved", "PartiallyApproved", "Denied", "Cancelled"],
  Approved: ["Cancelled"],
  PartiallyApproved: ["Cancelled"],
  Denied: [],
  Paid: [],
  Cancelled: [],
});

function ClaimStatusForm({ claim, isSubmitting, onSubmit, onCancel }) {
  const options = useMemo(() => CLAIM_STATUS_TRANSITIONS[claim?.status] ?? [], [claim?.status]);
  const [status, setStatus] = useState(options[0] ?? "");
  const [approvedAmount, setApprovedAmount] = useState(
    claim?.approvedAmount > 0 ? claim.approvedAmount : claim?.billedAmount ?? "",
  );
  const [error, setError] = useState("");

  useEffect(() => {
    setStatus(options[0] ?? "");
    setApprovedAmount(claim?.approvedAmount > 0 ? claim.approvedAmount : claim?.billedAmount ?? "");
    setError("");
  }, [claim, options]);

  const needsApprovalAmount = status === "Approved" || status === "PartiallyApproved";

  const handleSubmit = (event) => {
    event.preventDefault();
    setError("");

    if (!status) {
      setError("No valid status transition is available for this claim.");
      return;
    }

    const amount = Number(approvedAmount);
    if (status === "Approved" && (amount <= 0 || amount > Number(claim.billedAmount))) {
      setError("Approved amount must be greater than zero and cannot exceed the billed amount.");
      return;
    }
    if (status === "PartiallyApproved" && (amount <= 0 || amount >= Number(claim.billedAmount))) {
      setError("Partially approved amount must be greater than zero and less than the billed amount.");
      return;
    }

    onSubmit({
      id: claim.id,
      status,
      approvedAmount: needsApprovalAmount ? amount : 0,
    });
  };

  return (
    <form className="patient-form" onSubmit={handleSubmit}>
      <div className="detail-strip">
        <div><span>Claim number</span><strong>{claim.claimNumber}</strong></div>
        <div><span>Current status</span><strong>{STATUS_LABELS[claim.status] || claim.status}</strong></div>
        <div><span>Billed amount</span><strong>₹{Number(claim.billedAmount ?? 0).toLocaleString("en-IN", { minimumFractionDigits: 2 })}</strong></div>
      </div>

      <div className="patient-form-grid">
        <label>
          New status
          <select value={status} onChange={(event) => setStatus(event.target.value)} disabled={isSubmitting}>
            {options.length === 0 && <option value="">No transition available</option>}
            {options.map((value) => <option key={value} value={value}>{STATUS_LABELS[value] || value}</option>)}
          </select>
        </label>

        {needsApprovalAmount && (
          <label>
            Approved amount
            <input
              type="number"
              min="0.01"
              max={claim.billedAmount}
              step="0.01"
              value={approvedAmount}
              onChange={(event) => setApprovedAmount(event.target.value)}
              disabled={isSubmitting}
            />
          </label>
        )}
      </div>

      {status === "Approved" && <div className="page-alert" role="status">This claim can be used to create a payment after approval.</div>}
      {status === "PartiallyApproved" && <div className="page-alert" role="status">A partial approval cannot exceed or equal the billed amount.</div>}
      {error && <div className="form-error" role="alert">{error}</div>}

      <div className="patient-form-actions">
        <button type="button" className="button-secondary" onClick={onCancel} disabled={isSubmitting}>Cancel</button>
        <button type="submit" className="button-primary" disabled={isSubmitting || !status}>
          {isSubmitting ? "Updating..." : "Update Status"}
        </button>
      </div>
    </form>
  );
}

export default ClaimStatusForm;
