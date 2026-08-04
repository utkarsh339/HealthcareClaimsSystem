using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Application.Payments.Common;
using HealthcareClaims.Domain.Entities;
using HealthcareClaims.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.Payments.CreatePayment;

public class CreatePaymentCommandHandler
    : IRequestHandler<CreatePaymentCommand, PaymentResponse>
{
    private readonly IApplicationDbContext _context;

    public CreatePaymentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaymentResponse> Handle(
        CreatePaymentCommand request,
        CancellationToken cancellationToken)
    {
        var claim = await _context.Claims
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == request.ClaimId,
                cancellationToken);

        if (claim is null)
            throw new KeyNotFoundException("Claim not found.");

        if (claim.Status != ClaimStatus.Approved)
            throw new InvalidOperationException(
                "Payment can only be created for approved claims.");

        var paymentExists = await _context.Payments
            .AnyAsync(
                x => x.ClaimId == request.ClaimId,
                cancellationToken);

        if (paymentExists)
            throw new InvalidOperationException(
                "A payment already exists for this claim.");

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            ClaimId = request.ClaimId,
            Amount = request.Amount,
            PaymentMethod = request.PaymentMethod,
            PaymentReference = $"PAY-{DateTime.UtcNow:yyyyMMddHHmmss}",
            Status = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _context.Payments.Add(payment);

        await _context.SaveChangesAsync(cancellationToken);

        return new PaymentResponse
        {
            Id = payment.Id,
            ClaimId = payment.ClaimId,
            ClaimNumber = claim.ClaimNumber,
            Amount = payment.Amount,
            PaymentReference = payment.PaymentReference,
            PaymentMethod = payment.PaymentMethod,
            Status = payment.Status,
            PaidDate = payment.PaidDate,
            CreatedAt = payment.CreatedAt
        };
    }
}