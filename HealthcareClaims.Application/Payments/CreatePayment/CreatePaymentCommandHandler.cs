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
    private readonly IAuditService _auditService;

    public CreatePaymentCommandHandler(IApplicationDbContext context,
        IAuditService auditService)
    {
        _context = context;
        _auditService = auditService;
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

        if (request.Amount > claim.ApprovedAmount)
            throw new InvalidOperationException(
                "Payment amount cannot exceed the approved claim amount.");

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

        await _auditService.LogAsync(
            AuditAction.Create,
            "Payment",
            payment.Id,
            $"Created payment {payment.Id}",
            cancellationToken: cancellationToken);

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