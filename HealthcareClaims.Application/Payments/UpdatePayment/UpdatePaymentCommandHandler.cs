using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Application.Payments.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.Payments.UpdatePayment;

public class UpdatePaymentCommandHandler
    : IRequestHandler<UpdatePaymentCommand, PaymentResponse>
{
    private readonly IApplicationDbContext _context;

    public UpdatePaymentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaymentResponse> Handle(
        UpdatePaymentCommand request,
        CancellationToken cancellationToken)
    {
        var payment = await _context.Payments
            .Include(p => p.Claim)
            .FirstOrDefaultAsync(
                p => p.Id == request.Id,
                cancellationToken);

        if (payment is null)
            throw new KeyNotFoundException("Payment not found.");

        payment.Amount = request.Amount;
        payment.PaymentMethod = request.PaymentMethod;
        payment.Status = request.Status;
        payment.PaidDate = request.PaidDate;

        await _context.SaveChangesAsync(cancellationToken);

        return new PaymentResponse
        {
            Id = payment.Id,
            ClaimId = payment.ClaimId,
            ClaimNumber = payment.Claim.ClaimNumber,
            Amount = payment.Amount,
            PaymentReference = payment.PaymentReference,
            PaymentMethod = payment.PaymentMethod,
            Status = payment.Status,
            PaidDate = payment.PaidDate,
            CreatedAt = payment.CreatedAt
        };
    }
}