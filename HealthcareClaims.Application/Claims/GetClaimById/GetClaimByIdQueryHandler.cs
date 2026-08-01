using HealthcareClaims.Application.Claims.Common;
using HealthcareClaims.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.Claims.GetClaimById;

public class GetClaimByIdQueryHandler
    : IRequestHandler<GetClaimByIdQuery, ClaimResponse>
{
    private readonly IApplicationDbContext _context;

    public GetClaimByIdQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ClaimResponse> Handle(
        GetClaimByIdQuery request,
        CancellationToken cancellationToken)
    {
        var claim = await _context.Claims
            .AsNoTracking()
            .FirstOrDefaultAsync(
                c => c.Id == request.Id,
                cancellationToken);

        if (claim is null)
        {
            throw new KeyNotFoundException("Claim not found.");
        }

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