using HealthcareClaims.Application.InsurancePolicies.Common;
using MediatR;

namespace HealthcareClaims.Application.InsurancePolicies.UpdateInsurancePolicy;

public record UpdateInsurancePolicyCommand(
    Guid Id,
    string InsuranceCompany,
    string PolicyNumber,
    string MemberId,
    string GroupNumber,
    string PlanName,
    DateOnly EffectiveDate,
    DateOnly ExpirationDate,
    bool IsActive
) : IRequest<InsurancePolicyResponse>;