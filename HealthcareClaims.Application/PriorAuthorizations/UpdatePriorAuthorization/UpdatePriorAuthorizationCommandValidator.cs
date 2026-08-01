using FluentValidation;

namespace HealthcareClaims.Application.PriorAuthorizations.UpdatePriorAuthorization;

public class UpdatePriorAuthorizationCommandValidator
    : AbstractValidator<UpdatePriorAuthorizationCommand>
{
    public UpdatePriorAuthorizationCommandValidator()
    {
        RuleFor(x => x.Id)
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

        RuleFor(x => x.Status)
            .IsInEnum();

        RuleFor(x => x.ReviewNotes)
            .MaximumLength(1000);
    }
}