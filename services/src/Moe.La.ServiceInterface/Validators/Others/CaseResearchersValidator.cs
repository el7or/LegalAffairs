using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Others
{
    class CaseResearchersValidator : AbstractValidator<CaseResearchersDto>
    {
        public CaseResearchersValidator()
        {
            RuleFor(x => x.CaseId)
                           .NotNull()
                           .WithMessage($"{ValidatorsMessages.CaseId} {ValidatorsMessages.Required}");
            RuleFor(x => x.ResearcherId)
                           .NotNull()
                           .WithMessage($"{ValidatorsMessages.ResourceId} {ValidatorsMessages.Required}");

            RuleFor(x => x.Note)
                          .MaximumLength(100)
                          .When(x => x.Note != null)
                          .WithMessage($"{ValidatorsMessages.Note} {ValidatorsMessages.MaxLength}");

        }
    }
}
