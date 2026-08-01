using HealthcareClaims.Application.Claims.Common;
using MediatR;

namespace HealthcareClaims.Application.Claims.CreateClaim;

public record CreateClaimCommand(
    Guid PatientId,
    Guid ProviderId,
    Guid InsurancePolicyId,
    Guid PriorAuthorizationId,
    string DiagnosisCode,
    string ProcedureCode,
    DateOnly ServiceDate,
    decimal BilledAmount,
    string? Remarks
) : IRequest<ClaimResponse>;