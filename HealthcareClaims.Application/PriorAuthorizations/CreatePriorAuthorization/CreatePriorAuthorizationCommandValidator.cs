using FluentValidation;

namespace HealthcareClaims.Application.PriorAuthorizations.CreatePriorAuthorization;

public class CreatePriorAuthorizationCommandValidator
    : AbstractValidator<CreatePriorAuthorizationCommand>
{
    public CreatePriorAuthorizationCommandValidator()
    {
        RuleFor(x => x.PatientId)
            .NotEmpty();

        RuleFor(x => x.ProviderId)
            .NotEmpty();

        RuleFor(x => x.InsurancePolicyId)
            .NotEmpty();

        RuleFor(x => x.DiagnosisCode)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.ProcedureCode)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.ServiceDescription)
            .NotEmpty()
            .MaximumLength(500);
    }
}