namespace HealthcareClaims.Domain.Entities;

public class PriorAuthorization
{
    public Guid Id { get; set; }

    public string AuthorizationNumber { get; set; } = string.Empty;

    public Guid PatientId { get; set; }

    public Guid ProviderId { get; set; }

    public Guid InsurancePolicyId { get; set; }

    public string RequestedProcedureCode { get; set; } = string.Empty;

    public string DiagnosisCode { get; set; } = string.Empty;

    public DateTime RequestedAt { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public string Status { get; set; } = string.Empty;

    public string? ReviewerComments { get; set; }

    public Patient Patient { get; set; } = null!;

    public Provider Provider { get; set; } = null!;

    public InsurancePolicy InsurancePolicy { get; set; } = null!;
}