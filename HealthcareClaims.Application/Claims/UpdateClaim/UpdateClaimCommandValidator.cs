using FluentValidation;

namespace HealthcareClaims.Application.Claims.UpdateClaim;

public class UpdateClaimCommandValidator
    : AbstractValidator<UpdateClaimCommand>
{
    public UpdateClaimCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.DiagnosisCode)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.ProcedureCode)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.BilledAmount)
            .GreaterThan(0);

        RuleFor(x => x.Remarks)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.Remarks));
    }
}