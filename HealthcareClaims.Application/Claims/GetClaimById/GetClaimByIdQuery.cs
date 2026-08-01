using HealthcareClaims.Application.Claims.Common;
using MediatR;

namespace HealthcareClaims.Application.Claims.GetClaimById;

public record GetClaimByIdQuery(Guid Id)
    : IRequest<ClaimResponse>;