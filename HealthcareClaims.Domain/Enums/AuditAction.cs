namespace HealthcareClaims.Domain.Enums;

public enum AuditAction
{
    Create = 1,
    Update,
    Delete,
    Login,
    Logout,
    Approve,
    Reject,
    Submit,
    PaymentProcessed
}