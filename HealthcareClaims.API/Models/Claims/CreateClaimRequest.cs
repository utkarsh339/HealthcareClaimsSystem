using System.ComponentModel.DataAnnotations;

namespace HealthcareClaims.API.Models.Claims;

public class CreateClaimRequest
{
    [Required]
    public Guid PatientId { get; set; }

    [Required]
    public Guid ProviderId { get; set; }

    [Required]
    public Guid InsurancePolicyId { get; set; }

    [Required]
    public Guid PriorAuthorizationId { get; set; }

    [Required]
    [MaxLength(20)]
    public string DiagnosisCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string ProcedureCode { get; set; } = string.Empty;

    [Required]
    public DateOnly ServiceDate { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal BilledAmount { get; set; }

    public string? Remarks { get; set; }
}