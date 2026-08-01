using HealthcareClaims.Application.PriorAuthorizations.Common;
using HealthcareClaims.Domain.Enums;
using MediatR;

namespace HealthcareClaims.Application.PriorAuthorizations.UpdatePriorAuthorization;

public class UpdatePriorAuthorizationCommand : IRequest<PriorAuthorizationResponse>
{
    public Guid Id { get; set; }

    public string DiagnosisCode { get; set; } = string.Empty;

    public string ProcedureCode { get; set; } = string.Empty;

    public string ServiceDescription { get; set; } = string.Empty;

    public PriorAuthorizationStatus Status { get; set; }

    public string? ReviewNotes { get; set; }
}