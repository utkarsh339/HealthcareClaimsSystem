using FluentValidation;

namespace HealthcareClaims.Application.Payments.CreatePayment;

public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(x => x.ClaimId)
            .NotEmpty();

        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.PaymentMethod)
            .IsInEnum();
    }
}