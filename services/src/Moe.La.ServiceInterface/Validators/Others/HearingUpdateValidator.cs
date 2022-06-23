using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;

namespace Moe.La.ServiceInterface.Validators.Others
{
    public class HearingUpdateValidator : AbstractValidator<HearingUpdateDto>
    {
        public HearingUpdateValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.Text} {ValidatorsMessages.Required}")
                .MaximumLength(200)
                .When(x => x.Text != null)
                .WithMessage($"{ValidatorsMessages.Text} {ValidatorsMessages.MaxLength}");

        }
    }

    public class HearingUpdateQueryObjectValidator : AbstractValidator<HearingUpdateQueryObject>
    {
        public HearingUpdateQueryObjectValidator()
        {
        }
    }
}
