import { useEffect, useMemo, useState } from "react";

const EMPTY_FORM = {
  patientId: "",
  providerId: "",
  insurancePolicyId: "",
  priorAuthorizationId: "",
  diagnosisCode: "",
  procedureCode: "",
  serviceDate: "",
  billedAmount: "",
  remarks: "",
};

const STATUS_LABELS = {
  Draft: "Draft",
  Submitted: "Submitted",
  UnderReview: "Under review",
  Approved: "Approved",
  PartiallyApproved: "Partially approved",
  Denied: "Denied",
  Paid: "Paid",
  Cancelled: "Cancelled",
};

function ClaimForm({
  claim,
  patients,
  providers,
  policies,
  priorAuthorizations,
  isSubmitting,
  onSubmit,
  onCancel,
}) {
  const [form, setForm] = useState(EMPTY_FORM);
  const [clientError, setClientError] = useState("");

  useEffect(() => {
    if (!claim) {
      setForm(EMPTY_FORM);
      setClientError("");
      return;
    }

    setForm({
      patientId: claim.patientId ?? "",
      providerId: claim.providerId ?? "",
      insurancePolicyId: claim.insurancePolicyId ?? "",
      priorAuthorizationId: claim.priorAuthorizationId ?? "",
      diagnosisCode: claim.diagnosisCode ?? "",
      procedureCode: claim.procedureCode ?? "",
      serviceDate: claim.serviceDate ?? "",
      billedAmount: claim.billedAmount ?? "",
      remarks: claim.remarks ?? "",
    });
    setClientError("");
  }, [claim]);

  const patientPolicies = useMemo(
    () => policies.filter((policy) => policy.patientId === form.patientId),
    [policies, form.patientId],
  );

  const matchingAuthorizations = useMemo(
    () => priorAuthorizations.filter(
      (authorization) =>
        authorization.patientId === form.patientId &&
        authorization.providerId === form.providerId &&
        authorization.insurancePolicyId === form.insurancePolicyId,
    ),
    [priorAuthorizations, form.patientId, form.providerId, form.insurancePolicyId],
  );

  const handleChange = (event) => {
    const { name, value } = event.target;
    setForm((current) => {
      if (name === "patientId") {
        return {
          ...current,
          patientId: value,
          insurancePolicyId: "",
          priorAuthorizationId: "",
        };
      }
      if (name === "providerId" || name === "insurancePolicyId") {
        return { ...current, [name]: value, priorAuthorizationId: "" };
      }
      return { ...current, [name]: value };
    });
    setClientError("");
  };

  const handleSubmit = (event) => {
    event.preventDefault();

    if (!claim && !form.patientId) {
      setClientError("Patient is required.");
      return;
    }
    if (!claim && !form.providerId) {
      setClientError("Provider is required.");
      return;
    }
    if (!claim && !form.insurancePolicyId) {
      setClientError("Insurance policy is required.");
      return;
    }
    if (!claim && !form.priorAuthorizationId) {
      setClientError("Prior authorization is required.");
      return;
    }
    if (!form.diagnosisCode.trim()) {
      setClientError("Diagnosis code is required.");
      return;
    }
    if (!form.procedureCode.trim()) {
      setClientError("Procedure code is required.");
      return;
    }
    if (!form.serviceDate) {
      setClientError("Service date is required.");
      return;
    }
    if (!form.billedAmount || Number(form.billedAmount) <= 0) {
      setClientError("Billed amount must be greater than zero.");
      return;
    }

    const payload = {
      diagnosisCode: form.diagnosisCode.trim(),
      procedureCode: form.procedureCode.trim(),
      serviceDate: form.serviceDate,
      billedAmount: Number(form.billedAmount),
      remarks: form.remarks.trim() || null,
    };

    if (!claim) {
      payload.patientId = form.patientId;
      payload.providerId = form.providerId;
      payload.insurancePolicyId = form.insurancePolicyId;
      payload.priorAuthorizationId = form.priorAuthorizationId;
    }

    onSubmit(payload);
  };

  return (
    <form className="patient-form" onSubmit={handleSubmit}>
      {claim && (
        <div className="detail-strip">
          <div><span>Claim number</span><strong>{claim.claimNumber}</strong></div>
          <div><span>Status</span><strong>{STATUS_LABELS[claim.status] || claim.status}</strong></div>
          <div><span>Approved amount</span><strong>₹{Number(claim.approvedAmount ?? 0).toLocaleString("en-IN", { minimumFractionDigits: 2 })}</strong></div>
        </div>
      )}

      <div className="patient-form-grid">
        {!claim && (
          <>
            <label>
              Patient
              <select name="patientId" value={form.patientId} onChange={handleChange}>
                <option value="">Select patient</option>
                {patients.map((patient) => (
                  <option key={patient.id} value={patient.id}>{patient.firstName} {patient.lastName}</option>
                ))}
              </select>
            </label>

            <label>
              Provider
              <select name="providerId" value={form.providerId} onChange={handleChange}>
                <option value="">Select provider</option>
                {providers.map((provider) => (
                  <option key={provider.id} value={provider.id}>{provider.providerName}{provider.npi ? ` — NPI ${provider.npi}` : ""}</option>
                ))}
              </select>
            </label>

            <label>
              Insurance policy
              <select name="insurancePolicyId" value={form.insurancePolicyId} onChange={handleChange} disabled={!form.patientId}>
                <option value="">{form.patientId ? "Select insurance policy" : "Select patient first"}</option>
                {patientPolicies.map((policy) => (
                  <option key={policy.id} value={policy.id}>{policy.insuranceCompany} — {policy.policyNumber} ({policy.memberId})</option>
                ))}
              </select>
            </label>

            <label>
              Prior authorization
              <select name="priorAuthorizationId" value={form.priorAuthorizationId} onChange={handleChange} disabled={!form.patientId || !form.providerId || !form.insurancePolicyId}>
                <option value="">{form.patientId && form.providerId && form.insurancePolicyId ? "Select authorization" : "Select patient, provider and policy first"}</option>
                {matchingAuthorizations.map((authorization) => (
                  <option key={authorization.id} value={authorization.id}>
                    {authorization.diagnosisCode} / {authorization.procedureCode} — {STATUS_LABELS[authorization.status] || authorization.status}
                  </option>
                ))}
              </select>
            </label>
          </>
        )}

        <label>
          Diagnosis code
          <input name="diagnosisCode" value={form.diagnosisCode} onChange={handleChange} maxLength={50} autoFocus />
        </label>

        <label>
          Procedure code
          <input name="procedureCode" value={form.procedureCode} onChange={handleChange} maxLength={50} />
        </label>

        <label>
          Service date
          <input type="date" name="serviceDate" value={form.serviceDate} onChange={handleChange} />
        </label>

        <label>
          Billed amount
          <input type="number" name="billedAmount" value={form.billedAmount} onChange={handleChange} min="0.01" step="0.01" inputMode="decimal" />
        </label>

        <label className="patient-form-full-width">
          Remarks
          <textarea name="remarks" value={form.remarks} onChange={handleChange} maxLength={1000} rows={4} placeholder="Optional claim notes..." />
        </label>
      </div>

      {clientError && <div className="form-error" role="alert">{clientError}</div>}

      {!claim && form.patientId && form.insurancePolicyId && matchingAuthorizations.length === 0 && (
        <div className="page-alert" role="status">No prior authorization matches the selected patient, provider and insurance policy.</div>
      )}

      <div className="patient-form-actions">
        <button type="button" className="button-secondary" onClick={onCancel} disabled={isSubmitting}>Cancel</button>
        <button type="submit" className="button-primary" disabled={isSubmitting || (!claim && (patients.length === 0 || providers.length === 0 || patientPolicies.length === 0 || matchingAuthorizations.length === 0))}>
          {isSubmitting ? "Saving..." : claim ? "Update Claim" : "Create Claim"}
        </button>
      </div>
    </form>
  );
}

export { STATUS_LABELS };
export default ClaimForm;
