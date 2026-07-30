namespace HealthcareClaims.Application.InsurancePolicies.Common;

public class InsurancePolicyResponse
{
    public Guid Id { get; set; }

    public Guid PatientId { get; set; }

    public string InsuranceCompany { get; set; } = string.Empty;

    public string PolicyNumber { get; set; } = string.Empty;

    public string MemberId { get; set; } = string.Empty;

    public string GroupNumber { get; set; } = string.Empty;

    public string PlanName { get; set; } = string.Empty;

    public DateOnly EffectiveDate { get; set; }

    public DateOnly ExpirationDate { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}