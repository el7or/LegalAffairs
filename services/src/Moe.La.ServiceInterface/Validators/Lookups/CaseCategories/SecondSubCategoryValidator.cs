using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;


namespace Moe.La.ServiceInterface.Validators.Lookups
{
    class SecondSubCategoryValidator : AbstractValidator<SecondSubCategoryDto>
    {
        public SecondSubCategoryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .When(x => x.Id != 0);

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.Name} {ValidatorsMessages.Required}")
                .MaximumLength(50)
                .WithMessage($"{ValidatorsMessages.Name} {ValidatorsMessages.MaxLength}");
        }
    }
}
