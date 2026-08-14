import { getErrorMessage } from "../utils_errorMessage";
import { useMemo, useState } from "react";
import Can from "../components/authorization/Can";
import InsurancePolicyForm from "../components/insurancePolicies/InsurancePolicyForm";
import { useGetPatientsQuery } from "../features/patients/patientsApi";
import {
  useCreateInsurancePolicyMutation,
  useDeleteInsurancePolicyMutation,
  useGetInsurancePoliciesQuery,
  useLazyGetInsurancePolicyByIdQuery,
  useUpdateInsurancePolicyMutation,
} from "../features/insurancePolicies/insurancePoliciesApi";

function formatDate(value) {
  if (!value) return "—";
  return new Intl.DateTimeFormat("en-IN", { day: "2-digit", month: "short", year: "numeric" })
    .format(new Date(`${value.slice(0, 10)}T00:00:00`));
}

function patientName(patientMap, id) {
  const patient = patientMap.get(id);
  return patient ? `${patient.firstName} ${patient.lastName}` : "Unknown patient";
}

function InsurancePolicies() {
  const { data: policies = [], isLoading, isFetching, isError, error, refetch } = useGetInsurancePoliciesQuery();
  const { data: patients = [], isLoading: patientsLoading } = useGetPatientsQuery();
  const [getPolicyById] = useLazyGetInsurancePolicyByIdQuery();
  const [createPolicy, { isLoading: isCreating }] = useCreateInsurancePolicyMutation();
  const [updatePolicy, { isLoading: isUpdating }] = useUpdateInsurancePolicyMutation();
  const [deletePolicy, { isLoading: isDeleting }] = useDeleteInsurancePolicyMutation();

  const [search, setSearch] = useState("");
  const [isFormOpen, setIsFormOpen] = useState(false);
  const [editingPolicy, setEditingPolicy] = useState(null);
  const [formError, setFormError] = useState("");
  const [deletingId, setDeletingId] = useState(null);

  const patientMap = useMemo(() => new Map(patients.map((patient) => [patient.id, patient])), [patients]);

  const filteredPolicies = useMemo(() => {
    const query = search.trim().toLowerCase();
    if (!query) return policies;
    return policies.filter((policy) => [
      policy.insuranceCompany,
      policy.policyNumber,
      policy.memberId,
      policy.groupNumber,
      policy.planName,
      patientName(patientMap, policy.patientId),
      policy.isActive ? "active" : "inactive",
    ].some((value) => String(value ?? "").toLowerCase().includes(query)));
  }, [policies, search, patientMap]);

  const openCreateForm = () => {
    setEditingPolicy(null);
    setFormError("");
    setIsFormOpen(true);
  };

  const openEditForm = async (id) => {
    setFormError("");
    try {
      const policy = await getPolicyById(id).unwrap();
      setEditingPolicy(policy);
      setIsFormOpen(true);
    } catch (requestError) {
      setFormError(getErrorMessage(requestError, "Unable to load the insurance policy."));
    }
  };

  const closeForm = () => {
    if (isCreating || isUpdating) return;
    setIsFormOpen(false);
    setEditingPolicy(null);
    setFormError("");
  };

  const handleSubmit = async (policy) => {
    setFormError("");
    try {
      if (editingPolicy) {
        await updatePolicy({ id: editingPolicy.id, ...policy }).unwrap();
      } else {
        await createPolicy(policy).unwrap();
      }
      closeForm();
    } catch (requestError) {
      setFormError(getErrorMessage(requestError, "Unable to save the insurance policy."));
    }
  };

  const handleDelete = async (id, number) => {
    if (!window.confirm(`Delete insurance policy ${number}? This action cannot be undone.`)) return;
    setDeletingId(id);
    setFormError("");
    try {
      await deletePolicy(id).unwrap();
    } catch (requestError) {
      setFormError(getErrorMessage(requestError, "Unable to delete the insurance policy."));
    } finally {
      setDeletingId(null);
    }
  };

  if (isLoading) return <div className="page-state"><div className="page-spinner" /><h2>Loading insurance policies...</h2></div>;

  if (isError) return (
    <div className="page-state">
      <h2>Unable to load insurance policies</h2>
      <p>{getErrorMessage(error, "The Insurance Policies API could not be reached.")}</p>
      <button type="button" className="button-primary" onClick={refetch}>Retry</button>
    </div>
  );

  return (
    <section className="patients-page">
      <div className="patients-header">
        <div>
          <div className="page-eyebrow">Coverage Management</div>
          <h1>Insurance Policies</h1>
          <p>Manage patient insurance coverage, policy identifiers and active dates.</p>
        </div>
        <Can permission="InsurancePolicyWrite">
          <button type="button" className="button-primary" onClick={openCreateForm} disabled={patientsLoading || patients.length === 0}>+ Add Policy</button>
        </Can>
      </div>

      {formError && !isFormOpen && <div className="page-alert" role="alert">{formError}</div>}

      <div className="patients-toolbar">
        <input type="search" value={search} onChange={(event) => setSearch(event.target.value)} placeholder="Search by patient, insurer, policy, member or status..." aria-label="Search insurance policies" />
        <span>{filteredPolicies.length} polic{filteredPolicies.length === 1 ? "y" : "ies"}</span>
        {isFetching && <span className="sync-status">Refreshing...</span>}
      </div>

      <div className="patients-table-card">
        {filteredPolicies.length === 0 ? (
          <div className="empty-state">
            <h2>{search ? "No matching policies" : "No insurance policies yet"}</h2>
            <p>{search ? "Try a different search term." : "Create the first insurance policy to get started."}</p>
            {!search && <Can permission="InsurancePolicyWrite"><button type="button" className="button-primary" onClick={openCreateForm} disabled={patients.length === 0}>Add Policy</button></Can>}
          </div>
        ) : (
          <div className="table-wrapper">
            <table className="patients-table">
              <thead>
                <tr><th>Patient</th><th>Insurance</th><th>Policy / Member</th><th>Coverage</th><th>Status</th><th className="actions-column">Actions</th></tr>
              </thead>
              <tbody>
                {filteredPolicies.map((policy) => (
                  <tr key={policy.id}>
                    <td><div className="patient-name">{patientName(patientMap, policy.patientId)}</div><div className="patient-secondary">{policy.planName || "No plan name"}</div></td>
                    <td><div className="patient-name">{policy.insuranceCompany}</div><div className="patient-secondary">Group: {policy.groupNumber || "—"}</div></td>
                    <td><div className="patient-name">{policy.policyNumber}</div><div className="patient-secondary">Member: {policy.memberId}</div></td>
                    <td>{formatDate(policy.effectiveDate)} — {formatDate(policy.expirationDate)}</td>
                    <td><span className="status-pill">{policy.isActive ? "Active" : "Inactive"}</span></td>
                    <td className="actions-column"><Can permission="InsurancePolicyWrite"><div className="table-actions"><button type="button" className="button-link" onClick={() => openEditForm(policy.id)}>Edit</button><button type="button" className="button-link button-danger-link" disabled={deletingId === policy.id || isDeleting} onClick={() => handleDelete(policy.id, policy.policyNumber)}>{deletingId === policy.id ? "Deleting..." : "Delete"}</button></div></Can></td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>

      {isFormOpen && (
        <div className="modal-backdrop" role="presentation" onMouseDown={(event) => { if (event.target === event.currentTarget) closeForm(); }}>
          <div className="patient-modal" role="dialog" aria-modal="true" aria-labelledby="insurance-policy-modal-title">
            <div className="patient-modal-header"><div><div className="page-eyebrow">Insurance Coverage</div><h2 id="insurance-policy-modal-title">{editingPolicy ? "Edit Insurance Policy" : "Add Insurance Policy"}</h2></div><button type="button" className="modal-close" onClick={closeForm} disabled={isCreating || isUpdating} aria-label="Close">×</button></div>
            {formError && <div className="page-alert" role="alert">{formError}</div>}
            {patientsLoading ? <div className="empty-state"><p>Loading patients...</p></div> : <InsurancePolicyForm policy={editingPolicy} patients={patients} isSubmitting={isCreating || isUpdating} onSubmit={handleSubmit} onCancel={closeForm} />}
          </div>
        </div>
      )}
    </section>
  );
}

export default InsurancePolicies;
