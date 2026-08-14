import { getErrorMessage } from "../utils_errorMessage";
import { useMemo, useState } from "react";
import Can from "../components/authorization/Can";
import ClaimForm, { STATUS_LABELS } from "../components/claims/ClaimForm";
import ClaimStatusForm, { CLAIM_STATUS_TRANSITIONS } from "../components/claims/ClaimStatusForm";
import { useGetPatientsQuery } from "../features/patients/patientsApi";
import { useGetProvidersQuery } from "../features/providers/providersApi";
import { useGetInsurancePoliciesQuery } from "../features/insurancePolicies/insurancePoliciesApi";
import { useGetPriorAuthorizationsQuery } from "../features/priorAuthorizations/priorAuthorizationsApi";
import {
  useCreateClaimMutation,
  useDeleteClaimMutation,
  useGetClaimsQuery,
  useLazyGetClaimByIdQuery,
  useUpdateClaimMutation,
  useUpdateClaimStatusMutation,
} from "../features/claims/claimsApi";

function formatDate(value) {
  if (!value) return "—";
  return new Intl.DateTimeFormat("en-IN", { day: "2-digit", month: "short", year: "numeric" }).format(new Date(value));
}

function formatAmount(value) {
  return `₹${Number(value ?? 0).toLocaleString("en-IN", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`;
}

function displayName(map, id, fallback) {
  return map.get(id) || fallback;
}

