using FluentValidation;
using FluentValidation.Validators;

namespace CommonLib.Validators
{
    
    public class QuantityValidator<T> : PropertyValidator<T,int>
    {
        private int _minCount;
        private int _maxCount;
        public QuantityValidator(int minCount, int maxCount)
        {
            _minCount = minCount;
            _maxCount = maxCount;
        }
        public override bool IsValid(ValidationContext<T> context, int value)
        {
            if (value < _minCount) 
                _messageTemplate = "{PropertyName} must be bigger than" + $" {_minCount}.";
            else if (value > _maxCount) 
                _messageTemplate = "{PropertyName} must be smaller than" + $" {_maxCount}.";
            else
            {
                return true;
            }
            return false;
        }

        public override string Name => "QuantityValidator";

        private string _messageTemplate = "{PropertyName} is not a valid quantity value.";

        protected override string GetDefaultMessageTemplate(string errorCode)
            => _messageTemplate;
    }
    public static class QuantityValidatorExtension{
        public static IRuleBuilderOptions<T,int> Quantity<T>(this IRuleBuilder<T, int> ruleBuilder, int minCount, int maxCount) {
            return ruleBuilder.SetValidator(new QuantityValidator<T>(minCount, maxCount ));
        }
    }
}