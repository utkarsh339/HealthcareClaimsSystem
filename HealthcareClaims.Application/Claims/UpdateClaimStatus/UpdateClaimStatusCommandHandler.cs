using HealthcareClaims.Application.Claims.Common;
using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.Claims.UpdateClaimStatus;

public class UpdateClaimStatusCommandHandler : IRequestHandler<UpdateClaimStatusCommand, ClaimResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuditService _auditService;

    public UpdateClaimStatusCommandHandler(IApplicationDbContext context, IAuditService auditService)
    {
        _context = context;
        _auditService = auditService;
    }

    public async Task<ClaimResponse> Handle(UpdateClaimStatusCommand request, CancellationToken cancellationToken)
    {
        var claim = await _context.Claims.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (claim is null)
            throw new KeyNotFoundException("Claim not found.");

        if (!IsValidTransition(claim.Status, request.Status))
            throw new InvalidOperationException($"Claim cannot move from {claim.Status} to {request.Status}.");

        if (request.Status == ClaimStatus.Approved)
        {
            if (request.ApprovedAmount <= 0 || request.ApprovedAmount > claim.BilledAmount)
                throw new InvalidOperationException("Approved amount must be greater than zero and cannot exceed the billed amount.");
        }
        else if (request.Status == ClaimStatus.PartiallyApproved)
        {
            if (request.ApprovedAmount <= 0 || request.ApprovedAmount >= claim.BilledAmount)
                throw new InvalidOperationException("Partially approved amount must be greater than zero and less than the billed amount.");
        }

        if (request.Status == ClaimStatus.Cancelled && claim.Status is (ClaimStatus.Approved or ClaimStatus.PartiallyApproved))
        {
            var hasPayment = await _context.Payments.AnyAsync(p => p.ClaimId == claim.Id, cancellationToken);
            if (hasPayment)
                throw new InvalidOperationException("A claim with an existing payment cannot be cancelled.");
        }

        var previousStatus = claim.Status;
        claim.Status = request.Status;

        if (request.Status is ClaimStatus.Approved or ClaimStatus.PartiallyApproved)
            claim.ApprovedAmount = request.ApprovedAmount;
        else if (request.Status is ClaimStatus.Denied or ClaimStatus.Cancelled)
            claim.ApprovedAmount = 0;

        if (request.Status == ClaimStatus.Submitted && claim.SubmittedDate is null)
            claim.SubmittedDate = DateTime.UtcNow;

        if (request.Status is ClaimStatus.Approved or ClaimStatus.PartiallyApproved or ClaimStatus.Denied)
            claim.ProcessedDate = DateTime.UtcNow;

        await _auditService.LogAsync(
            AuditAction.Update,
            "Claim",
            claim.Id,
            $"Changed claim {claim.ClaimNumber} status from {previousStatus} to {claim.Status}",
            cancellationToken: cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return new ClaimResponse
        {
            Id = claim.Id,
            ClaimNumber = claim.ClaimNumber,
            PatientId = claim.PatientId,
            ProviderId = claim.ProviderId,
            InsurancePolicyId = claim.InsurancePolicyId,
            PriorAuthorizationId = claim.PriorAuthorizationId,
            DiagnosisCode = claim.DiagnosisCode,
            ProcedureCode = claim.ProcedureCode,
            ServiceDate = claim.ServiceDate,
            BilledAmount = claim.BilledAmount,
            ApprovedAmount = claim.ApprovedAmount,
            Status = claim.Status,
            SubmittedDate = claim.SubmittedDate,
            ProcessedDate = claim.ProcessedDate,
            Remarks = claim.Remarks,
            CreatedAt = claim.CreatedAt
        };
    }

    private static bool IsValidTransition(ClaimStatus current, ClaimStatus next) => current switch
    {
        ClaimStatus.Draft => next is ClaimStatus.Submitted or ClaimStatus.Cancelled,
        ClaimStatus.Submitted => next is ClaimStatus.UnderReview or ClaimStatus.Cancelled,
        ClaimStatus.UnderReview => next is ClaimStatus.Approved or ClaimStatus.PartiallyApproved or ClaimStatus.Denied or ClaimStatus.Cancelled,
        ClaimStatus.Approved => next == ClaimStatus.Cancelled,
        ClaimStatus.PartiallyApproved => next == ClaimStatus.Cancelled,
        ClaimStatus.Denied => false,
        ClaimStatus.Paid => false,
        ClaimStatus.Cancelled => false,
        _ => false
    };
}
