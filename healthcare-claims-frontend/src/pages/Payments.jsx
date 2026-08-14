import { getErrorMessage } from "../utils_errorMessage";
import { useMemo, useState } from "react";
import Can from "../components/authorization/Can";
import PaymentForm, { PAYMENT_METHODS, PAYMENT_STATUSES } from "../components/payments/PaymentForm";
import { useGetClaimsQuery } from "../features/claims/claimsApi";
import {
  useCreatePaymentMutation,
  useDeletePaymentMutation,
  useGetPaymentsQuery,
  useLazyGetPaymentByIdQuery,
  useUpdatePaymentMutation,
} from "../features/payments/paymentsApi";

function formatDate(value) {
  if (!value) return "—";
  return new Intl.DateTimeFormat("en-IN", { day: "2-digit", month: "short", year: "numeric" }).format(new Date(value));
}

function formatAmount(value) {
  return `₹${Number(value ?? 0).toLocaleString("en-IN", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`;
}

function Payments() {
  const { data: payments = [], isLoading, isFetching, isError, error, refetch } = useGetPaymentsQuery();
  const { data: claims = [], isLoading: claimsLoading } = useGetClaimsQuery();
  const [getById] = useLazyGetPaymentByIdQuery();
  const [createPayment, { isLoading: isCreating }] = useCreatePaymentMutation();
  const [updatePayment, { isLoading: isUpdating }] = useUpdatePaymentMutation();
  const [deletePayment, { isLoading: isDeleting }] = useDeletePaymentMutation();

  const [search, setSearch] = useState("");
  const [isFormOpen, setIsFormOpen] = useState(false);
  const [editingPayment, setEditingPayment] = useState(null);
  const [formError, setFormError] = useState("");
  const [deletingId, setDeletingId] = useState(null);

  const paidClaimIds = useMemo(() => new Set(payments.map((payment) => payment.claimId)), [payments]);
  const availableCreateClaims = useMemo(() => claims.filter((claim) => claim.status === "Approved" && !paidClaimIds.has(claim.id)), [claims, paidClaimIds]);

  const filteredPayments = useMemo(() => {
    const query = search.trim().toLowerCase();
    if (!query) return payments;
    return payments.filter((payment) => [
      payment.claimNumber,
      payment.paymentReference,
      PAYMENT_METHODS[payment.paymentMethod] || payment.paymentMethod,
      PAYMENT_STATUSES[payment.status] || payment.status,
      payment.amount,
      formatDate(payment.paidDate),
    ].some((value) => String(value ?? "").toLowerCase().includes(query)));
  }, [payments, search]);

  const openCreateForm = () => {
    setEditingPayment(null);
    setFormError("");
    setIsFormOpen(true);
  };

  const openEditForm = async (id) => {
    setFormError("");
    try {
      const payment = await getById(id).unwrap();
      setEditingPayment(payment);
      setIsFormOpen(true);
    } catch (requestError) {
      setFormError(getErrorMessage(requestError, "Unable to load the payment."));
    }
  };

  const closeForm = () => {
    if (isCreating || isUpdating) return;
    setIsFormOpen(false);
    setEditingPayment(null);
    setFormError("");
  };

  const handleSubmit = async (payload) => {
    setFormError("");
    try {
      if (editingPayment) {
        await updatePayment({ id: editingPayment.id, ...payload }).unwrap();
      } else {
        await createPayment(payload).unwrap();
      }
      closeForm();
    } catch (requestError) {
      setFormError(getErrorMessage(requestError, "Unable to save the payment."));
    }
  };

  const handleDelete = async (payment) => {
    if (payment.status === "Completed") {
      setFormError("Completed payments cannot be deleted.");
      return;
    }
    if (!window.confirm(`Delete payment ${payment.paymentReference}? This action cannot be undone.`)) return;
    setDeletingId(payment.id);
    setFormError("");
    try {
      await deletePayment(payment.id).unwrap();
    } catch (requestError) {
      setFormError(getErrorMessage(requestError, "Unable to delete the payment."));
    } finally {
      setDeletingId(null);
    }
  };

  if (isLoading) {
    return <div className="page-state"><div className="page-spinner" /><h2>Loading payments...</h2></div>;
  }

  if (isError) {
    return <div className="page-state"><h2>Unable to load payments</h2><p>{getErrorMessage(error, "The Payments API could not be reached.")}</p><button type="button" className="button-primary" onClick={refetch}>Retry</button></div>;
  }

  return (
    <section className="patients-page">
      <div className="patients-header">
        <div>
          <div className="page-eyebrow">Payment Processing</div>
          <h1>Payments</h1>
          <p>Manage payments created for approved claims and track their processing status.</p>
        </div>
        <Can permission="PaymentWrite">
          <button type="button" className="button-primary" onClick={openCreateForm} disabled={claimsLoading || availableCreateClaims.length === 0}>+ Add Payment</button>
        </Can>
      </div>

      {formError && !isFormOpen && <div className="page-alert" role="alert">{formError}</div>}

      <div className="patients-toolbar">
        <input type="search" value={search} onChange={(event) => setSearch(event.target.value)} placeholder="Search by claim, payment reference, method or status..." aria-label="Search payments" />
        <span>{filteredPayments.length} payment{filteredPayments.length === 1 ? "" : "s"}</span>
        {isFetching && <span className="sync-status">Refreshing...</span>}
      </div>

      <div className="patients-table-card">
        {filteredPayments.length === 0 ? (
          <div className="empty-state">
            <h2>{search ? "No matching payments" : "No payments yet"}</h2>
            <p>{search ? "Try a different search term." : "Create a payment for an approved claim to get started."}</p>
            {!search && <Can permission="PaymentWrite"><button type="button" className="button-primary" onClick={openCreateForm} disabled={claimsLoading || availableCreateClaims.length === 0}>Add Payment</button></Can>}
            {!search && !claimsLoading && availableCreateClaims.length === 0 && <p>No approved claims without an existing payment are currently available.</p>}
          </div>
        ) : (
          <div className="table-wrapper">
            <table className="patients-table">
              <thead><tr><th>Payment</th><th>Claim</th><th>Amount</th><th>Method</th><th>Status</th><th>Paid Date</th><th className="actions-column">Actions</th></tr></thead>
              <tbody>
                {filteredPayments.map((payment) => (
                  <tr key={payment.id}>
                    <td><div className="patient-name">{payment.paymentReference}</div><div className="patient-secondary">Created {formatDate(payment.createdAt)}</div></td>
                    <td><div className="patient-name">{payment.claimNumber}</div><div className="patient-secondary">ID: {payment.claimId}</div></td>
                    <td><div className="patient-name">{formatAmount(payment.amount)}</div></td>
                    <td>{PAYMENT_METHODS[payment.paymentMethod] || payment.paymentMethod}</td>
                    <td><span className="status-pill">{PAYMENT_STATUSES[payment.status] || payment.status}</span></td>
                    <td>{formatDate(payment.paidDate)}</td>
                    <td className="actions-column"><Can permission="PaymentWrite"><div className="table-actions"><button type="button" className="button-link" onClick={() => openEditForm(payment.id)}>Edit</button><button type="button" className="button-link button-danger-link" disabled={payment.status === "Completed" || deletingId === payment.id || isDeleting} onClick={() => handleDelete(payment)}>{deletingId === payment.id ? "Deleting..." : "Delete"}</button></div></Can></td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>

      {isFormOpen && (
        <div className="modal-backdrop" role="presentation" onMouseDown={(event) => { if (event.target === event.currentTarget) closeForm(); }}>
          <div className="patient-modal" role="dialog" aria-modal="true" aria-labelledby="payment-modal-title">
            <div className="patient-modal-header"><div><div className="page-eyebrow">Payment Record</div><h2 id="payment-modal-title">{editingPayment ? "Update Payment" : "Add Payment"}</h2></div><button type="button" className="modal-close" onClick={closeForm} disabled={isCreating || isUpdating} aria-label="Close">×</button></div>
            {formError && <div className="page-alert" role="alert">{formError}</div>}
            {claimsLoading && !editingPayment ? <div className="empty-state"><p>Loading claims...</p></div> : <PaymentForm payment={editingPayment} claims={editingPayment ? claims : availableCreateClaims} isSubmitting={isCreating || isUpdating} onSubmit={handleSubmit} onCancel={closeForm} />}
          </div>
        </div>
      )}
    </section>
  );
}

export default Payments;
