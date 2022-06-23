using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Request
{
    class ChangeResearcherToHearingRequestValidator : AbstractValidator<ChangeResearcherToHearingRequestDto>
    {
        public ChangeResearcherToHearingRequestValidator()
        {
            RuleFor(x => x.HearingId)
               .GreaterThan(0)
               .WithMessage($"{ValidatorsMessages.HearingId} {ValidatorsMessages.Required}");

            RuleFor(x => x.Note)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.Note} {ValidatorsMessages.Required}")
                .MaximumLength(400)
                .WithMessage($"{ValidatorsMessages.Note} {ValidatorsMessages.MaxLength}");
        }
    }
}
