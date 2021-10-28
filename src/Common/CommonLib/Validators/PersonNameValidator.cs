using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Validators;

namespace CommonLib.Validators
{
    /*
     In Here, I am trying to learn and use "Property Validators" in FluentValidation to achieve DRY.
     Following the documentation on : https://docs.fluentvalidation.net/en/latest/custom-validators.html
    */
     
    public class PersonNameValidator<T> : PropertyValidator<T,string>
    {
        private int _minLength;
        private int _maxLength;
        public PersonNameValidator(int minLength, int maxLength)
        {
            _minLength = minLength;
            _maxLength = maxLength;
        }
        public override bool IsValid(ValidationContext<T> context, string value)
        {
            if (string.IsNullOrEmpty(value))
                _messageTemplate = "{PropertyName} is empty.";
            else if (value.Length < _minLength) 
                _messageTemplate = "{PropertyName} must be longer than" + $" {_minLength} chars.";
            else if (value.Length > _maxLength) 
                _messageTemplate = "{PropertyName} must be shorter than" + $" {_maxLength} chars.";
            else
            {
                value = value.Replace(" ", "");
                value = value.Replace("-", "");
                return value.All(Char.IsLetter);
            }
            return false;
        }

        public override string Name => "NameValidator";

        private string _messageTemplate = "{PropertyName} is not a valid name.";

        protected override string GetDefaultMessageTemplate(string errorCode)
            => _messageTemplate;
    }
    public static class PersonNameValidatorExtension{
        public static IRuleBuilderOptions<T,string> PersonName<T>(this IRuleBuilder<T, string> ruleBuilder, int minLength, int maxLength) {
            return ruleBuilder.SetValidator(new PersonNameValidator<T>(minLength, maxLength ));
        }
    }
    

}