using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Case
{
    class CaseChangeStatusValidator : AbstractValidator<CaseChangeStatusDto>
    {
        public CaseChangeStatusValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .When(x => x.Id != 0);

            RuleFor(x => x.Status)
                .IsInEnum();

            RuleFor(x => x.Note)
                .MaximumLength(400)
                .When(x => x.Note != null)
                .WithMessage($"{ValidatorsMessages.Note} {ValidatorsMessages.MaxLength}");
        }
    }
}
