using FluentValidation;

namespace HealthcareClaims.Application.InsurancePolicies.UpdateInsurancePolicy;

public class UpdateInsurancePolicyValidator
    : AbstractValidator<UpdateInsurancePolicyCommand>
{
    public UpdateInsurancePolicyValidator()
    {
        RuleFor(x => x.Id)
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

        RuleFor(x => x.ExpirationDate)
            .GreaterThan(x => x.EffectiveDate)
            .WithMessage("Expiration date must be later than the effective date.");
    }
}