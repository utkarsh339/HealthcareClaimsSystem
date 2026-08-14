const ROLE = Object.freeze({
  ADMIN: "Admin",
  CLAIMS_PROCESSOR: "ClaimsProcessor",
  MEDICAL_REVIEWER: "MedicalReviewer",
  PROVIDER_STAFF: "ProviderStaff",
});

const ROLE_PERMISSIONS = Object.freeze({
  [ROLE.ADMIN]: [
    "DashboardRead",
    "PatientRead",
    "PatientWrite",
    "ProviderRead",
    "ProviderWrite",
    "InsurancePolicyRead",
    "InsurancePolicyWrite",
    "PriorAuthorizationRead",
    "PriorAuthorizationWrite",
    "ClaimRead",
    "ClaimWrite",
    "ClaimProcess",
    "PaymentRead",
    "PaymentWrite",
    "AuditLogRead",
  ],
  [ROLE.CLAIMS_PROCESSOR]: [
    "DashboardRead",
    "PatientRead",
    "ProviderRead",
    "InsurancePolicyRead",
    "PriorAuthorizationRead",
    "PriorAuthorizationWrite",
    "ClaimRead",
    "ClaimWrite",
    "ClaimProcess",
    "PaymentRead",
    "PaymentWrite",
    "AuditLogRead",
  ],
  [ROLE.MEDICAL_REVIEWER]: [
    "DashboardRead",
    "PatientRead",
    "ProviderRead",
    "InsurancePolicyRead",
    "PriorAuthorizationRead",
    "PriorAuthorizationWrite",
    "ClaimRead",
    "PaymentRead",
    "AuditLogRead",
  ],
  [ROLE.PROVIDER_STAFF]: [
    "DashboardRead",
    "PatientRead",
    "PatientWrite",
    "ProviderRead",
    "ProviderWrite",
    "InsurancePolicyRead",
    "InsurancePolicyWrite",
    "PriorAuthorizationRead",
    "ClaimRead",
    "PaymentRead",
  ],
});

export { ROLE, ROLE_PERMISSIONS };

export function hasPermission(role, permission) {
  if (!role || !permission) {
    return false;
  }

  return ROLE_PERMISSIONS[role]?.includes(permission) ?? false;
}

export function hasAnyPermission(role, permissions = []) {
  return permissions.some((permission) => hasPermission(role, permission));
}
