using HealthcareClaims.Application.Payments.Common;
using MediatR;

namespace HealthcareClaims.Application.Payments.GetAllPayments;

public record GetAllPaymentsQuery()
    : IRequest<IReadOnlyList<PaymentResponse>>;