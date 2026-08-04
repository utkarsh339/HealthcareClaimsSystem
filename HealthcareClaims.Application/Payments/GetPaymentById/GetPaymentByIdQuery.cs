using HealthcareClaims.Application.Payments.Common;
using MediatR;

namespace HealthcareClaims.Application.Payments.GetPaymentById;

public record GetPaymentByIdQuery(Guid Id)
    : IRequest<PaymentResponse>;