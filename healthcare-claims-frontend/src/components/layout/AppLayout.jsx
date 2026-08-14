import { NavLink, Outlet, useNavigate } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";

import { logout } from "../../features/auth/authSlice";
import { hasPermission } from "../../features/auth/permissions";

const navigationItems = [
  { label: "Dashboard", to: "/dashboard", permission: "DashboardRead" },
  { label: "Patients", to: "/patients", permission: "PatientRead" },
  { label: "Providers", to: "/providers", permission: "ProviderRead" },
  {
    label: "Insurance Policies",
    to: "/insurance-policies",
    permission: "InsurancePolicyRead",
  },
  {
    label: "Prior Authorizations",
    to: "/prior-authorizations",
    permission: "PriorAuthorizationRead",
  },
  { label: "Claims", to: "/claims", permission: "ClaimRead" },
  { label: "Payments", to: "/payments", permission: "PaymentRead" },
  { label: "Audit Logs", to: "/audit-logs", permission: "AuditLogRead" },
];

function AppLayout() {
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const user = useSelector((state) => state.auth.user);
  const role = user?.role;

  const visibleNavigationItems = navigationItems.filter((item) =>
    hasPermission(role, item.permission),
  );

  const handleLogout = () => {
    dispatch(logout());
    navigate("/login", { replace: true });
  };

  return (
    <div className="app-layout">
      <header className="app-header">
        <div>
          <h1>Healthcare Claims System</h1>
        </div>

        <div>
          <div className="app-user-info">
            <span>{user?.fullName || user?.email || "User"}</span>
            {role && <span className="app-user-role">{role}</span>}
          </div>

          <button type="button" onClick={handleLogout}>
            Logout
          </button>
        </div>
      </header>

      <div className="app-body">
        <aside className="sidebar">
          <nav aria-label="Main navigation">
            {visibleNavigationItems.map((item) => (
              <NavLink key={item.to} to={item.to}>
                {item.label}
              </NavLink>
            ))}
          </nav>
        </aside>

        <main className="main-content">
          <Outlet />
        </main>
      </div>
    </div>
  );
}

export default AppLayout;
