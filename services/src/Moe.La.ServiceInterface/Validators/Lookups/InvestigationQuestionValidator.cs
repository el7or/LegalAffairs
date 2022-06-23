using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Lookups
{
    class InvestigationQuestionValidator : AbstractValidator<InvestigationQuestionDto>
    {
        public InvestigationQuestionValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .When(x => x.Id != 0);

            RuleFor(x => x.Question)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.Question} {ValidatorsMessages.Required}")
                .MaximumLength(500)
                .WithMessage($"{ValidatorsMessages.Question} {ValidatorsMessages.MaxLength}");
        }
    }
}
