using HealthcareClaims.Domain.Enums;

namespace HealthcareClaims.API.Models.Payments;

public class CreatePaymentRequest
{
    public Guid ClaimId { get; set; }

    public decimal Amount { get; set; }

    public PaymentMethod PaymentMethod { get; set; }
}