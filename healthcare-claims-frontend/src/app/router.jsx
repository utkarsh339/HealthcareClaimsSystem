import { createBrowserRouter } from "react-router-dom";

import Home from "../pages/Home";
import Login from "../pages/Login";
import Register from "../pages/Register";
import Dashboard from "../pages/Dashboard";
import NotFound from "../pages/NotFound";

import ProtectedRoute from "../components/protected/ProtectedRoute";
import PublicRoute from "../components/protected/PublicRoute";
import AppLayout from "../components/layout/AppLayout";
import Patients from "../pages/Patients";
import Providers from "../pages/Providers";
import InsurancePolicies from "../pages/InsurancePolicies";
import PriorAuthorizations from "../pages/PriorAuthorizations";
import Claims from "../pages/Claims";
import Payments from "../pages/Payments";
import AuditLogs from "../pages/AuditLogs";

export const router = createBrowserRouter([
  {
    element: <PublicRoute />,
    children: [
      {
        path: "/",
        element: <Home />,
      },
      {
        path: "/login",
        element: <Login />,
      },
      {
        path: "/register",
        element: <Register />,
      },
    ],
  },

  {
    element: <ProtectedRoute />,
    children: [
      {
        element: <AppLayout />,
        children: [
          {
            path: "/dashboard",
            element: <Dashboard />,
          },
          {
            path: "/patients",
            element: <Patients />,
          },
          {
            path: "/providers",
            element: <Providers />,
          },
          {
            path: "/insurance-policies",
            element: <InsurancePolicies />,
          },
          {
            path: "/prior-authorizations",
            element: <PriorAuthorizations />,
          },
          {
            path: "/claims",
            element: <Claims />,
          },
          {
            path: "/payments",
            element: <Payments />,
          },
          {
            path: "/audit-logs",
            element: <AuditLogs />,
          },
        ],
      },
    ],
  },

  {
    path: "*",
    element: <NotFound />,
  },
]);
