using HealthcareClaims.Application.Claims.Common;
using HealthcareClaims.Domain.Enums;
using MediatR;

namespace HealthcareClaims.Application.Claims.UpdateClaimStatus;

public record UpdateClaimStatusCommand(
    Guid Id,
    ClaimStatus Status,
    decimal ApprovedAmount
) : IRequest<ClaimResponse>;
