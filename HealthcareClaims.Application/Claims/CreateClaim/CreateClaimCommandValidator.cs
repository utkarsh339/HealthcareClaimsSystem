using FluentValidation;

namespace HealthcareClaims.Application.Claims.CreateClaim;

public class CreateClaimCommandValidator
    : AbstractValidator<CreateClaimCommand>
{
    public CreateClaimCommandValidator()
    {
        RuleFor(x => x.PatientId)
            .NotEmpty();

        RuleFor(x => x.ProviderId)
            .NotEmpty();

        RuleFor(x => x.InsurancePolicyId)
            .NotEmpty();

        RuleFor(x => x.PriorAuthorizationId)
            .NotEmpty();

        RuleFor(x => x.DiagnosisCode)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.ProcedureCode)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.BilledAmount)
            .GreaterThan(0);

        RuleFor(x => x.ServiceDate)
            .NotEmpty();

        RuleFor(x => x.Remarks)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.Remarks));
    }
}