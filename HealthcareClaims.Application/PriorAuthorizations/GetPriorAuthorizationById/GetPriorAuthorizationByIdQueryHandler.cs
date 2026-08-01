using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Application.PriorAuthorizations.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.PriorAuthorizations.GetPriorAuthorizationById;

public class GetPriorAuthorizationByIdQueryHandler
    : IRequestHandler<GetPriorAuthorizationByIdQuery, PriorAuthorizationResponse>
{
    private readonly IApplicationDbContext _context;

    public GetPriorAuthorizationByIdQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PriorAuthorizationResponse> Handle(
        GetPriorAuthorizationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var priorAuthorization = await _context.PriorAuthorizations
            .AsNoTracking()
            .FirstOrDefaultAsync(
                pa => pa.Id == request.Id,
                cancellationToken);

        if (priorAuthorization is null)
        {
            throw new KeyNotFoundException("Prior authorization not found.");
        }

        return new PriorAuthorizationResponse
        {
            Id = priorAuthorization.Id,
            PatientId = priorAuthorization.PatientId,
            ProviderId = priorAuthorization.ProviderId,
            InsurancePolicyId = priorAuthorization.InsurancePolicyId,
            DiagnosisCode = priorAuthorization.DiagnosisCode,
            ProcedureCode = priorAuthorization.ProcedureCode,
            ServiceDescription = priorAuthorization.ServiceDescription,
            RequestedDate = priorAuthorization.RequestedDate,
            Status = priorAuthorization.Status,
            ReviewNotes = priorAuthorization.ReviewNotes,
            DecisionDate = priorAuthorization.DecisionDate,
            CreatedAt = priorAuthorization.CreatedAt
        };
    }
}