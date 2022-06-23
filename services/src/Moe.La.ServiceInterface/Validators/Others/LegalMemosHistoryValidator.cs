using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Others
{
    class LegalMemosHistoryValidator : AbstractValidator<LegalMemosHistoryDto>
    {
        public LegalMemosHistoryValidator()
        {
            RuleFor(x => x.LegalMemoId)
               .GreaterThan(0)
               .WithMessage($"{ValidatorsMessages.LegalMemoId} {ValidatorsMessages.Required}");

            RuleFor(x => x.Name)
               .MaximumLength(50)
               .When(x => x.Name != null)
               .WithMessage($"{ValidatorsMessages.Name} {ValidatorsMessages.MaxLength}")
               .NotNull().WithMessage($"{ValidatorsMessages.Name} {ValidatorsMessages.Required}");

            RuleFor(x => x.LegalMemoId)
               .IsInEnum()
               .WithMessage($"{ValidatorsMessages.Status} {ValidatorsMessages.Required}");
        }
    }
}
