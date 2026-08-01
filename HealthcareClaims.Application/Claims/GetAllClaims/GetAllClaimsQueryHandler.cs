using HealthcareClaims.Application.Claims.Common;
using HealthcareClaims.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.Claims.GetAllClaims;

public class GetAllClaimsQueryHandler
    : IRequestHandler<GetAllClaimsQuery, IReadOnlyList<ClaimResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetAllClaimsQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ClaimResponse>> Handle(
        GetAllClaimsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Claims
            .AsNoTracking()
            .OrderByDescending(c => c.CreatedAt)
            .Select(claim => new ClaimResponse
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
            })
            .ToListAsync(cancellationToken);
    }
}