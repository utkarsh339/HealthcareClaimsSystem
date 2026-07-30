using FluentValidation;

namespace HealthcareClaims.Application.InsurancePolicies.CreateInsurancePolicy;

public class CreateInsurancePolicyValidator
    : AbstractValidator<CreateInsurancePolicyCommand>
{
    public CreateInsurancePolicyValidator()
    {
        RuleFor(x => x.PatientId)
            .NotEmpty();

        RuleFor(x => x.InsuranceCompany)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.PolicyNumber)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.MemberId)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.GroupNumber)
            .MaximumLength(100);

        RuleFor(x => x.PlanName)
            .MaximumLength(150);

        RuleFor(x => x.EffectiveDate)
            .NotEmpty();

        RuleFor(x => x.ExpirationDate)
            .GreaterThan(x => x.EffectiveDate)
            .WithMessage("Expiration date must be later than the effective date.");
    }
}