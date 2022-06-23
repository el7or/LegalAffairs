using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos.Case;

namespace Moe.La.ServiceInterface.Validators.Others
{
    class CaseCaseCategoryValidator : AbstractValidator<CaseCaseCategoryDto>
    {
        public CaseCaseCategoryValidator()
        {
            RuleFor(x => x.CaseId)
                           .NotNull()
                           .WithMessage($"{ValidatorsMessages.CaseId} {ValidatorsMessages.Required}");

            RuleFor(x => x.CaseCategory)
                           .NotNull()
                           .WithMessage($"{ValidatorsMessages.CaseCategories} {ValidatorsMessages.Required}");
        }
    }
}
