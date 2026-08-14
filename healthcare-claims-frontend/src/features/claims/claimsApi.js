import { baseApi } from "../../api/baseApi";

export const claimsApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    getClaims: builder.query({
      query: () => "/Claims",
      providesTags: (result = []) => [
        { type: "Claim", id: "LIST" },
        ...result.map(({ id }) => ({ type: "Claim", id })),
      ],
    }),

    getClaimById: builder.query({
      query: (id) => `/Claims/${id}`,
      providesTags: (_result, _error, id) => [{ type: "Claim", id }],
    }),

    createClaim: builder.mutation({
      query: (claim) => ({
        url: "/Claims",
        method: "POST",
        body: claim,
      }),
      invalidatesTags: [{ type: "Claim", id: "LIST" }],
    }),

    updateClaim: builder.mutation({
      query: ({ id, ...claim }) => ({
        url: `/Claims/${id}`,
        method: "PUT",
        body: claim,
      }),
      invalidatesTags: (_result, _error, { id }) => [
        { type: "Claim", id },
        { type: "Claim", id: "LIST" },
      ],
    }),

    updateClaimStatus: builder.mutation({
      query: ({ id, status, approvedAmount = 0 }) => ({
        url: `/Claims/${id}/status`,
        method: "PUT",
        body: { status, approvedAmount },
      }),
      invalidatesTags: (_result, _error, { id }) => [
        { type: "Claim", id },
        { type: "Claim", id: "LIST" },
        { type: "Dashboard", id: "SUMMARY" },
        { type: "Dashboard", id: "CLAIMS" },
      ],
    }),

    deleteClaim: builder.mutation({
      query: (id) => ({
        url: `/Claims/${id}`,
        method: "DELETE",
      }),
      invalidatesTags: (_result, _error, id) => [
        { type: "Claim", id },
        { type: "Claim", id: "LIST" },
      ],
    }),
  }),
  overrideExisting: false,
});

export const {
  useGetClaimsQuery,
  useLazyGetClaimByIdQuery,
  useCreateClaimMutation,
  useUpdateClaimMutation,
  useDeleteClaimMutation,
  useUpdateClaimStatusMutation,
} = claimsApi;
