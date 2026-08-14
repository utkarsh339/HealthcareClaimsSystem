import { baseApi } from "../../api/baseApi";

export const providersApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    getProviders: builder.query({
      query: () => "/providers",
      providesTags: (result = []) => [
        { type: "Provider", id: "LIST" },
        ...result.map(({ id }) => ({ type: "Provider", id })),
      ],
    }),

    getProviderById: builder.query({
      query: (id) => `/providers/${id}`,
      providesTags: (_result, _error, id) => [{ type: "Provider", id }],
    }),

    createProvider: builder.mutation({
      query: (provider) => ({
        url: "/providers",
        method: "POST",
        body: provider,
      }),
      invalidatesTags: [{ type: "Provider", id: "LIST" }],
    }),

    updateProvider: builder.mutation({
      query: ({ id, ...provider }) => ({
        url: `/providers/${id}`,
        method: "PUT",
        body: provider,
      }),
      invalidatesTags: (_result, _error, { id }) => [
        { type: "Provider", id },
        { type: "Provider", id: "LIST" },
      ],
    }),

    deleteProvider: builder.mutation({
      query: (id) => ({
        url: `/providers/${id}`,
        method: "DELETE",
      }),
      invalidatesTags: (_result, _error, id) => [
        { type: "Provider", id },
        { type: "Provider", id: "LIST" },
      ],
    }),
  }),
  overrideExisting: false,
});

export const {
  useGetProvidersQuery,
  useLazyGetProviderByIdQuery,
  useCreateProviderMutation,
  useUpdateProviderMutation,
  useDeleteProviderMutation,
} = providersApi;
