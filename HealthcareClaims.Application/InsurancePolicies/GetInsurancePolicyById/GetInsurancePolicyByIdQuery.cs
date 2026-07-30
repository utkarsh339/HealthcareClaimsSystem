using HealthcareClaims.Application.InsurancePolicies.Common;
using MediatR;

namespace HealthcareClaims.Application.InsurancePolicies.GetInsurancePolicyById;

public record GetInsurancePolicyByIdQuery(Guid Id)
    : IRequest<InsurancePolicyResponse>;