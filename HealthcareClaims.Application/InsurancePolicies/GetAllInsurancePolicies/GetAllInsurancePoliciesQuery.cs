using HealthcareClaims.Application.InsurancePolicies.Common;
using MediatR;

namespace HealthcareClaims.Application.InsurancePolicies.GetAllInsurancePolicies;

public record GetAllInsurancePoliciesQuery
    : IRequest<IReadOnlyList<InsurancePolicyResponse>>;