using CommonLib.Validators;
using CustomerService.Application.Commands;
using FluentValidation;

namespace CustomerService.Application.Validators
{
    public class UpdateCustomerCommandValidator: AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            RuleFor(c => c.Name).PersonName(2, 50);
            RuleFor(c => c.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} is empty.")
                .EmailAddress().WithMessage("{PropertyName} is not a valid email address.");
        }
    }
}