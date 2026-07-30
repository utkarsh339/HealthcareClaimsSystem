using MediatR;

namespace HealthcareClaims.Application.InsurancePolicies.DeleteInsurancePolicy;

public record DeleteInsurancePolicyCommand(Guid Id)
    : IRequest;