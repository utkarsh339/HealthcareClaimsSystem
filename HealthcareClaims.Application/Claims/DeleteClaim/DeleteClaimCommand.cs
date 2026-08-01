using MediatR;

namespace HealthcareClaims.Application.Claims.DeleteClaim;

public record DeleteClaimCommand(Guid Id) : IRequest;