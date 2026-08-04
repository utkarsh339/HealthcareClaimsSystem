using HealthcareClaims.Domain.Enums;

namespace HealthcareClaims.Domain.Entities;

public class Payment
{
    public Guid Id { get; set; }

    public Guid ClaimId { get; set; }

    public decimal Amount { get; set; }

    public string PaymentReference { get; set; } = string.Empty;

    public PaymentMethod PaymentMethod { get; set; }

    public PaymentStatus Status { get; set; }

    public DateTime? PaidDate { get; set; }

    public DateTime CreatedAt { get; set; }

    // Navigation Property
    public Claim Claim { get; set; } = null!;
}