using HealthcareClaims.Application.Claims.Common;
using MediatR;

namespace HealthcareClaims.Application.Claims.GetAllClaims;

public record GetAllClaimsQuery
    : IRequest<IReadOnlyList<ClaimResponse>>;