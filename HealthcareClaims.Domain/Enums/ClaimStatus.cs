namespace HealthcareClaims.Domain.Enums;

public enum ClaimStatus
{
    Draft,
    Submitted,
    UnderReview,
    Approved,
    PartiallyApproved,
    Denied,
    Paid,
    Cancelled
}