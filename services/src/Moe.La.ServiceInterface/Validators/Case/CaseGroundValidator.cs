using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Case
{
    public class CaseGroundValidator : AbstractValidator<CaseGroundsDto>
    {
        public CaseGroundValidator()
        {
            RuleFor(x => x.Text)
                .NotNull()
                .WithMessage($"{ValidatorsMessages.Text} {ValidatorsMessages.Required}");

            RuleFor(x => x.Text)
                .MaximumLength(400)
                .When(x => x.Text != null)
                .WithMessage($"{ValidatorsMessages.Text} {ValidatorsMessages.MaxLength}");
        }
    }
}
