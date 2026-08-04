using HealthcareClaims.Application.Payments.Common;
using HealthcareClaims.Domain.Enums;
using MediatR;

namespace HealthcareClaims.Application.Payments.UpdatePayment;

public record UpdatePaymentCommand(
    Guid Id,
    decimal Amount,
    PaymentMethod PaymentMethod,
    PaymentStatus Status,
    DateTime? PaidDate
) : IRequest<PaymentResponse>;