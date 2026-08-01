using HealthcareClaims.Domain.Enums;

namespace HealthcareClaims.API.Models.PriorAuthorizations;

public class UpdatePriorAuthorizationRequest
{
    public string DiagnosisCode { get; set; } = string.Empty;

    public string ProcedureCode { get; set; } = string.Empty;

    public string ServiceDescription { get; set; } = string.Empty;

    public PriorAuthorizationStatus Status { get; set; }

    public string? ReviewNotes { get; set; }
}