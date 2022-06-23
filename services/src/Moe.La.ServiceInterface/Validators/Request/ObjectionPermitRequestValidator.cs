using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Request
{
    class ObjectionPermitRequestValidator : AbstractValidator<ObjectionPermitRequestDto>
    {
        public ObjectionPermitRequestValidator()
        {
            RuleFor(x => x.CaseId)
               .GreaterThan(0)
               .WithMessage($"{ValidatorsMessages.CaseId} {ValidatorsMessages.Required}");

            RuleFor(x => x.Note)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.Note } {ValidatorsMessages.Required}")
                .MaximumLength(4000)
                .WithMessage($"{ValidatorsMessages.Note} {ValidatorsMessages.MaxLength}");

        }
    }
}
