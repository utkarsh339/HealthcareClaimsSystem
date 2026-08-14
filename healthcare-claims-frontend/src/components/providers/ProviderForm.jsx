import { useEffect, useState } from "react";

const EMPTY_PROVIDER = {
  providerName: "",
  providerType: "Hospital",
  npi: "",
  email: "",
  phoneNumber: "",
  address: "",
  city: "",
  state: "",
  zipCode: "",
  isActive: true,
};

const PROVIDER_TYPES = [
  "Hospital",
  "Clinic",
  "Physician",
  "Laboratory",
  "Pharmacy",
  "ImagingCenter",
  "Other",
];

function ProviderForm({ provider, isSubmitting, onSubmit, onCancel }) {
  const [form, setForm] = useState(EMPTY_PROVIDER);
  const [clientError, setClientError] = useState("");

  useEffect(() => {
    if (!provider) {
      setForm(EMPTY_PROVIDER);
      setClientError("");
      return;
    }

    setForm({
      providerName: provider.providerName ?? "",
      providerType: provider.providerType ?? "Hospital",
      npi: provider.npi ?? "",
      email: provider.email ?? "",
      phoneNumber: provider.phoneNumber ?? "",
      address: provider.address ?? "",
      city: provider.city ?? "",
      state: provider.state ?? "",
      zipCode: provider.zipCode ?? "",
      isActive: provider.isActive ?? true,
    });
    setClientError("");
  }, [provider]);

  const handleChange = (event) => {
    const { name, value, type, checked } = event.target;
    setForm((current) => ({
      ...current,
      [name]: type === "checkbox" ? checked : value,
    }));
    setClientError("");
  };

  const handleSubmit = (event) => {
    event.preventDefault();

    if (!form.providerName.trim()) {
      setClientError("Provider name is required.");
      return;
    }
    if (!form.npi.trim()) {
      setClientError("NPI is required.");
      return;
    }
    if (!form.email.trim() || !/^\S+@\S+\.\S+$/.test(form.email.trim())) {
      setClientError("Please enter a valid email address.");
      return;
    }
    if (!form.phoneNumber.trim()) {
      setClientError("Phone number is required.");
      return;
    }
    if (!form.address.trim() || !form.city.trim() || !form.state.trim() || !form.zipCode.trim()) {
      setClientError("Address, city, state and ZIP code are required.");
      return;
    }

    const payload = {
      providerName: form.providerName.trim(),
      providerType: form.providerType,
      npi: form.npi.trim(),
      email: form.email.trim(),
      phoneNumber: form.phoneNumber.trim(),
      address: form.address.trim(),
      city: form.city.trim(),
      state: form.state.trim(),
      zipCode: form.zipCode.trim(),
    };

    if (provider) {
      payload.isActive = form.isActive;
    }

    onSubmit(payload);
  };

  return (
    <form className="patient-form" onSubmit={handleSubmit}>
      <div className="patient-form-grid">
        <label>
          Provider name
          <input
            name="providerName"
            value={form.providerName}
            onChange={handleChange}
            maxLength={200}
            autoFocus
          />
        </label>

        <label>
          Provider type
          <select name="providerType" value={form.providerType} onChange={handleChange}>
            {PROVIDER_TYPES.map((type) => (
              <option key={type} value={type}>{type}</option>
            ))}
          </select>
        </label>

        <label>
          NPI
          <input name="npi" value={form.npi} onChange={handleChange} maxLength={20} />
        </label>

        <label>
          Email
          <input name="email" type="email" value={form.email} onChange={handleChange} maxLength={254} />
        </label>

        <label>
          Phone number
          <input name="phoneNumber" value={form.phoneNumber} onChange={handleChange} maxLength={20} />
        </label>

        <label>
          ZIP code
          <input name="zipCode" value={form.zipCode} onChange={handleChange} maxLength={20} />
        </label>

        <label className="patient-form-full-width">
          Address
          <textarea name="address" value={form.address} onChange={handleChange} maxLength={500} rows={3} />
        </label>

        <label>
          City
          <input name="city" value={form.city} onChange={handleChange} maxLength={100} />
        </label>

        <label>
          State
          <input name="state" value={form.state} onChange={handleChange} maxLength={100} />
        </label>

        {provider && (
          <label className="checkbox-field">
            <span>Status</span>
            <span className="checkbox-control">
              <input name="isActive" type="checkbox" checked={form.isActive} onChange={handleChange} />
              Active provider
            </span>
          </label>
        )}
      </div>

      {clientError && <div className="form-error" role="alert">{clientError}</div>}

      <div className="patient-form-actions">
        <button type="button" className="button-secondary" onClick={onCancel} disabled={isSubmitting}>
          Cancel
        </button>
        <button type="submit" className="button-primary" disabled={isSubmitting}>
          {isSubmitting ? "Saving..." : provider ? "Update Provider" : "Create Provider"}
        </button>
      </div>
    </form>
  );
}

export default ProviderForm;
