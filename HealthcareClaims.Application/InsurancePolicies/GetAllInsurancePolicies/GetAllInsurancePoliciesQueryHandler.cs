using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Application.InsurancePolicies.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.InsurancePolicies.GetAllInsurancePolicies;

public class GetAllInsurancePoliciesQueryHandler
    : IRequestHandler<GetAllInsurancePoliciesQuery, IReadOnlyList<InsurancePolicyResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetAllInsurancePoliciesQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<InsurancePolicyResponse>> Handle(
        GetAllInsurancePoliciesQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.InsurancePolicies
            .AsNoTracking()
            .OrderBy(ip => ip.InsuranceCompany)
            .ThenBy(ip => ip.PolicyNumber)
            .Select(ip => new InsurancePolicyResponse
            {
                Id = ip.Id,
                PatientId = ip.PatientId,
                InsuranceCompany = ip.InsuranceCompany,
                PolicyNumber = ip.PolicyNumber,
                MemberId = ip.MemberId,
                GroupNumber = ip.GroupNumber,
                PlanName = ip.PlanName,
                EffectiveDate = ip.EffectiveDate,
                ExpirationDate = ip.ExpirationDate,
                IsActive = ip.IsActive,
                CreatedAt = ip.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }
}