using CommonLib.Validators;
using FluentValidation;
using OrderService.Application.Commands;

namespace OrderService.Application.Validators
{
    public class UpdateOrderCommandValidator: AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(c => c.Quantity)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} is empty.")
                .Quantity(0, 999);
            
            RuleFor(c => c.Price)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} is empty.")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.")
                .LessThan(99999).WithMessage("{PropertyName} must be lower than 99999");
        }
    }
}