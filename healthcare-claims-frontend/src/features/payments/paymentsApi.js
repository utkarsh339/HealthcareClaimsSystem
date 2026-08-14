import { baseApi } from "../../api/baseApi";

export const paymentsApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    getPayments: builder.query({
      query: () => "/Payments",
      providesTags: (result = []) => [
        { type: "Payment", id: "LIST" },
        ...result.map(({ id }) => ({ type: "Payment", id })),
      ],
    }),

    getPaymentById: builder.query({
      query: (id) => `/Payments/${id}`,
      providesTags: (_result, _error, id) => [{ type: "Payment", id }],
    }),

    createPayment: builder.mutation({
      query: (payment) => ({
        url: "/Payments",
        method: "POST",
        body: payment,
      }),
      invalidatesTags: [
        { type: "Payment", id: "LIST" },
      ],
    }),

    updatePayment: builder.mutation({
      query: ({ id, ...payment }) => ({
        url: `/Payments/${id}`,
        method: "PUT",
        body: payment,
      }),
      invalidatesTags: (_result, _error, { id }) => [
        { type: "Payment", id },
        { type: "Payment", id: "LIST" },
      ],
    }),

    deletePayment: builder.mutation({
      query: (id) => ({
        url: `/Payments/${id}`,
        method: "DELETE",
      }),
      invalidatesTags: (_result, _error, id) => [
        { type: "Payment", id },
        { type: "Payment", id: "LIST" },
      ],
    }),
  }),
  overrideExisting: false,
});

export const {
  useGetPaymentsQuery,
  useLazyGetPaymentByIdQuery,
  useCreatePaymentMutation,
  useUpdatePaymentMutation,
  useDeletePaymentMutation,
} = paymentsApi;
