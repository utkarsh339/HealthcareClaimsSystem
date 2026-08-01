namespace HealthcareClaims.API.Models.PriorAuthorizations;

public class CreatePriorAuthorizationRequest
{
    public Guid PatientId { get; set; }

    public Guid ProviderId { get; set; }

    public Guid InsurancePolicyId { get; set; }

    public string DiagnosisCode { get; set; } = string.Empty;

    public string ProcedureCode { get; set; } = string.Empty;

    public string ServiceDescription { get; set; } = string.Empty;
}