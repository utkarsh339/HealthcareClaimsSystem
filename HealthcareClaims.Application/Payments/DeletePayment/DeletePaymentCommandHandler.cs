using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.Payments.DeletePayment;

public class DeletePaymentCommandHandler
    : IRequestHandler<DeletePaymentCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuditService _auditService;

    public DeletePaymentCommandHandler(IApplicationDbContext context,
        IAuditService auditService)
    {
        _context = context;
        _auditService = auditService;
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

        await _auditService.LogAsync(
            AuditAction.Delete,
            "Payment",
            payment.Id,
            $"Deleted payment {payment.Id}",
            cancellationToken: cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
}