import { getErrorMessage } from "../utils_errorMessage";
import { useMemo, useState } from "react";
import Can from "../components/authorization/Can";
import PriorAuthorizationForm, { STATUS_LABELS } from "../components/priorAuthorizations/PriorAuthorizationForm";
import { useGetPatientsQuery } from "../features/patients/patientsApi";
import { useGetProvidersQuery } from "../features/providers/providersApi";
import { useGetInsurancePoliciesQuery } from "../features/insurancePolicies/insurancePoliciesApi";
import {
  useCreatePriorAuthorizationMutation,
  useDeletePriorAuthorizationMutation,
  useGetPriorAuthorizationsQuery,
  useLazyGetPriorAuthorizationByIdQuery,
  useUpdatePriorAuthorizationMutation,
} from "../features/priorAuthorizations/priorAuthorizationsApi";

function formatDate(value) {
  if (!value) return "—";
  return new Intl.DateTimeFormat("en-IN", {
    day: "2-digit",
    month: "short",
    year: "numeric",
  }).format(new Date(value));
}

function displayName(map, id, fallback) {
  return map.get(id) || fallback;
}

function PriorAuthorizations() {
  const { data: authorizations = [], isLoading, isFetching, isError, error, refetch } = useGetPriorAuthorizationsQuery();
  const { data: patients = [], isLoading: patientsLoading } = useGetPatientsQuery();
  const { data: providers = [], isLoading: providersLoading } = useGetProvidersQuery();
  const { data: policies = [], isLoading: policiesLoading } = useGetInsurancePoliciesQuery();
  const [getById] = useLazyGetPriorAuthorizationByIdQuery();
  const [createPriorAuthorization, { isLoading: isCreating }] = useCreatePriorAuthorizationMutation();
  const [updatePriorAuthorization, { isLoading: isUpdating }] = useUpdatePriorAuthorizationMutation();
  const [deletePriorAuthorization, { isLoading: isDeleting }] = useDeletePriorAuthorizationMutation();

  const [search, setSearch] = useState("");
  const [isFormOpen, setIsFormOpen] = useState(false);
  const [editingAuthorization, setEditingAuthorization] = useState(null);
  const [formError, setFormError] = useState("");
  const [deletingId, setDeletingId] = useState(null);

  const patientMap = useMemo(
    () => new Map(patients.map((patient) => [patient.id, `${patient.firstName} ${patient.lastName}`])),
    [patients],
  );
  const providerMap = useMemo(
    () => new Map(providers.map((provider) => [provider.id, provider.providerName])),
    [providers],
  );
  const policyMap = useMemo(
    () => new Map(policies.map((policy) => [policy.id, `${policy.insuranceCompany} — ${policy.policyNumber}`])),
    [policies],
  );

  const filteredAuthorizations = useMemo(() => {
    const query = search.trim().toLowerCase();
    if (!query) return authorizations;

    return authorizations.filter((authorization) => [
      displayName(patientMap, authorization.patientId, "Unknown patient"),
      displayName(providerMap, authorization.providerId, "Unknown provider"),
      displayName(policyMap, authorization.insurancePolicyId, "Unknown policy"),
      authorization.diagnosisCode,
      authorization.procedureCode,
      authorization.serviceDescription,
      STATUS_LABELS[authorization.status] || authorization.status,
      authorization.reviewNotes,
    ].some((value) => String(value ?? "").toLowerCase().includes(query)));
  }, [authorizations, search, patientMap, providerMap, policyMap]);

  const openCreateForm = () => {
    setEditingAuthorization(null);
    setFormError("");
    setIsFormOpen(true);
  };

  const openEditForm = async (id) => {
    setFormError("");
    try {
      const authorization = await getById(id).unwrap();
      setEditingAuthorization(authorization);
      setIsFormOpen(true);
    } catch (requestError) {
      setFormError(getErrorMessage(requestError, "Unable to load the prior authorization."));
    }
  };

  const closeForm = () => {
    if (isCreating || isUpdating) return;
    setIsFormOpen(false);
    setEditingAuthorization(null);
    setFormError("");
  };

  const handleSubmit = async (payload) => {
    setFormError("");
    try {
      if (editingAuthorization) {
        await updatePriorAuthorization({ id: editingAuthorization.id, ...payload }).unwrap();
      } else {
        await createPriorAuthorization(payload).unwrap();
      }
      closeForm();
    } catch (requestError) {
      setFormError(getErrorMessage(requestError, "Unable to save the prior authorization."));
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm("Delete this prior authorization? This action cannot be undone.")) return;
    setDeletingId(id);
    setFormError("");
    try {
      await deletePriorAuthorization(id).unwrap();
    } catch (requestError) {
      setFormError(getErrorMessage(requestError, "Unable to delete the prior authorization."));
    } finally {
      setDeletingId(null);
    }
  };

  if (isLoading) {
    return <div className="page-state"><div className="page-spinner" /><h2>Loading prior authorizations...</h2></div>;
  }

  if (isError) {
    return (
      <div className="page-state">
        <h2>Unable to load prior authorizations</h2>
        <p>{getErrorMessage(error, "The Prior Authorization API could not be reached.")}</p>
        <button type="button" className="button-primary" onClick={refetch}>Retry</button>
      </div>
    );
  }

  const relatedDataLoading = patientsLoading || providersLoading || policiesLoading;

  return (
    <section className="patients-page">
      <div className="patients-header">
        <div>
          <div className="page-eyebrow">Clinical Authorization</div>
          <h1>Prior Authorizations</h1>
          <p>Manage authorization requests, clinical codes, review decisions and supporting notes.</p>
        </div>
        <Can permission="PriorAuthorizationWrite">
          <button type="button" className="button-primary" onClick={openCreateForm} disabled={relatedDataLoading || patients.length === 0 || providers.length === 0 || policies.length === 0}>+ Add Authorization</button>
        </Can>
      </div>

      {formError && !isFormOpen && <div className="page-alert" role="alert">{formError}</div>}

      <div className="patients-toolbar">
        <input type="search" value={search} onChange={(event) => setSearch(event.target.value)} placeholder="Search by patient, provider, policy, code or status..." aria-label="Search prior authorizations" />
        <span>{filteredAuthorizations.length} authorization{filteredAuthorizations.length === 1 ? "" : "s"}</span>
        {isFetching && <span className="sync-status">Refreshing...</span>}
      </div>

      <div className="patients-table-card">
        {filteredAuthorizations.length === 0 ? (
          <div className="empty-state">
            <h2>{search ? "No matching authorizations" : "No prior authorizations yet"}</h2>
            <p>{search ? "Try a different search term." : "Create the first prior authorization to get started."}</p>
            {!search && <Can permission="PriorAuthorizationWrite"><button type="button" className="button-primary" onClick={openCreateForm} disabled={relatedDataLoading || patients.length === 0 || providers.length === 0 || policies.length === 0}>Add Authorization</button></Can>}
          </div>
        ) : (
          <div className="table-wrapper">
            <table className="patients-table">
              <thead>
                <tr>
                  <th>Patient / Provider</th>
                  <th>Insurance Policy</th>
                  <th>Clinical Codes</th>
                  <th>Requested</th>
                  <th>Status</th>
                  <th className="actions-column">Actions</th>
                </tr>
              </thead>
              <tbody>
                {filteredAuthorizations.map((authorization) => (
                  <tr key={authorization.id}>
                    <td>
                      <div className="patient-name">{displayName(patientMap, authorization.patientId, "Unknown patient")}</div>
                      <div className="patient-secondary">Provider: {displayName(providerMap, authorization.providerId, "Unknown provider")}</div>
                    </td>
                    <td>
                      <div className="patient-name">{displayName(policyMap, authorization.insurancePolicyId, "Unknown policy")}</div>
                    </td>
                    <td>
                      <div className="patient-name">Dx: {authorization.diagnosisCode}</div>
                      <div className="patient-secondary">Procedure: {authorization.procedureCode}</div>
                    </td>
                    <td>{formatDate(authorization.requestedDate)}</td>
                    <td><span className="status-pill">{STATUS_LABELS[authorization.status] || authorization.status}</span></td>
                    <td className="actions-column">
                      <Can permission="PriorAuthorizationWrite">
                        <div className="table-actions">
                          <button type="button" className="button-link" onClick={() => openEditForm(authorization.id)}>Edit</button>
                          <button type="button" className="button-link button-danger-link" disabled={deletingId === authorization.id || isDeleting} onClick={() => handleDelete(authorization.id)}>
                            {deletingId === authorization.id ? "Deleting..." : "Delete"}
                          </button>
                        </div>
                      </Can>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>

      {isFormOpen && (
        <div className="modal-backdrop" role="presentation" onMouseDown={(event) => { if (event.target === event.currentTarget) closeForm(); }}>
          <div className="patient-modal" role="dialog" aria-modal="true" aria-labelledby="prior-authorization-modal-title">
            <div className="patient-modal-header">
              <div><div className="page-eyebrow">Authorization Request</div><h2 id="prior-authorization-modal-title">{editingAuthorization ? "Review Prior Authorization" : "Add Prior Authorization"}</h2></div>
              <button type="button" className="modal-close" onClick={closeForm} disabled={isCreating || isUpdating} aria-label="Close">×</button>
            </div>
            {formError && <div className="page-alert" role="alert">{formError}</div>}
            {relatedDataLoading && !editingAuthorization ? (
              <div className="empty-state"><p>Loading patients, providers and insurance policies...</p></div>
            ) : (
              <PriorAuthorizationForm
                priorAuthorization={editingAuthorization}
                patients={patients}
                providers={providers}
                policies={policies}
                isSubmitting={isCreating || isUpdating}
                onSubmit={handleSubmit}
                onCancel={closeForm}
              />
            )}
          </div>
        </div>
      )}
    </section>
  );
}

export default PriorAuthorizations;
