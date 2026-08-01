using HealthcareClaims.Domain.Enums;

namespace HealthcareClaims.Domain.Entities;

public class PriorAuthorization
{
    public Guid Id { get; set; }

    public Guid PatientId { get; set; }

    public Guid ProviderId { get; set; }

    public Guid InsurancePolicyId { get; set; }

    public string DiagnosisCode { get; set; } = string.Empty;

    public string ProcedureCode { get; set; } = string.Empty;

    public string ServiceDescription { get; set; } = string.Empty;

    public DateTime RequestedDate { get; set; } = DateTime.UtcNow;

    public PriorAuthorizationStatus Status { get; set; } = PriorAuthorizationStatus.Pending;

    public string? ReviewNotes { get; set; }

    public DateTime? DecisionDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public Patient Patient { get; set; } = null!;

    public Provider Provider { get; set; } = null!;

    public InsurancePolicy InsurancePolicy { get; set; } = null!;
}