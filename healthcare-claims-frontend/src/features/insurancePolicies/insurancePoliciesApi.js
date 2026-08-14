import { baseApi } from "../../api/baseApi";

export const insurancePoliciesApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    getInsurancePolicies: builder.query({
      query: () => "/insurancepolicy",
      providesTags: (result = []) => [
        { type: "InsurancePolicy", id: "LIST" },
        ...result.map(({ id }) => ({ type: "InsurancePolicy", id })),
      ],
    }),

    getInsurancePolicyById: builder.query({
      query: (id) => `/insurancepolicy/${id}`,
      providesTags: (_result, _error, id) => [{ type: "InsurancePolicy", id }],
    }),

    createInsurancePolicy: builder.mutation({
      query: (policy) => ({
        url: "/insurancepolicy",
        method: "POST",
        body: policy,
      }),
      invalidatesTags: [{ type: "InsurancePolicy", id: "LIST" }],
    }),

    updateInsurancePolicy: builder.mutation({
      query: ({ id, ...policy }) => ({
        url: `/insurancepolicy/${id}`,
        method: "PUT",
        body: policy,
      }),
      invalidatesTags: (_result, _error, { id }) => [
        { type: "InsurancePolicy", id },
        { type: "InsurancePolicy", id: "LIST" },
      ],
    }),

    deleteInsurancePolicy: builder.mutation({
      query: (id) => ({
        url: `/insurancepolicy/${id}`,
        method: "DELETE",
      }),
      invalidatesTags: (_result, _error, id) => [
        { type: "InsurancePolicy", id },
        { type: "InsurancePolicy", id: "LIST" },
      ],
    }),
  }),
  overrideExisting: false,
});

export const {
  useGetInsurancePoliciesQuery,
  useLazyGetInsurancePolicyByIdQuery,
  useCreateInsurancePolicyMutation,
  useUpdateInsurancePolicyMutation,
  useDeleteInsurancePolicyMutation,
} = insurancePoliciesApi;
