import { getErrorMessage } from "../utils_errorMessage";
import { useMemo, useState } from "react";
import Can from "../components/authorization/Can";
import PatientForm from "../components/patients/PatientForm";
import {
  useCreatePatientMutation,
  useDeletePatientMutation,
  useGetPatientsQuery,
  useLazyGetPatientByIdQuery,
  useUpdatePatientMutation,
} from "../features/patients/patientsApi";

function formatDate(value) {
  if (!value) return "—";
  return new Intl.DateTimeFormat("en-IN", {
    day: "2-digit",
    month: "short",
    year: "numeric",
  }).format(new Date(`${value}T00:00:00`));
}

function Patients() {
  const { data: patients = [], isLoading, isFetching, isError, error, refetch } = useGetPatientsQuery();
  const [getPatientById] = useLazyGetPatientByIdQuery();
  const [createPatient, { isLoading: isCreating }] = useCreatePatientMutation();
  const [updatePatient, { isLoading: isUpdating }] = useUpdatePatientMutation();
  const [deletePatient, { isLoading: isDeleting }] = useDeletePatientMutation();

  const [search, setSearch] = useState("");
  const [isFormOpen, setIsFormOpen] = useState(false);
  const [editingPatient, setEditingPatient] = useState(null);
  const [formError, setFormError] = useState("");
  const [deletingId, setDeletingId] = useState(null);

  const filteredPatients = useMemo(() => {
    const query = search.trim().toLowerCase();
    if (!query) return patients;

    return patients.filter((patient) =>
      [
        patient.firstName,
        patient.lastName,
        patient.email,
        patient.phoneNumber,
        patient.gender,
      ].some((value) => value?.toLowerCase().includes(query)),
    );
  }, [patients, search]);

  const openCreateForm = () => {
    setEditingPatient(null);
    setFormError("");
    setIsFormOpen(true);
  };

  const openEditForm = async (id) => {
    setFormError("");
    try {
      const patient = await getPatientById(id).unwrap();
      setEditingPatient(patient);
      setIsFormOpen(true);
    } catch (requestError) {
      setFormError(getErrorMessage(requestError, "Unable to load the patient."));
    }
  };

  const closeForm = () => {
    if (isCreating || isUpdating) return;
    setIsFormOpen(false);
    setEditingPatient(null);
    setFormError("");
  };

  const handleSubmit = async (patient) => {
    setFormError("");

    try {
      if (editingPatient) {
        await updatePatient({ id: editingPatient.id, ...patient }).unwrap();
      } else {
        await createPatient(patient).unwrap();
      }

      closeForm();
    } catch (requestError) {
      setFormError(getErrorMessage(requestError, "Unable to save the patient."));
    }
  };

  const handleDelete = async (id, name) => {
    if (!window.confirm(`Delete patient ${name}? This action cannot be undone.`)) return;

    setDeletingId(id);
    setFormError("");

    try {
      await deletePatient(id).unwrap();
    } catch (requestError) {
      setFormError(getErrorMessage(requestError, "Unable to delete the patient."));
    } finally {
      setDeletingId(null);
    }
  };

  if (isLoading) {
    return (
      <div className="page-state">
        <div className="page-spinner" />
        <h2>Loading patients...</h2>
      </div>
    );
  }

  if (isError) {
    return (
      <div className="page-state">
        <h2>Unable to load patients</h2>
        <p>{getErrorMessage(error, "The Patients API could not be reached.")}</p>
        <button type="button" className="button-primary" onClick={refetch}>Retry</button>
      </div>
    );
  }

  return (
    <section className="patients-page">
      <div className="patients-header">
        <div>
          <div className="page-eyebrow">Patient Management</div>
          <h1>Patients</h1>
          <p>Manage patient records and demographic information.</p>
        </div>

        <Can permission="PatientWrite">
          <button type="button" className="button-primary" onClick={openCreateForm}>
            + Add Patient
          </button>
        </Can>
      </div>

      {formError && <div className="page-alert" role="alert">{formError}</div>}

      <div className="patients-toolbar">
        <input
          type="search"
          placeholder="Search by name, email, phone or gender..."
          value={search}
          onChange={(event) => setSearch(event.target.value)}
          aria-label="Search patients"
        />
        <span>{filteredPatients.length} patient{filteredPatients.length === 1 ? "" : "s"}</span>
        {isFetching && !isLoading && <span className="sync-status">Refreshing...</span>}
      </div>

      <div className="patients-table-card">
        {filteredPatients.length === 0 ? (
          <div className="empty-state">
            <h2>{search ? "No matching patients" : "No patients found"}</h2>
            <p>{search ? "Try a different search term." : "Create the first patient record to get started."}</p>
            {!search && (
              <Can permission="PatientWrite">
                <button type="button" className="button-primary" onClick={openCreateForm}>Add Patient</button>
              </Can>
            )}
          </div>
        ) : (
          <div className="table-wrapper">
            <table className="patients-table">
              <thead>
                <tr>
                  <th>Patient</th>
                  <th>Date of Birth</th>
                  <th>Gender</th>
                  <th>Contact</th>
                  <th>Created</th>
                  <th className="actions-column">Actions</th>
                </tr>
              </thead>
              <tbody>
                {filteredPatients.map((patient) => (
                  <tr key={patient.id}>
                    <td>
                      <div className="patient-name">{patient.firstName} {patient.lastName}</div>
                      <div className="patient-secondary">{patient.email || "No email"}</div>
                    </td>
                    <td>{formatDate(patient.dateOfBirth)}</td>
                    <td><span className="status-pill">{patient.gender}</span></td>
                    <td>{patient.phoneNumber || "—"}</td>
                    <td>{formatDate(patient.createdAt?.slice(0, 10))}</td>
                    <td className="actions-column">
                      <Can permission="PatientWrite">
                        <div className="table-actions">
                          <button type="button" className="button-link" onClick={() => openEditForm(patient.id)}>Edit</button>
                          <button
                            type="button"
                            className="button-link button-danger-link"
                            disabled={deletingId === patient.id || isDeleting}
                            onClick={() => handleDelete(patient.id, `${patient.firstName} ${patient.lastName}`)}
                          >
                            {deletingId === patient.id ? "Deleting..." : "Delete"}
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
        <div className="modal-backdrop" role="presentation" onMouseDown={(event) => {
          if (event.target === event.currentTarget) closeForm();
        }}>
          <div className="patient-modal" role="dialog" aria-modal="true" aria-labelledby="patient-modal-title">
            <div className="patient-modal-header">
              <div>
                <div className="page-eyebrow">Patient Record</div>
                <h2 id="patient-modal-title">{editingPatient ? "Edit Patient" : "Add Patient"}</h2>
              </div>
              <button type="button" className="modal-close" onClick={closeForm} disabled={isCreating || isUpdating} aria-label="Close">×</button>
            </div>

            {formError && <div className="page-alert" role="alert">{formError}</div>}

            <PatientForm
              patient={editingPatient}
              isSubmitting={isCreating || isUpdating}
              onSubmit={handleSubmit}
              onCancel={closeForm}
            />
          </div>
        </div>
      )}
    </section>
  );
}

export default Patients;
