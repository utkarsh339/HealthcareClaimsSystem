using System.ComponentModel.DataAnnotations;

namespace HealthcareClaims.API.Models.Claims;

public class UpdateClaimRequest
{
    [Required]
    [MaxLength(50)]
    public string DiagnosisCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ProcedureCode { get; set; } = string.Empty;

    [Required]
    public DateOnly ServiceDate { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal BilledAmount { get; set; }

    [MaxLength(1000)]
    public string? Remarks { get; set; }
}