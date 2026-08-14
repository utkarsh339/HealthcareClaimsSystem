using HealthcareClaims.Application.Claims.Common;
using HealthcareClaims.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HealthcareClaims.Domain.Enums;

namespace HealthcareClaims.Application.Claims.UpdateClaim;

public class UpdateClaimCommandHandler
    : IRequestHandler<UpdateClaimCommand, ClaimResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuditService _auditService;

    public UpdateClaimCommandHandler(
        IApplicationDbContext context,
        IAuditService auditService)
    {
        _context = context;
        _auditService = auditService;
    }

    public async Task<ClaimResponse> Handle(
        UpdateClaimCommand request,
        CancellationToken cancellationToken)
    {
        var claim = await _context.Claims
            .FirstOrDefaultAsync(
                c => c.Id == request.Id,
                cancellationToken);

        if (claim is null)
        {
            throw new KeyNotFoundException("Claim not found.");
        }

        if (claim.Status is not (ClaimStatus.Draft or ClaimStatus.Submitted))
        {
            throw new InvalidOperationException(
                "Claim details can only be edited while the claim is Draft or Submitted.");
        }

        claim.DiagnosisCode = request.DiagnosisCode;
        claim.ProcedureCode = request.ProcedureCode;
        claim.ServiceDate = request.ServiceDate;
        claim.BilledAmount = request.BilledAmount;
        claim.Remarks = request.Remarks;

        await _auditService.LogAsync(
            AuditAction.Update,
            "Claim",
            claim.Id,
            $"Updated claim {claim.Id}",
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
}
