using HealthcareClaims.Domain.Enums;

namespace HealthcareClaims.Application.Payments.Common;

public class PaymentResponse
{
    public Guid Id { get; set; }

    public Guid ClaimId { get; set; }

    public string ClaimNumber { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string PaymentReference { get; set; } = string.Empty;

    public PaymentMethod PaymentMethod { get; set; }

    public PaymentStatus Status { get; set; }

    public DateTime? PaidDate { get; set; }

    public DateTime CreatedAt { get; set; }
}