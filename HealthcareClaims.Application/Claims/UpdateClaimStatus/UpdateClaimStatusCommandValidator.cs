using FluentValidation;
using HealthcareClaims.Domain.Enums;

namespace HealthcareClaims.Application.Claims.UpdateClaimStatus;

public class UpdateClaimStatusCommandValidator : AbstractValidator<UpdateClaimStatusCommand>
{
    public UpdateClaimStatusCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Status).IsInEnum();
        RuleFor(x => x.ApprovedAmount).GreaterThanOrEqualTo(0);
    }
}
