using FluentValidation;

namespace HealthcareClaims.Application.Payments.UpdatePayment;

public class UpdatePaymentCommandValidator
    : AbstractValidator<UpdatePaymentCommand>
{
    public UpdatePaymentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.PaymentMethod)
            .IsInEnum();

        RuleFor(x => x.Status)
            .IsInEnum();
    }
}