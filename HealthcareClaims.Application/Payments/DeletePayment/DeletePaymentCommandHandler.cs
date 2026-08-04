using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.Payments.DeletePayment;

public class DeletePaymentCommandHandler
    : IRequestHandler<DeletePaymentCommand>
{
    private readonly IApplicationDbContext _context;

    public DeletePaymentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeletePaymentCommand request,
        CancellationToken cancellationToken)
    {
        var payment = await _context.Payments
            .FirstOrDefaultAsync(
                p => p.Id == request.Id,
                cancellationToken);

        if (payment is null)
            throw new KeyNotFoundException("Payment not found.");

        if (payment.Status == PaymentStatus.Completed)
            throw new InvalidOperationException(
                "Completed payments cannot be deleted.");

        _context.Payments.Remove(payment);

        await _context.SaveChangesAsync(cancellationToken);
    }
}