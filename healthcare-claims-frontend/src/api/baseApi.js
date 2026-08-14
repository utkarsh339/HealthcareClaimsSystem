import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL || "/api";

const baseQuery = fetchBaseQuery({
  baseUrl: apiBaseUrl.replace(/\/$/, ""),

  prepareHeaders: (headers, { getState }) => {
    const token = getState().auth?.token;

    if (token) {
      headers.set("Authorization", `Bearer ${token}`);
    }

    return headers;
  },
});

export const baseApi = createApi({
  reducerPath: "api",
  baseQuery,
  tagTypes: [
    "Patient",
    "Provider",
    "InsurancePolicy",
    "PriorAuthorization",
    "Claim",
    "Payment",
    "AuditLog",
    "Dashboard",
  ],
  endpoints: () => ({}),
});
