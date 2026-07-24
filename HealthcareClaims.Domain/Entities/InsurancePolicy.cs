namespace HealthcareClaims.Domain.Entities;

public class InsurancePolicy
{
    public Guid Id { get; set; }

    public string PolicyNumber { get; set; } = string.Empty;

    public string InsuranceCompany { get; set; } = string.Empty;

    public string PlanName { get; set; } = string.Empty;

    public decimal CoveragePercentage { get; set; }

    public decimal DeductibleAmount { get; set; }

    public decimal OutOfPocketMaximum { get; set; }

    public DateOnly EffectiveDate { get; set; }

    public DateOnly ExpirationDate { get; set; }

    public bool IsActive { get; set; }

    public Guid PatientId { get; set; }

    public Patient Patient { get; set; } = null!;
}