using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Others
{
    class HearingCloseCreateValidator : AbstractValidator<HearingCloseCreateDto>
    {
        public HearingCloseCreateValidator()
        {
            RuleFor(x => x.CurrentHearingId)
                   .GreaterThan(0).When(x => x.CurrentHearingId != 0);

            RuleFor(x => x.ClosingReport)
                .MaximumLength(512)
                .When(x => x.ClosingReport != null)
                .WithMessage($"{ValidatorsMessages.ClosingReport} {ValidatorsMessages.MaxLength}");

            RuleFor(x => x.NewHearing)
                .SetValidator(new HearingValidator())
                .When(x => x.NewHearing != null);
        }
    }
}
