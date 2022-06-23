using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Case
{
    class ObjectionCaseValidator : AbstractValidator<ObjectionCaseDto>
    {
        public ObjectionCaseValidator()
        {

            RuleFor(x => x.CaseSourceNumber)
                 .MaximumLength(30)
                .WithMessage($"{ValidatorsMessages.CaseSourceNumber} {ValidatorsMessages.MaxLength}");

            RuleFor(x => x.CaseSource)
                .IsInEnum()
                .WithMessage($"{ValidatorsMessages.CaseSource} {ValidatorsMessages.Required}");

            RuleFor(x => x.CourtId)
                .GreaterThan(0)
                .WithMessage($"{ValidatorsMessages.CourtId} {ValidatorsMessages.Required}");

            RuleFor(x => x.CircleNumber)
                .MaximumLength(50)
                .When(x => x.CircleNumber != null)
                .WithMessage($"{ValidatorsMessages.CircleNumber} {ValidatorsMessages.MaxLength}");
        }
    }
}