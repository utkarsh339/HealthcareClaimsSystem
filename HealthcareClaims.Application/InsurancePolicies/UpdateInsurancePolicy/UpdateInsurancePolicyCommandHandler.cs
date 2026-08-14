using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Application.InsurancePolicies.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HealthcareClaims.Domain.Enums;

namespace HealthcareClaims.Application.InsurancePolicies.UpdateInsurancePolicy;

public class UpdateInsurancePolicyCommandHandler
    : IRequestHandler<UpdateInsurancePolicyCommand, InsurancePolicyResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuditService _auditService;

    public UpdateInsurancePolicyCommandHandler(
        IApplicationDbContext context,
        IAuditService auditService)
    {
        _context = context;
        _auditService = auditService;
    }

    public async Task<InsurancePolicyResponse> Handle(
        UpdateInsurancePolicyCommand request,
        CancellationToken cancellationToken)
    {
        var insurancePolicy = await _context.InsurancePolicies
            .FirstOrDefaultAsync(
                ip => ip.Id == request.Id,
                cancellationToken);

        if (insurancePolicy is null)
        {
            throw new KeyNotFoundException("Insurance policy not found.");
        }

        // Check duplicate Policy Number only if it has changed
        if (!string.Equals(
                insurancePolicy.PolicyNumber,
                request.PolicyNumber,
                StringComparison.OrdinalIgnoreCase))
        {
            var policyNumberExists = await _context.InsurancePolicies
                .AnyAsync(
                    ip => ip.PolicyNumber == request.PolicyNumber &&
                          ip.Id != request.Id,
                    cancellationToken);

            if (policyNumberExists)
            {
                throw new InvalidOperationException("Policy number already exists.");
            }
        }

        insurancePolicy.InsuranceCompany = request.InsuranceCompany;
        insurancePolicy.PolicyNumber = request.PolicyNumber;
        insurancePolicy.MemberId = request.MemberId;
        insurancePolicy.GroupNumber = request.GroupNumber;
        insurancePolicy.PlanName = request.PlanName;
        insurancePolicy.EffectiveDate = request.EffectiveDate;
        insurancePolicy.ExpirationDate = request.ExpirationDate;
        insurancePolicy.IsActive = request.IsActive;

        await _auditService.LogAsync(
            AuditAction.Update,
            "InsurancePolicy",
            insurancePolicy.Id,
            $"Updated insurance policy {insurancePolicy.Id}",
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
