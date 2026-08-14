import { baseApi } from "../../api/baseApi";

export const auditLogsApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    getAuditLogs: builder.query({
      query: () => "/AuditLogs",
      providesTags: (result = []) => [
        { type: "AuditLog", id: "LIST" },
        ...result.map(({ id }) => ({ type: "AuditLog", id })),
      ],
    }),

    getAuditLogById: builder.query({
      query: (id) => `/AuditLogs/${id}`,
      providesTags: (_result, _error, id) => [{ type: "AuditLog", id }],
    }),
  }),
  overrideExisting: false,
});

export const {
  useGetAuditLogsQuery,
  useLazyGetAuditLogByIdQuery,
} = auditLogsApi;
