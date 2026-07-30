using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Application.InsurancePolicies.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.InsurancePolicies.GetInsurancePolicyById;

public class GetInsurancePolicyByIdQueryHandler
    : IRequestHandler<GetInsurancePolicyByIdQuery, InsurancePolicyResponse>
{
    private readonly IApplicationDbContext _context;

    public GetInsurancePolicyByIdQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InsurancePolicyResponse> Handle(
        GetInsurancePolicyByIdQuery request,
        CancellationToken cancellationToken)
    {
        var insurancePolicy = await _context.InsurancePolicies
            .AsNoTracking()
            .FirstOrDefaultAsync(
                p => p.Id == request.Id,
                cancellationToken);

        if (insurancePolicy is null)
        {
            throw new KeyNotFoundException("Insurance policy not found.");
        }

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