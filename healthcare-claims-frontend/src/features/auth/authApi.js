import { baseApi } from "../../api/baseApi";

export const authApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    login: builder.mutation({
      query: (credentials) => ({
        url: "/auth/login",
        method: "POST",
        body: credentials,
      }),
    }),
    register: builder.mutation({
      query: (registration) => ({
        url: "/auth/register",
        method: "POST",
        body: registration,
      }),
    }),
  }),
});

export const { useLoginMutation, useRegisterMutation } = authApi;
