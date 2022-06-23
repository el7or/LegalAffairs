using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Case
{
    class CaseCategoryValidator : AbstractValidator<CaseCategoryDto>
    {
        public CaseCategoryValidator()
        {
            RuleFor(x => x.CaseSource)
                .IsInEnum()
                .WithMessage($"{ValidatorsMessages.CaseSource} {ValidatorsMessages.Required}");

            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage($"{ValidatorsMessages.Name} {ValidatorsMessages.Required}");

        }
    }

}
