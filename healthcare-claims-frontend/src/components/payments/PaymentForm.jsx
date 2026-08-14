import { useEffect, useMemo, useState } from "react";

export const PAYMENT_METHODS = Object.freeze({
  BankTransfer: "Bank Transfer",
  Cheque: "Cheque",
  CreditCard: "Credit Card",
  DebitCard: "Debit Card",
  UPI: "UPI",
});

export const PAYMENT_STATUSES = Object.freeze({
  Pending: "Pending",
  Processing: "Processing",
  Completed: "Completed",
  Failed: "Failed",
  Cancelled: "Cancelled",
});

const EMPTY_FORM = {
  claimId: "",
  amount: "",
  paymentMethod: "BankTransfer",
  status: "Pending",
  paidDate: "",
};

function toDateInput(value) {
  if (!value) return "";
  return new Date(value).toISOString().slice(0, 10);
}

function PaymentForm({ payment, claims, isSubmitting, onSubmit, onCancel }) {
  const [form, setForm] = useState(EMPTY_FORM);
  const [errors, setErrors] = useState({});

  useEffect(() => {
    if (payment) {
      setForm({
        claimId: payment.claimId ?? "",
        amount: payment.amount ?? "",
        paymentMethod: payment.paymentMethod ?? "BankTransfer",
        status: payment.status ?? "Pending",
        paidDate: toDateInput(payment.paidDate),
      });
    } else {
      setForm(EMPTY_FORM);
    }
    setErrors({});
  }, [payment]);

  const availableClaims = useMemo(() => {
    if (payment) return claims;
    return claims.filter((claim) => claim.status === "Approved");
  }, [claims, payment]);

  const selectedClaim = useMemo(
    () => claims.find((claim) => claim.id === form.claimId),
    [claims, form.claimId],
  );

  const updateField = (field, value) => {
    setForm((current) => ({ ...current, [field]: value }));
    setErrors((current) => ({ ...current, [field]: "" }));
  };

  const validate = () => {
    const nextErrors = {};
    if (!form.claimId) nextErrors.claimId = "Select a claim.";
    if (!form.amount || Number(form.amount) <= 0) {
      nextErrors.amount = "Amount must be greater than 0.";
    } else if (selectedClaim && Number(form.amount) > Number(selectedClaim.approvedAmount ?? 0)) {
      nextErrors.amount = "Payment amount cannot exceed the approved claim amount.";
    }
    if (!Object.prototype.hasOwnProperty.call(PAYMENT_METHODS, form.paymentMethod)) {
      nextErrors.paymentMethod = "Select a valid payment method.";
    }
    if (!Object.prototype.hasOwnProperty.call(PAYMENT_STATUSES, form.status)) {
      nextErrors.status = "Select a valid payment status.";
    }
    if (form.status === "Completed" && selectedClaim && Number(form.amount) !== Number(selectedClaim.approvedAmount ?? 0)) {
      nextErrors.amount = "A completed payment must equal the approved claim amount.";
    }
    setErrors(nextErrors);
    return Object.keys(nextErrors).length === 0;
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    if (!validate()) return;

    const payload = payment
      ? {
          amount: Number(form.amount),
          paymentMethod: form.paymentMethod,
          status: form.status,
          paidDate: form.paidDate ? new Date(`${form.paidDate}T00:00:00`).toISOString() : null,
        }
      : {
          claimId: form.claimId,
          amount: Number(form.amount),
          paymentMethod: form.paymentMethod,
        };

    onSubmit(payload);
  };

  return (
    <form className="patient-form" onSubmit={handleSubmit} noValidate>
      <div className="patient-form-grid">
        <div className="checkbox-field patient-form-full-width">
          <label htmlFor="payment-claim">Claim</label>
          {payment ? (
            <input id="payment-claim" value={payment.claimNumber || form.claimId} disabled />
          ) : (
            <select id="payment-claim" value={form.claimId} onChange={(event) => updateField("claimId", event.target.value)} disabled={isSubmitting}>
              <option value="">Select approved claim</option>
              {availableClaims.map((claim) => (
                <option key={claim.id} value={claim.id}>
                  {claim.claimNumber} — Patient: {claim.patientId}
                </option>
              ))}
            </select>
          )}
          {errors.claimId && <span className="form-error">{errors.claimId}</span>}
          {!payment && availableClaims.length === 0 && <span className="form-error">No approved claims are currently available for payment.</span>}
        </div>

        <div className="checkbox-field">
          <label htmlFor="payment-amount">Amount</label>
          <input id="payment-amount" type="number" min="0.01" max={selectedClaim?.approvedAmount ?? undefined} step="0.01" value={form.amount} onChange={(event) => updateField("amount", event.target.value)} disabled={isSubmitting} placeholder="0.00" />
          {selectedClaim && <span className="patient-secondary">Approved claim amount: ₹{Number(selectedClaim.approvedAmount ?? 0).toLocaleString("en-IN", { minimumFractionDigits: 2 })}</span>}
          {errors.amount && <span className="form-error">{errors.amount}</span>}
        </div>

        <div className="checkbox-field">
          <label htmlFor="payment-method">Payment method</label>
          <select id="payment-method" value={form.paymentMethod} onChange={(event) => updateField("paymentMethod", event.target.value)} disabled={isSubmitting}>
            {Object.entries(PAYMENT_METHODS).map(([value, label]) => <option key={value} value={value}>{label}</option>)}
          </select>
          {errors.paymentMethod && <span className="form-error">{errors.paymentMethod}</span>}
        </div>

        {payment && (
          <>
            <div className="checkbox-field">
              <label htmlFor="payment-status">Status</label>
              <select id="payment-status" value={form.status} onChange={(event) => updateField("status", event.target.value)} disabled={isSubmitting}>
                {Object.entries(PAYMENT_STATUSES).map(([value, label]) => <option key={value} value={value}>{label}</option>)}
              </select>
              {errors.status && <span className="form-error">{errors.status}</span>}
            </div>

            <div className="checkbox-field">
              <label htmlFor="payment-paid-date">Paid date</label>
              <input id="payment-paid-date" type="date" value={form.paidDate} onChange={(event) => updateField("paidDate", event.target.value)} disabled={isSubmitting} />
            </div>
          </>
        )}
      </div>

      <div className="patient-form-actions">
        <button type="button" className="button-secondary" onClick={onCancel} disabled={isSubmitting}>Cancel</button>
        <button type="submit" className="button-primary" disabled={isSubmitting || (!payment && availableClaims.length === 0)}>{isSubmitting ? "Saving..." : payment ? "Update Payment" : "Create Payment"}</button>
      </div>
    </form>
  );
}

export default PaymentForm;
