using FluentValidation;
using System.Collections.Generic;
using System.Linq;

namespace Moe.La.Core.Models
{
    public class ValidationResult
    {
        public ValidationResult()
        {
            IsValid = false;
        }

        public ValidationResult(bool isValid, List<string> errors)
        {
            IsValid = isValid;
            Errors = errors;
        }

        public List<string> Errors { get; private set; } = new List<string>();

        public bool IsValid { get; private set; }

        public static ValidationResult CheckModelValidation<T>(AbstractValidator<T> vmClassValidator, T vmClass)
        {
            var validatorResponse = vmClassValidator.Validate(vmClass);

            if (!validatorResponse.IsValid)
            {
                var errors = validatorResponse.Errors.Select(m => m.ErrorMessage).ToList();

                return new ValidationResult(validatorResponse.IsValid, errors);
            }

            return new ValidationResult(validatorResponse.IsValid, null);
        }
    }
}
