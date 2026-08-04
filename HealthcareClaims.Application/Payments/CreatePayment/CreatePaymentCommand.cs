using HealthcareClaims.Application.Payments.Common;
using MediatR;
using HealthcareClaims.Domain.Enums;

namespace HealthcareClaims.Application.Payments.CreatePayment;

public record CreatePaymentCommand(
    Guid ClaimId,
    decimal Amount,
    PaymentMethod PaymentMethod
) : IRequest<PaymentResponse>;