using HealthcareClaims.Application.Claims.Common;
using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Domain.Entities;
using HealthcareClaims.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.Claims.CreateClaim;

public class CreateClaimCommandHandler
    : IRequestHandler<CreateClaimCommand, ClaimResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateClaimCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ClaimResponse> Handle(
        CreateClaimCommand request,
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

        var priorAuthorization = await _context.PriorAuthorizations
            .FirstOrDefaultAsync(
                pa => pa.Id == request.PriorAuthorizationId,
                cancellationToken);

        if (priorAuthorization is null)
        {
            throw new KeyNotFoundException("Prior authorization not found.");
        }

        if (priorAuthorization.PatientId != request.PatientId)
        {
            throw new InvalidOperationException(
                "The selected prior authorization does not belong to the specified patient.");
        }

        if (priorAuthorization.ProviderId != request.ProviderId)
        {
            throw new InvalidOperationException(
                "The selected prior authorization does not belong to the specified provider.");
        }

        if (priorAuthorization.InsurancePolicyId != request.InsurancePolicyId)
        {
            throw new InvalidOperationException(
                "The selected prior authorization does not belong to the specified insurance policy.");
        }

        var claim = new Claim
        {
            Id = Guid.NewGuid(),
            ClaimNumber = $"CLM-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(100000, 999999)}",
            PatientId = request.PatientId,
            ProviderId = request.ProviderId,
            InsurancePolicyId = request.InsurancePolicyId,
            PriorAuthorizationId = request.PriorAuthorizationId,
            DiagnosisCode = request.DiagnosisCode,
            ProcedureCode = request.ProcedureCode,
            ServiceDate = request.ServiceDate,
            BilledAmount = request.BilledAmount,
            ApprovedAmount = 0,
            Status = ClaimStatus.Draft,
            Remarks = request.Remarks,
            CreatedAt = DateTime.UtcNow
        };

        _context.Claims.Add(claim);

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