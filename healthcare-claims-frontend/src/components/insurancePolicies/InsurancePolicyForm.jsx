import { useEffect, useState } from "react";

const EMPTY_POLICY = {
  patientId: "",
  insuranceCompany: "",
  policyNumber: "",
  memberId: "",
  groupNumber: "",
  planName: "",
  effectiveDate: "",
  expirationDate: "",
  isActive: true,
};

function InsurancePolicyForm({ policy, patients, isSubmitting, onSubmit, onCancel }) {
  const [form, setForm] = useState(EMPTY_POLICY);
  const [clientError, setClientError] = useState("");

  useEffect(() => {
    if (!policy) {
      setForm(EMPTY_POLICY);
      setClientError("");
      return;
    }

    setForm({
      patientId: policy.patientId ?? "",
      insuranceCompany: policy.insuranceCompany ?? "",
      policyNumber: policy.policyNumber ?? "",
      memberId: policy.memberId ?? "",
      groupNumber: policy.groupNumber ?? "",
      planName: policy.planName ?? "",
      effectiveDate: policy.effectiveDate ?? "",
      expirationDate: policy.expirationDate ?? "",
      isActive: policy.isActive ?? true,
    });
    setClientError("");
  }, [policy]);

  const handleChange = (event) => {
    const { name, value, type, checked } = event.target;
    setForm((current) => ({ ...current, [name]: type === "checkbox" ? checked : value }));
    setClientError("");
  };

  const handleSubmit = (event) => {
    event.preventDefault();

    if (!policy && !form.patientId) {
      setClientError("Patient is required.");
      return;
    }
    if (!form.insuranceCompany.trim()) {
      setClientError("Insurance company is required.");
      return;
    }
    if (!form.policyNumber.trim()) {
      setClientError("Policy number is required.");
      return;
    }
    if (!form.memberId.trim()) {
      setClientError("Member ID is required.");
      return;
    }
    if (!form.effectiveDate || !form.expirationDate) {
      setClientError("Effective and expiration dates are required.");
      return;
    }
    if (form.expirationDate <= form.effectiveDate) {
      setClientError("Expiration date must be later than the effective date.");
      return;
    }

    const payload = {
      insuranceCompany: form.insuranceCompany.trim(),
      policyNumber: form.policyNumber.trim(),
      memberId: form.memberId.trim(),
      groupNumber: form.groupNumber.trim(),
      planName: form.planName.trim(),
      effectiveDate: form.effectiveDate,
      expirationDate: form.expirationDate,
      isActive: form.isActive,
    };

    if (!policy) payload.patientId = form.patientId;

    onSubmit(payload);
  };

  return (
    <form className="patient-form" onSubmit={handleSubmit}>
      <div className="patient-form-grid">
        {!policy && (
          <label>
            Patient
            <select name="patientId" value={form.patientId} onChange={handleChange}>
              <option value="">Select patient</option>
              {patients.map((patient) => (
                <option key={patient.id} value={patient.id}>
                  {patient.firstName} {patient.lastName} — {patient.email || "No email"}
                </option>
              ))}
            </select>
          </label>
        )}

        <label>
          Insurance company
          <input name="insuranceCompany" value={form.insuranceCompany} onChange={handleChange} maxLength={200} autoFocus={!!policy} />
        </label>

        <label>
          Policy number
          <input name="policyNumber" value={form.policyNumber} onChange={handleChange} maxLength={100} />
        </label>

        <label>
          Member ID
          <input name="memberId" value={form.memberId} onChange={handleChange} maxLength={100} />
        </label>

        <label>
          Group number
          <input name="groupNumber" value={form.groupNumber} onChange={handleChange} maxLength={100} />
        </label>

        <label>
          Plan name
          <input name="planName" value={form.planName} onChange={handleChange} maxLength={150} />
        </label>

        <label>
          Effective date
          <input name="effectiveDate" type="date" value={form.effectiveDate} onChange={handleChange} />
        </label>

        <label>
          Expiration date
          <input name="expirationDate" type="date" value={form.expirationDate} onChange={handleChange} />
        </label>

        {policy && (
          <label className="checkbox-field">
            <span>Status</span>
            <span className="checkbox-control">
              <input name="isActive" type="checkbox" checked={form.isActive} onChange={handleChange} />
              Active policy
            </span>
          </label>
        )}
      </div>

      {clientError && <div className="form-error" role="alert">{clientError}</div>}

      <div className="patient-form-actions">
        <button type="button" className="button-secondary" onClick={onCancel} disabled={isSubmitting}>Cancel</button>
        <button type="submit" className="button-primary" disabled={isSubmitting || (!policy && patients.length === 0)}>
          {isSubmitting ? "Saving..." : policy ? "Update Policy" : "Create Policy"}
        </button>
      </div>
    </form>
  );
}

export default InsurancePolicyForm;
