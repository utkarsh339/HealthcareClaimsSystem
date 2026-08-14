using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Application.Payments.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HealthcareClaims.Domain.Enums;

namespace HealthcareClaims.Application.Payments.UpdatePayment;

public class UpdatePaymentCommandHandler
    : IRequestHandler<UpdatePaymentCommand, PaymentResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuditService _auditService;

    public UpdatePaymentCommandHandler(IApplicationDbContext context,
        IAuditService auditService)
    {
        _context = context;
        _auditService = auditService;
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

        if (request.Amount > payment.Claim.ApprovedAmount)
            throw new InvalidOperationException(
                "Payment amount cannot exceed the approved claim amount.");

        if (payment.Status == PaymentStatus.Completed && request.Status != PaymentStatus.Completed)
            throw new InvalidOperationException(
                "A completed payment cannot be moved back to another status.");

        if (request.Status == PaymentStatus.Completed && payment.Claim.Status != ClaimStatus.Approved && payment.Claim.Status != ClaimStatus.Paid)
            throw new InvalidOperationException(
                "A payment can only be completed for an approved claim.");

        if (request.Status == PaymentStatus.Completed && request.Amount != payment.Claim.ApprovedAmount)
            throw new InvalidOperationException(
                "A completed payment must equal the approved claim amount.");

        payment.Amount = request.Amount;
        payment.PaymentMethod = request.PaymentMethod;
        payment.Status = request.Status;
        payment.PaidDate = request.Status == PaymentStatus.Completed
            ? request.PaidDate ?? DateTime.UtcNow
            : request.PaidDate;

        if (request.Status == PaymentStatus.Completed)
            payment.Claim.Status = ClaimStatus.Paid;

        await _auditService.LogAsync(
            AuditAction.Update,
            "Payment",
            payment.Id,
            $"Updated payment {payment.Id}",
            cancellationToken: cancellationToken);

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
