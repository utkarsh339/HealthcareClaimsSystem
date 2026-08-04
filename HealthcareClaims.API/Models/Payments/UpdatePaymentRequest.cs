using HealthcareClaims.Domain.Enums;

namespace HealthcareClaims.API.Models.Payments;

public class UpdatePaymentRequest
{
    public decimal Amount { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public PaymentStatus Status { get; set; }

    public DateTime? PaidDate { get; set; }
}