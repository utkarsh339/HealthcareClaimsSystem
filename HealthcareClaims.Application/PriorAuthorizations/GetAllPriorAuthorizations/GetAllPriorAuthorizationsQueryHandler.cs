using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Application.PriorAuthorizations.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.PriorAuthorizations.GetAllPriorAuthorizations;

public class GetAllPriorAuthorizationsQueryHandler
    : IRequestHandler<GetAllPriorAuthorizationsQuery, IReadOnlyList<PriorAuthorizationResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetAllPriorAuthorizationsQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<PriorAuthorizationResponse>> Handle(
        GetAllPriorAuthorizationsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.PriorAuthorizations
            .AsNoTracking()
            .OrderBy(pa => pa.CreatedAt)
            .Select(pa => new PriorAuthorizationResponse
            {
                Id = pa.Id,
                PatientId = pa.PatientId,
                ProviderId = pa.ProviderId,
                InsurancePolicyId = pa.InsurancePolicyId,
                DiagnosisCode = pa.DiagnosisCode,
                ProcedureCode = pa.ProcedureCode,
                ServiceDescription = pa.ServiceDescription,
                RequestedDate = pa.RequestedDate,
                Status = pa.Status,
                ReviewNotes = pa.ReviewNotes,
                DecisionDate = pa.DecisionDate,
                CreatedAt = pa.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }
}