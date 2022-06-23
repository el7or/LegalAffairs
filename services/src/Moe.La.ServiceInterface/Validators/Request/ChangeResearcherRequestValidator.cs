using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Request
{
    class ChangeResearcherRequestValidator : AbstractValidator<ChangeResearcherRequestDto>
    {
        public ChangeResearcherRequestValidator()
        {
            RuleFor(x => x.Note)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.Note} {ValidatorsMessages.Required}")
                .MaximumLength(400)
                .WithMessage($"{ValidatorsMessages.Note} {ValidatorsMessages.MaxLength}");
        }
    }
}
