using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Application.PriorAuthorizations.Common;
using HealthcareClaims.Domain.Entities;
using HealthcareClaims.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.PriorAuthorizations.CreatePriorAuthorization;

public class CreatePriorAuthorizationCommandHandler
    : IRequestHandler<CreatePriorAuthorizationCommand, PriorAuthorizationResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuditService _auditService;

    public CreatePriorAuthorizationCommandHandler(
        IApplicationDbContext context,
        IAuditService auditService)
    {
        _context = context;
        _auditService = auditService;
    }

    public async Task<PriorAuthorizationResponse> Handle(
        CreatePriorAuthorizationCommand request,
        CancellationToken cancellationToken)
    {
        var patientExists = await _context.Patients
            .AnyAsync(p => p.Id == request.PatientId, cancellationToken);

        if (!patientExists)
        {
            throw new KeyNotFoundException("Patient not found.");
        }

        var providerExists = await _context.Providers
            .AnyAsync(p => p.Id == request.ProviderId, cancellationToken);

        if (!providerExists)
        {
            throw new KeyNotFoundException("Provider not found.");
        }

        var insurancePolicy = await _context.InsurancePolicies
    .FirstOrDefaultAsync(
        ip => ip.Id == request.InsurancePolicyId,
        cancellationToken);

        if (insurancePolicy is null)
        {
            throw new KeyNotFoundException("Insurance policy not found.");
        }

        if (insurancePolicy.PatientId != request.PatientId)
        {
            throw new InvalidOperationException(
                "The selected insurance policy does not belong to the specified patient.");
        }

        var priorAuthorization = new PriorAuthorization
        {
            Id = Guid.NewGuid(),
            PatientId = request.PatientId,
            ProviderId = request.ProviderId,
            InsurancePolicyId = request.InsurancePolicyId,
            DiagnosisCode = request.DiagnosisCode,
            ProcedureCode = request.ProcedureCode,
            ServiceDescription = request.ServiceDescription,
            RequestedDate = DateTime.UtcNow,
            Status = PriorAuthorizationStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _context.PriorAuthorizations.Add(priorAuthorization);

        await _auditService.LogAsync(
            AuditAction.Create,
            "PriorAuthorization",
            priorAuthorization.Id,
            $"Created prior authorization {priorAuthorization.Id}",
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