import { useEffect, useMemo, useState } from "react";

const EMPTY_FORM = {
  patientId: "",
  providerId: "",
  insurancePolicyId: "",
  diagnosisCode: "",
  procedureCode: "",
  serviceDescription: "",
  status: "Pending",
  reviewNotes: "",
};

const STATUS_OPTIONS = [
  "Pending",
  "Approved",
  "Denied",
  "MoreInformationRequired",
];

const STATUS_LABELS = {
  Pending: "Pending",
  Approved: "Approved",
  Denied: "Denied",
  MoreInformationRequired: "More information required",
};

function PriorAuthorizationForm({
  priorAuthorization,
  patients,
  providers,
  policies,
  isSubmitting,
  onSubmit,
  onCancel,
}) {
  const [form, setForm] = useState(EMPTY_FORM);
  const [clientError, setClientError] = useState("");

  useEffect(() => {
    if (!priorAuthorization) {
      setForm(EMPTY_FORM);
      setClientError("");
      return;
    }

    setForm({
      patientId: priorAuthorization.patientId ?? "",
      providerId: priorAuthorization.providerId ?? "",
      insurancePolicyId: priorAuthorization.insurancePolicyId ?? "",
      diagnosisCode: priorAuthorization.diagnosisCode ?? "",
      procedureCode: priorAuthorization.procedureCode ?? "",
      serviceDescription: priorAuthorization.serviceDescription ?? "",
      status: priorAuthorization.status ?? "Pending",
      reviewNotes: priorAuthorization.reviewNotes ?? "",
    });
    setClientError("");
  }, [priorAuthorization]);

  const patientPolicies = useMemo(
    () => policies.filter((policy) => policy.patientId === form.patientId),
    [policies, form.patientId],
  );

  const handleChange = (event) => {
    const { name, value } = event.target;
    setForm((current) => {
      if (name === "patientId") {
        return { ...current, patientId: value, insurancePolicyId: "" };
      }
      return { ...current, [name]: value };
    });
    setClientError("");
  };

  const handleSubmit = (event) => {
    event.preventDefault();

    if (!priorAuthorization && !form.patientId) {
      setClientError("Patient is required.");
      return;
    }
    if (!priorAuthorization && !form.providerId) {
      setClientError("Provider is required.");
      return;
    }
    if (!priorAuthorization && !form.insurancePolicyId) {
      setClientError("Insurance policy is required.");
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
    if (!form.serviceDescription.trim()) {
      setClientError("Service description is required.");
      return;
    }

    const payload = {
      diagnosisCode: form.diagnosisCode.trim(),
      procedureCode: form.procedureCode.trim(),
      serviceDescription: form.serviceDescription.trim(),
    };

    if (priorAuthorization) {
      payload.status = form.status;
      payload.reviewNotes = form.reviewNotes.trim() || null;
    } else {
      payload.patientId = form.patientId;
      payload.providerId = form.providerId;
      payload.insurancePolicyId = form.insurancePolicyId;
    }

    onSubmit(payload);
  };

  return (
    <form className="patient-form" onSubmit={handleSubmit}>
      <div className="patient-form-grid">
        {!priorAuthorization && (
          <>
            <label>
              Patient
              <select name="patientId" value={form.patientId} onChange={handleChange}>
                <option value="">Select patient</option>
                {patients.map((patient) => (
                  <option key={patient.id} value={patient.id}>
                    {patient.firstName} {patient.lastName}
                  </option>
                ))}
              </select>
            </label>

            <label>
              Provider
              <select name="providerId" value={form.providerId} onChange={handleChange}>
                <option value="">Select provider</option>
                {providers.map((provider) => (
                  <option key={provider.id} value={provider.id}>
                    {provider.providerName} {provider.npi ? `— NPI ${provider.npi}` : ""}
                  </option>
                ))}
              </select>
            </label>

            <label>
              Insurance policy
              <select
                name="insurancePolicyId"
                value={form.insurancePolicyId}
                onChange={handleChange}
                disabled={!form.patientId}
              >
                <option value="">
                  {form.patientId ? "Select insurance policy" : "Select patient first"}
                </option>
                {patientPolicies.map((policy) => (
                  <option key={policy.id} value={policy.id}>
                    {policy.insuranceCompany} — {policy.policyNumber} ({policy.memberId})
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

        <label className="patient-form-full-width">
          Service description
          <textarea name="serviceDescription" value={form.serviceDescription} onChange={handleChange} maxLength={500} rows={3} />
        </label>

        {priorAuthorization && (
          <>
            <label>
              Status
              <select name="status" value={form.status} onChange={handleChange}>
                {STATUS_OPTIONS.map((status) => (
                  <option key={status} value={status}>
                    {STATUS_LABELS[status]}
                  </option>
                ))}
              </select>
            </label>

            <label className="patient-form-full-width">
              Review notes
              <textarea name="reviewNotes" value={form.reviewNotes} onChange={handleChange} maxLength={1000} rows={4} placeholder="Add reviewer notes or rationale..." />
            </label>
          </>
        )}
      </div>

      {clientError && <div className="form-error" role="alert">{clientError}</div>}

      {!priorAuthorization && form.patientId && patientPolicies.length === 0 && (
        <div className="page-alert" role="status">The selected patient has no insurance policies available for a prior authorization.</div>
      )}

      <div className="patient-form-actions">
        <button type="button" className="button-secondary" onClick={onCancel} disabled={isSubmitting}>Cancel</button>
        <button type="submit" className="button-primary" disabled={isSubmitting || (!priorAuthorization && (patients.length === 0 || providers.length === 0 || patientPolicies.length === 0))}>
          {isSubmitting ? "Saving..." : priorAuthorization ? "Update Prior Authorization" : "Create Prior Authorization"}
        </button>
      </div>
    </form>
  );
}

export { STATUS_LABELS };
export default PriorAuthorizationForm;
