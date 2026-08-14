import { baseApi } from "../../api/baseApi";

export const dashboardApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    getDashboardSummary: builder.query({
      query: () => "/dashboard/summary",
      providesTags: [{ type: "Dashboard", id: "SUMMARY" }],
    }),

    getClaimsStatistics: builder.query({
      query: () => "/dashboard/claims/statistics",
      providesTags: [{ type: "Dashboard", id: "CLAIMS" }],
    }),
  }),
});

export const {
  useGetDashboardSummaryQuery,
  useGetClaimsStatisticsQuery,
} = dashboardApi;
