using MediatR;

namespace HealthcareClaims.Application.Payments.DeletePayment;

public record DeletePaymentCommand(Guid Id) : IRequest;