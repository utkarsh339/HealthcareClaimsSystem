using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Application.Payments.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.Payments.GetAllPayments;

public class GetAllPaymentsQueryHandler
    : IRequestHandler<GetAllPaymentsQuery, IReadOnlyList<PaymentResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetAllPaymentsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<PaymentResponse>> Handle(
        GetAllPaymentsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Payments
            .AsNoTracking()
            .Include(p => p.Claim)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new PaymentResponse
            {
                Id = p.Id,
                ClaimId = p.ClaimId,
                ClaimNumber = p.Claim.ClaimNumber,
                Amount = p.Amount,
                PaymentReference = p.PaymentReference,
                PaymentMethod = p.PaymentMethod,
                Status = p.Status,
                PaidDate = p.PaidDate,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }
}