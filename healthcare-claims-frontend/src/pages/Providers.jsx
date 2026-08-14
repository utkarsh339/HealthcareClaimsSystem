import { getErrorMessage } from "../utils_errorMessage";
import { useMemo, useState } from "react";
import Can from "../components/authorization/Can";
import ProviderForm from "../components/providers/ProviderForm";
import {
  useCreateProviderMutation,
  useDeleteProviderMutation,
  useGetProvidersQuery,
  useLazyGetProviderByIdQuery,
  useUpdateProviderMutation,
} from "../features/providers/providersApi";

function formatDate(value) {
  if (!value) return "—";
  return new Intl.DateTimeFormat("en-IN", {
    day: "2-digit",
    month: "short",
    year: "numeric",
  }).format(new Date(value));
}

function Providers() {
  const {
    data: providers = [],
    isLoading,
    isFetching,
    isError,
    error,
    refetch,
  } = useGetProvidersQuery();
  const [getProviderById] = useLazyGetProviderByIdQuery();
  const [createProvider, { isLoading: isCreating }] = useCreateProviderMutation();
  const [updateProvider, { isLoading: isUpdating }] = useUpdateProviderMutation();
  const [deleteProvider, { isLoading: isDeleting }] = useDeleteProviderMutation();

  const [search, setSearch] = useState("");
  const [isFormOpen, setIsFormOpen] = useState(false);
  const [editingProvider, setEditingProvider] = useState(null);
  const [formError, setFormError] = useState("");
  const [deletingId, setDeletingId] = useState(null);

  const filteredProviders = useMemo(() => {
    const query = search.trim().toLowerCase();
    if (!query) return providers;

    return providers.filter((provider) =>
      [
        provider.providerName,
        provider.providerType,
        provider.npi,
        provider.email,
        provider.phoneNumber,
        provider.city,
        provider.state,
        provider.zipCode,
        provider.isActive ? "active" : "inactive",
      ].some((value) => String(value ?? "").toLowerCase().includes(query)),
    );
  }, [providers, search]);

  const openCreateForm = () => {
    setEditingProvider(null);
    setFormError("");
    setIsFormOpen(true);
  };

  const openEditForm = async (id) => {
    setFormError("");
    try {
      const provider = await getProviderById(id).unwrap();
      setEditingProvider(provider);
      setIsFormOpen(true);
    } catch (requestError) {
      setFormError(getErrorMessage(requestError, "Unable to load the provider."));
    }
  };

  const closeForm = () => {
    if (isCreating || isUpdating) return;
    setIsFormOpen(false);
    setEditingProvider(null);
    setFormError("");
  };

  const handleSubmit = async (provider) => {
    setFormError("");

    try {
      if (editingProvider) {
        await updateProvider({ id: editingProvider.id, ...provider }).unwrap();
      } else {
        await createProvider(provider).unwrap();
      }
      closeForm();
    } catch (requestError) {
      setFormError(getErrorMessage(requestError, "Unable to save the provider."));
    }
  };

  const handleDelete = async (id, name) => {
    if (!window.confirm(`Delete provider ${name}? This action cannot be undone.`)) return;

    setDeletingId(id);
    setFormError("");

    try {
      await deleteProvider(id).unwrap();
    } catch (requestError) {
      setFormError(getErrorMessage(requestError, "Unable to delete the provider."));
    } finally {
      setDeletingId(null);
    }
  };

  if (isLoading) {
    return (
      <div className="page-state">
        <div className="page-spinner" />
        <h2>Loading providers...</h2>
      </div>
    );
  }

  if (isError) {
    return (
      <div className="page-state">
        <h2>Unable to load providers</h2>
        <p>{getErrorMessage(error, "The Providers API could not be reached.")}</p>
        <button type="button" className="button-primary" onClick={refetch}>Retry</button>
      </div>
    );
  }

  return (
    <section className="patients-page">
      <div className="patients-header">
        <div>
          <div className="page-eyebrow">Provider Management</div>
          <h1>Providers</h1>
          <p>Manage healthcare provider records and contact information.</p>
        </div>

        <Can permission="ProviderWrite">
          <button type="button" className="button-primary" onClick={openCreateForm}>
            + Add Provider
          </button>
        </Can>
      </div>

      {formError && !isFormOpen && <div className="page-alert" role="alert">{formError}</div>}

      <div className="patients-toolbar">
        <input
          type="search"
          value={search}
          onChange={(event) => setSearch(event.target.value)}
          placeholder="Search by provider, NPI, type, city or status..."
          aria-label="Search providers"
        />
        <span>{filteredProviders.length} provider{filteredProviders.length === 1 ? "" : "s"}</span>
        {isFetching && <span className="sync-status">Refreshing...</span>}
      </div>

      <div className="patients-table-card">
        {filteredProviders.length === 0 ? (
          <div className="empty-state">
            <h2>{search ? "No matching providers" : "No providers yet"}</h2>
            <p>
              {search
                ? "Try a different search term."
                : "Create the first provider record to start managing providers."}
            </p>
            {!search && (
              <Can permission="ProviderWrite">
                <button type="button" className="button-primary" onClick={openCreateForm}>
                  Add Provider
                </button>
              </Can>
            )}
          </div>
        ) : (
          <div className="table-wrapper">
            <table className="patients-table">
              <thead>
                <tr>
                  <th>Provider</th>
                  <th>Type</th>
                  <th>NPI</th>
                  <th>Contact</th>
                  <th>Location</th>
                  <th>Status</th>
                  <th>Created</th>
                  <th className="actions-column">Actions</th>
                </tr>
              </thead>
              <tbody>
                {filteredProviders.map((provider) => (
                  <tr key={provider.id}>
                    <td>
                      <div className="patient-name">{provider.providerName}</div>
                      <div className="patient-secondary">{provider.email}</div>
                    </td>
                    <td>{provider.providerType}</td>
                    <td>{provider.npi}</td>
                    <td>
                      <div>{provider.phoneNumber}</div>
                      <div className="patient-secondary">{provider.email}</div>
                    </td>
                    <td>
                      <div>{provider.city}</div>
                      <div className="patient-secondary">{provider.state} {provider.zipCode}</div>
                    </td>
                    <td>
                      <span className="status-pill">
                        {provider.isActive ? "Active" : "Inactive"}
                      </span>
                    </td>
                    <td>{formatDate(provider.createdAt)}</td>
                    <td>
                      <Can permission="ProviderWrite">
                        <div className="table-actions">
                          <button type="button" className="button-link" onClick={() => openEditForm(provider.id)}>
                            Edit
                          </button>
                          <button
                            type="button"
                            className="button-link button-danger-link"
                            disabled={deletingId === provider.id || isDeleting}
                            onClick={() => handleDelete(provider.id, provider.providerName)}
                          >
                            {deletingId === provider.id ? "Deleting..." : "Delete"}
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
        <div
          className="modal-backdrop"
          role="presentation"
          onMouseDown={(event) => {
            if (event.target === event.currentTarget) closeForm();
          }}
        >
          <div className="patient-modal" role="dialog" aria-modal="true" aria-labelledby="provider-modal-title">
            <div className="patient-modal-header">
              <div>
                <div className="page-eyebrow">Provider Record</div>
                <h2 id="provider-modal-title">
                  {editingProvider ? "Edit Provider" : "Add Provider"}
                </h2>
              </div>
              <button
                type="button"
                className="modal-close"
                onClick={closeForm}
                disabled={isCreating || isUpdating}
                aria-label="Close"
              >
                ×
              </button>
            </div>

            {formError && <div className="page-alert" role="alert">{formError}</div>}

            <ProviderForm
              provider={editingProvider}
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

export default Providers;
