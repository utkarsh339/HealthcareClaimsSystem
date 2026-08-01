using HealthcareClaims.Application.Claims.Common;
using MediatR;

namespace HealthcareClaims.Application.Claims.UpdateClaim;

public record UpdateClaimCommand(
    Guid Id,
    string DiagnosisCode,
    string ProcedureCode,
    DateOnly ServiceDate,
    decimal BilledAmount,
    string? Remarks
) : IRequest<ClaimResponse>;