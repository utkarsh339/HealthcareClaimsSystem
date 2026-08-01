using HealthcareClaims.Domain.Enums;

namespace HealthcareClaims.Application.Claims.Common;

public class ClaimResponse
{
    public Guid Id { get; set; }

    public string ClaimNumber { get; set; } = string.Empty;

    public Guid PatientId { get; set; }

    public Guid ProviderId { get; set; }

    public Guid InsurancePolicyId { get; set; }

    public Guid PriorAuthorizationId { get; set; }

    public string DiagnosisCode { get; set; } = string.Empty;

    public string ProcedureCode { get; set; } = string.Empty;

    public DateOnly ServiceDate { get; set; }

    public decimal BilledAmount { get; set; }

    public decimal ApprovedAmount { get; set; }

    public ClaimStatus Status { get; set; }

    public DateTime? SubmittedDate { get; set; }

    public DateTime? ProcessedDate { get; set; }

    public string? Remarks { get; set; }

    public DateTime CreatedAt { get; set; }
}