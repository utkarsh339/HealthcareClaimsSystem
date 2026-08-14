import { useEffect, useState } from "react";

const EMPTY_PATIENT = {
  firstName: "",
  lastName: "",
  dateOfBirth: "",
  gender: "Male",
  email: "",
  phoneNumber: "",
  address: "",
};

function PatientForm({ patient, isSubmitting, onSubmit, onCancel }) {
  const [form, setForm] = useState(EMPTY_PATIENT);
  const [clientError, setClientError] = useState("");

  useEffect(() => {
    if (!patient) {
      setForm(EMPTY_PATIENT);
      return;
    }

    setForm({
      firstName: patient.firstName ?? "",
      lastName: patient.lastName ?? "",
      dateOfBirth: patient.dateOfBirth ?? "",
      gender: patient.gender ?? "Male",
      email: patient.email ?? "",
      phoneNumber: patient.phoneNumber ?? "",
      address: patient.address ?? "",
    });
  }, [patient]);

  const handleChange = (event) => {
    const { name, value } = event.target;
    setForm((current) => ({ ...current, [name]: value }));
    setClientError("");
  };

  const handleSubmit = (event) => {
    event.preventDefault();

    if (!form.firstName.trim() || !form.lastName.trim()) {
      setClientError("First name and last name are required.");
      return;
    }

    if (!form.dateOfBirth) {
      setClientError("Date of birth is required.");
      return;
    }

    const today = new Date().toISOString().slice(0, 10);
    if (form.dateOfBirth >= today) {
      setClientError("Date of birth must be in the past.");
      return;
    }

    if (form.email && !/^\S+@\S+\.\S+$/.test(form.email)) {
      setClientError("Please enter a valid email address.");
      return;
    }

    onSubmit({
      firstName: form.firstName.trim(),
      lastName: form.lastName.trim(),
      dateOfBirth: form.dateOfBirth,
      gender: form.gender,
      email: form.email.trim(),
      phoneNumber: form.phoneNumber.trim(),
      address: form.address.trim(),
    });
  };

  return (
    <form className="patient-form" onSubmit={handleSubmit}>
      <div className="patient-form-grid">
        <label>
          First name
          <input name="firstName" value={form.firstName} onChange={handleChange} maxLength={100} autoFocus />
        </label>

        <label>
          Last name
          <input name="lastName" value={form.lastName} onChange={handleChange} maxLength={100} />
        </label>

        <label>
          Date of birth
          <input name="dateOfBirth" type="date" value={form.dateOfBirth} onChange={handleChange} />
        </label>

        <label>
          Gender
          <select name="gender" value={form.gender} onChange={handleChange}>
            <option value="Male">Male</option>
            <option value="Female">Female</option>
            <option value="Other">Other</option>
          </select>
        </label>

        <label>
          Email
          <input name="email" type="email" value={form.email} onChange={handleChange} maxLength={254} />
        </label>

        <label>
          Phone number
          <input name="phoneNumber" value={form.phoneNumber} onChange={handleChange} maxLength={20} />
        </label>

        <label className="patient-form-full-width">
          Address
          <textarea name="address" value={form.address} onChange={handleChange} maxLength={500} rows={3} />
        </label>
      </div>

      {clientError && <div className="form-error" role="alert">{clientError}</div>}

      <div className="patient-form-actions">
        <button type="button" className="button-secondary" onClick={onCancel} disabled={isSubmitting}>
          Cancel
        </button>
        <button type="submit" className="button-primary" disabled={isSubmitting}>
          {isSubmitting ? "Saving..." : patient ? "Update Patient" : "Create Patient"}
        </button>
      </div>
    </form>
  );
}

export default PatientForm;
