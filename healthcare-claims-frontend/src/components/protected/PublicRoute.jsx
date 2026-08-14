import { Navigate, Outlet } from "react-router-dom";
import { useSelector } from "react-redux";

function PublicRoute() {
  const token = useSelector((state) => state.auth.token);

  if (token) {
    return <Navigate to="/dashboard" replace />;
  }

  return <Outlet />;
}

export default PublicRoute;
