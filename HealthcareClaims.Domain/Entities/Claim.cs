namespace HealthcareClaims.Domain.Entities;

public class Claim
{
    public Guid Id { get; set; }

    public string ClaimNumber { get; set; } = string.Empty;

    public Guid PatientId { get; set; }

    public Guid ProviderId { get; set; }

    public Guid InsurancePolicyId { get; set; }

    public DateOnly ServiceDate { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal ApprovedAmount { get; set; }

    public decimal PatientResponsibilityAmount { get; set; }

    public string DiagnosisCode { get; set; } = string.Empty;

    public string ProcedureCode { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime SubmittedAt { get; set; }

    public Patient Patient { get; set; } = null!;

    public Provider Provider { get; set; } = null!;

    public InsurancePolicy InsurancePolicy { get; set; } = null!;
}