using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Application.Payments.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.Payments.GetPaymentById;

public class GetPaymentByIdQueryHandler
    : IRequestHandler<GetPaymentByIdQuery, PaymentResponse>
{
    private readonly IApplicationDbContext _context;

    public GetPaymentByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaymentResponse> Handle(
        GetPaymentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var payment = await _context.Payments
            .AsNoTracking()
            .Include(p => p.Claim)
            .FirstOrDefaultAsync(
                p => p.Id == request.Id,
                cancellationToken);

        if (payment is null)
            throw new KeyNotFoundException("Payment not found.");

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