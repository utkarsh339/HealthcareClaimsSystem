using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Application.PriorAuthorizations.Common;
using HealthcareClaims.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.PriorAuthorizations.UpdatePriorAuthorization;

public class UpdatePriorAuthorizationCommandHandler
    : IRequestHandler<UpdatePriorAuthorizationCommand, PriorAuthorizationResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuditService _auditService;

    public UpdatePriorAuthorizationCommandHandler(
        IApplicationDbContext context,
        IAuditService auditService)
    {
        _context = context;
        _auditService = auditService;
    }

    public async Task<PriorAuthorizationResponse> Handle(
        UpdatePriorAuthorizationCommand request,
        CancellationToken cancellationToken)
    {
        var priorAuthorization = await _context.PriorAuthorizations
            .FirstOrDefaultAsync(
                pa => pa.Id == request.Id,
                cancellationToken);

        if (priorAuthorization is null)
        {
            throw new KeyNotFoundException("Prior authorization not found.");
        }

        priorAuthorization.DiagnosisCode = request.DiagnosisCode;
        priorAuthorization.ProcedureCode = request.ProcedureCode;
        priorAuthorization.ServiceDescription = request.ServiceDescription;
        priorAuthorization.Status = request.Status;
        priorAuthorization.ReviewNotes = request.ReviewNotes;

        if (request.Status == PriorAuthorizationStatus.Pending)
        {
            priorAuthorization.DecisionDate = null;
        }
        else
        {
            priorAuthorization.DecisionDate = DateTime.UtcNow;
        }

        await _auditService.LogAsync(
            AuditAction.Update,
            "PriorAuthorization",
            priorAuthorization.Id,
            $"Updated prior authorization {priorAuthorization.Id}",
            cancellationToken: cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

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