import { useSelector } from "react-redux";

import { hasAnyPermission, hasPermission } from "../../features/auth/permissions";

function Can({ permission, anyOf, children, fallback = null }) {
  const role = useSelector((state) => state.auth.user?.role);

  const allowed = permission
    ? hasPermission(role, permission)
    : hasAnyPermission(role, anyOf);

  return allowed ? children : fallback;
}

export default Can;