function Claims() {
  const { data: claims = [], isLoading, isFetching, isError, error, refetch } = useGetClaimsQuery();
  const { data: patients = [], isLoading: patientsLoading } = useGetPatientsQuery();
  const { data: providers = [], isLoading: providersLoading } = useGetProvidersQuery();
  const { data: policies = [], isLoading: policiesLoading } = useGetInsurancePoliciesQuery();
  const { data: authorizations = [], isLoading: authorizationsLoading } = useGetPriorAuthorizationsQuery();
  const [getById] = useLazyGetClaimByIdQuery();
  const [createClaim, { isLoading: isCreating }] = useCreateClaimMutation();
  const [updateClaim, { isLoading: isUpdating }] = useUpdateClaimMutation();
  const [updateClaimStatus, { isLoading: isUpdatingStatus }] = useUpdateClaimStatusMutation();
  const [deleteClaim, { isLoading: isDeleting }] = useDeleteClaimMutation();

  const [search, setSearch] = useState("");
  const [isFormOpen, setIsFormOpen] = useState(false);
  const [editingClaim, setEditingClaim] = useState(null);
  const [processingClaim, setProcessingClaim] = useState(null);
  const [formError, setFormError] = useState("");
  const [deletingId, setDeletingId] = useState(null);

  const patientMap = useMemo(() => new Map(patients.map((patient) => [patient.id, `${patient.firstName} ${patient.lastName}`])), [patients]);
  const providerMap = useMemo(() => new Map(providers.map((provider) => [provider.id, provider.providerName])), [providers]);
  const policyMap = useMemo(() => new Map(policies.map((policy) => [policy.id, `${policy.insuranceCompany} — ${policy.policyNumber}`])), [policies]);

  const filteredClaims = useMemo(() => {
    const query = search.trim().toLowerCase();
    if (!query) return claims;
    return claims.filter((claim) => [
      claim.claimNumber,
      displayName(patientMap, claim.patientId, "Unknown patient"),
      displayName(providerMap, claim.providerId, "Unknown provider"),
      displayName(policyMap, claim.insurancePolicyId, "Unknown policy"),
      claim.diagnosisCode,
      claim.procedureCode,
      STATUS_LABELS[claim.status] || claim.status,
      claim.remarks,
    ].some((value) => String(value ?? "").toLowerCase().includes(query)));
  }, [claims, search, patientMap, providerMap, policyMap]);

  const openCreateForm = () => {
    setEditingClaim(null);
    setFormError("");
    setIsFormOpen(true);
  };

  const openEditForm = async (id) => {
    setFormError("");
    try {
      const claim = await getById(id).unwrap();
      setEditingClaim(claim);
      setIsFormOpen(true);
    } catch (requestError) {
      setFormError(getErrorMessage(requestError, "Unable to load the claim."));
    }
  };

  const closeForm = () => {
    if (isCreating || isUpdating) return;
    setIsFormOpen(false);
    setEditingClaim(null);
    setFormError("");
  };

  const handleSubmit = async (payload) => {
    setFormError("");
    try {
      if (editingClaim) {
        await updateClaim({ id: editingClaim.id, ...payload }).unwrap();
      } else {
        await createClaim(payload).unwrap();
      }
      closeForm();
    } catch (requestError) {
      setFormError(getErrorMessage(requestError, "Unable to save the claim."));
    }
  };

  const openStatusForm = async (id) => {
    setFormError("");
    try {
      const claim = await getById(id).unwrap();
      setProcessingClaim(claim);
    } catch (requestError) {
      setFormError(getErrorMessage(requestError, "Unable to load the claim."));
    }
  };

  const closeStatusForm = () => {
    if (isUpdatingStatus) return;
    setProcessingClaim(null);
    setFormError("");
  };

  const handleStatusSubmit = async ({ id, status, approvedAmount }) => {
    setFormError("");
    try {
      await updateClaimStatus({ id, status, approvedAmount }).unwrap();
      closeStatusForm();
    } catch (requestError) {
      setFormError(getErrorMessage(requestError, "Unable to update claim status."));
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm("Delete this claim? This action cannot be undone.")) return;
    setDeletingId(id);
    setFormError("");
    try {
      await deleteClaim(id).unwrap();
    } catch (requestError) {
      setFormError(getErrorMessage(requestError, "Unable to delete the claim."));
    } finally {
      setDeletingId(null);
    }
  };

  if (isLoading) {
    return <div className="page-state"><div className="page-spinner" /><h2>Loading claims...</h2></div>;
  }

  if (isError) {
    return <div className="page-state"><h2>Unable to load claims</h2><p>{getErrorMessage(error, "The Claims API could not be reached.")}</p><button type="button" className="button-primary" onClick={refetch}>Retry</button></div>;
  }

  const relatedDataLoading = patientsLoading || providersLoading || policiesLoading || authorizationsLoading;
  const canCreate = !relatedDataLoading && patients.length > 0 && providers.length > 0 && policies.length > 0 && authorizations.length > 0;

  return (
    <section className="patients-page">
      <div className="patients-header">
        <div>
          <div className="page-eyebrow">Claims Processing</div>
          <h1>Claims</h1>
          <p>Manage submitted healthcare claims, billed amounts, clinical codes and processing status.</p>
        </div>
        <Can permission="ClaimWrite">
          <button type="button" className="button-primary" onClick={openCreateForm} disabled={!canCreate}>+ Add Claim</button>
        </Can>
      </div>

      {formError && !isFormOpen && <div className="page-alert" role="alert">{formError}</div>}

      <div className="patients-toolbar">
        <input type="search" value={search} onChange={(event) => setSearch(event.target.value)} placeholder="Search by claim number, patient, provider, code or status..." aria-label="Search claims" />
        <span>{filteredClaims.length} claim{filteredClaims.length === 1 ? "" : "s"}</span>
        {isFetching && <span className="sync-status">Refreshing...</span>}
      </div>

      <div className="patients-table-card">
        {filteredClaims.length === 0 ? (
          <div className="empty-state">
            <h2>{search ? "No matching claims" : "No claims yet"}</h2>
            <p>{search ? "Try a different search term." : "Create the first claim to get started."}</p>
            {!search && <Can permission="ClaimWrite"><button type="button" className="button-primary" onClick={openCreateForm} disabled={!canCreate}>Add Claim</button></Can>}
          </div>
        ) : (
          <div className="table-wrapper">
            <table className="patients-table">
              <thead><tr><th>Claim</th><th>Patient / Provider</th><th>Clinical Codes</th><th>Service</th><th>Amounts</th><th>Status</th><th className="actions-column">Actions</th></tr></thead>
              <tbody>
                {filteredClaims.map((claim) => (
                  <tr key={claim.id}>
                    <td><div className="patient-name">{claim.claimNumber}</div><div className="patient-secondary">Created {formatDate(claim.createdAt)}</div></td>
                    <td><div className="patient-name">{displayName(patientMap, claim.patientId, "Unknown patient")}</div><div className="patient-secondary">Provider: {displayName(providerMap, claim.providerId, "Unknown provider")}</div></td>
                    <td><div className="patient-name">Dx: {claim.diagnosisCode}</div><div className="patient-secondary">Procedure: {claim.procedureCode}</div></td>
                    <td>{formatDate(claim.serviceDate)}</td>
                    <td><div className="patient-name">Billed: {formatAmount(claim.billedAmount)}</div><div className="patient-secondary">Approved: {formatAmount(claim.approvedAmount)}</div></td>
                    <td><span className="status-pill">{STATUS_LABELS[claim.status] || claim.status}</span></td>
                    <td className="actions-column"><Can permission="ClaimWrite"><div className="table-actions">{(claim.status === "Draft" || claim.status === "Submitted") && <button type="button" className="button-link" onClick={() => openEditForm(claim.id)}>Edit</button>}<Can permission="ClaimProcess">{(CLAIM_STATUS_TRANSITIONS[claim.status] || []).length > 0 && <button type="button" className="button-link" onClick={() => openStatusForm(claim.id)}>Process</button>}</Can><button type="button" className="button-link button-danger-link" disabled={deletingId === claim.id || isDeleting} onClick={() => handleDelete(claim.id)}>{deletingId === claim.id ? "Deleting..." : "Delete"}</button></div></Can></td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>

      {isFormOpen && (
        <div className="modal-backdrop" role="presentation" onMouseDown={(event) => { if (event.target === event.currentTarget) closeForm(); }}>
          <div className="patient-modal" role="dialog" aria-modal="true" aria-labelledby="claim-modal-title">
            <div className="patient-modal-header"><div><div className="page-eyebrow">Claim Record</div><h2 id="claim-modal-title">{editingClaim ? "Update Claim" : "Add Claim"}</h2></div><button type="button" className="modal-close" onClick={closeForm} disabled={isCreating || isUpdating} aria-label="Close">×</button></div>
            {formError && <div className="page-alert" role="alert">{formError}</div>}
            {relatedDataLoading && !editingClaim ? <div className="empty-state"><p>Loading patients, providers, insurance policies and prior authorizations...</p></div> : <ClaimForm claim={editingClaim} patients={patients} providers={providers} policies={policies} priorAuthorizations={authorizations} isSubmitting={isCreating || isUpdating} onSubmit={handleSubmit} onCancel={closeForm} />}
          </div>
        </div>
      )}

      {processingClaim && (
        <div className="modal-backdrop" role="presentation" onMouseDown={(event) => { if (event.target === event.currentTarget) closeStatusForm(); }}>
          <div className="patient-modal" role="dialog" aria-modal="true" aria-labelledby="claim-status-modal-title">
            <div className="patient-modal-header"><div><div className="page-eyebrow">Claim Processing</div><h2 id="claim-status-modal-title">Update Claim Status</h2></div><button type="button" className="modal-close" onClick={closeStatusForm} disabled={isUpdatingStatus} aria-label="Close">×</button></div>
            {formError && <div className="page-alert" role="alert">{formError}</div>}
            <ClaimStatusForm claim={processingClaim} isSubmitting={isUpdatingStatus} onSubmit={handleStatusSubmit} onCancel={closeStatusForm} />
          </div>
        </div>
      )}
    </section>
  );
}

export default Claims;
