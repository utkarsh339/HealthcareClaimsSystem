namespace HealthcareClaims.Application.PriorAuthorizations.Common;

using HealthcareClaims.Domain.Enums;

public class PriorAuthorizationResponse
{
    public Guid Id { get; set; }

    public Guid PatientId { get; set; }

    public Guid ProviderId { get; set; }

    public Guid InsurancePolicyId { get; set; }

    public string DiagnosisCode { get; set; } = string.Empty;

    public string ProcedureCode { get; set; } = string.Empty;

    public string ServiceDescription { get; set; } = string.Empty;

    public DateTime RequestedDate { get; set; }

    public PriorAuthorizationStatus Status { get; set; }

    public string? ReviewNotes { get; set; }

    public DateTime? DecisionDate { get; set; }

    public DateTime CreatedAt { get; set; }
}