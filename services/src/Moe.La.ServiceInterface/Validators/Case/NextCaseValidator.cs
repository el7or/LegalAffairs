using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Case
{
    class NextCaseValidator : AbstractValidator<NextCaseDto>
    {
        public NextCaseValidator()
        {
            RuleFor(x => x.CaseNumberInSource)
                 .MaximumLength(30)
                .WithMessage($"{ValidatorsMessages.CaseSourceNumber} {ValidatorsMessages.MaxLength}");

            RuleFor(x => x.CourtId)
                .GreaterThan(0)
                .WithMessage($"{ValidatorsMessages.CourtId} {ValidatorsMessages.Required}");

            RuleFor(x => x.CircleNumber)
                .MaximumLength(50).WithMessage($"{ValidatorsMessages.CircleNumber} {ValidatorsMessages.MaxLength}")
                .When(x => x.CircleNumber != null)
                .NotEmpty().WithMessage($"{ValidatorsMessages.CircleNumber} {ValidatorsMessages.Required}");
        }
    }
}
