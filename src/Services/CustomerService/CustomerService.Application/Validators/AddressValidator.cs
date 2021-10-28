using Entities.Models;
using FluentValidation;

namespace CustomerService.Application.Validators
{
    public class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(a => a.City)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} is empty.")
                .Length(2, 50).WithMessage("Length of {PropertyName} must be between 2 and 50.");
            
            
            RuleFor(a => a.Country)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} is empty.")
                .Length(2, 50).WithMessage("Length of {PropertyName} must be between 2 and 50.");
            
            RuleFor(a => a.AddressLine)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} is empty.")
                .Length(2, 300).WithMessage("Length of {PropertyName} must be between 2 and 300.");

            RuleFor(a => a.CityCode)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} is empty.")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.")
                .LessThan(99999).WithMessage("{PropertyName} is invalid.");

        }
    }
}