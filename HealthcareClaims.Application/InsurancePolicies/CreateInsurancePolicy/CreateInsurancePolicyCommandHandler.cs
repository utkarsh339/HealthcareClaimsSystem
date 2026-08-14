using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Application.InsurancePolicies.Common;
using HealthcareClaims.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HealthcareClaims.Domain.Enums;

namespace HealthcareClaims.Application.InsurancePolicies.CreateInsurancePolicy;

public class CreateInsurancePolicyCommandHandler
    : IRequestHandler<CreateInsurancePolicyCommand, InsurancePolicyResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuditService _auditService;

    public CreateInsurancePolicyCommandHandler(
        IApplicationDbContext context,
        IAuditService auditService)
    {
        _context = context;
        _auditService = auditService;
    }

    public async Task<InsurancePolicyResponse> Handle(
        CreateInsurancePolicyCommand request,
        CancellationToken cancellationToken)
    {
        var patientExists = await _context.Patients
            .AnyAsync(p => p.Id == request.PatientId, cancellationToken);

        if (!patientExists)
        {
            throw new KeyNotFoundException("Patient not found.");
        }

        var policyExists = await _context.InsurancePolicies
            .AnyAsync(p => p.PolicyNumber == request.PolicyNumber, cancellationToken);

        if (policyExists)
        {
            throw new InvalidOperationException("Policy number already exists.");
        }

        var insurancePolicy = new InsurancePolicy
        {
            Id = Guid.NewGuid(),
            PatientId = request.PatientId,
            InsuranceCompany = request.InsuranceCompany,
            PolicyNumber = request.PolicyNumber,
            MemberId = request.MemberId,
            GroupNumber = request.GroupNumber,
            PlanName = request.PlanName,
            EffectiveDate = request.EffectiveDate,
            ExpirationDate = request.ExpirationDate,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        _context.InsurancePolicies.Add(insurancePolicy);

        await _auditService.LogAsync(
            AuditAction.Create,
            "InsurancePolicy",
            insurancePolicy.Id,
            $"Created insurance policy {insurancePolicy.Id}",
            cancellationToken: cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return new InsurancePolicyResponse
        {
            Id = insurancePolicy.Id,
            PatientId = insurancePolicy.PatientId,
            InsuranceCompany = insurancePolicy.InsuranceCompany,
            PolicyNumber = insurancePolicy.PolicyNumber,
            MemberId = insurancePolicy.MemberId,
            GroupNumber = insurancePolicy.GroupNumber,
            PlanName = insurancePolicy.PlanName,
            EffectiveDate = insurancePolicy.EffectiveDate,
            ExpirationDate = insurancePolicy.ExpirationDate,
            IsActive = insurancePolicy.IsActive,
            CreatedAt = insurancePolicy.CreatedAt
        };
    }
}
