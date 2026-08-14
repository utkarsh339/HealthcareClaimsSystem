import { baseApi } from "../../api/baseApi";

export const patientsApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    getPatients: builder.query({
      query: () => "/patient",
      providesTags: (result = []) => [
        { type: "Patient", id: "LIST" },
        ...result.map(({ id }) => ({ type: "Patient", id })),
      ],
    }),

    getPatientById: builder.query({
      query: (id) => `/patient/${id}`,
      providesTags: (_result, _error, id) => [{ type: "Patient", id }],
    }),

    createPatient: builder.mutation({
      query: (patient) => ({
        url: "/patient",
        method: "POST",
        body: patient,
      }),
      invalidatesTags: [{ type: "Patient", id: "LIST" }],
    }),

    updatePatient: builder.mutation({
      query: ({ id, ...patient }) => ({
        url: `/patient/${id}`,
        method: "PUT",
        body: patient,
      }),
      invalidatesTags: (_result, _error, { id }) => [
        { type: "Patient", id },
        { type: "Patient", id: "LIST" },
      ],
    }),

    deletePatient: builder.mutation({
      query: (id) => ({
        url: `/patient/${id}`,
        method: "DELETE",
      }),
      invalidatesTags: (_result, _error, id) => [
        { type: "Patient", id },
        { type: "Patient", id: "LIST" },
      ],
    }),
  }),
  overrideExisting: false,
});

export const {
  useGetPatientsQuery,
  useLazyGetPatientByIdQuery,
  useCreatePatientMutation,
  useUpdatePatientMutation,
  useDeletePatientMutation,
} = patientsApi;
