import { baseApi } from "../../api/baseApi";

export const priorAuthorizationsApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    getPriorAuthorizations: builder.query({
      query: () => "/PriorAuthorization",
      providesTags: (result = []) => [
        { type: "PriorAuthorization", id: "LIST" },
        ...result.map(({ id }) => ({ type: "PriorAuthorization", id })),
      ],
    }),

    getPriorAuthorizationById: builder.query({
      query: (id) => `/PriorAuthorization/${id}`,
      providesTags: (_result, _error, id) => [{ type: "PriorAuthorization", id }],
    }),

    createPriorAuthorization: builder.mutation({
      query: (priorAuthorization) => ({
        url: "/PriorAuthorization",
        method: "POST",
        body: priorAuthorization,
      }),
      invalidatesTags: [{ type: "PriorAuthorization", id: "LIST" }],
    }),

    updatePriorAuthorization: builder.mutation({
      query: ({ id, ...priorAuthorization }) => ({
        url: `/PriorAuthorization/${id}`,
        method: "PUT",
        body: priorAuthorization,
      }),
      invalidatesTags: (_result, _error, { id }) => [
        { type: "PriorAuthorization", id },
        { type: "PriorAuthorization", id: "LIST" },
      ],
    }),

    deletePriorAuthorization: builder.mutation({
      query: (id) => ({
        url: `/PriorAuthorization/${id}`,
        method: "DELETE",
      }),
      invalidatesTags: (_result, _error, id) => [
        { type: "PriorAuthorization", id },
        { type: "PriorAuthorization", id: "LIST" },
      ],
    }),
  }),
  overrideExisting: false,
});

export const {
  useGetPriorAuthorizationsQuery,
  useLazyGetPriorAuthorizationByIdQuery,
  useCreatePriorAuthorizationMutation,
  useUpdatePriorAuthorizationMutation,
  useDeletePriorAuthorizationMutation,
} = priorAuthorizationsApi;
