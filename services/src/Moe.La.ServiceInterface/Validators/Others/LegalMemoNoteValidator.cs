using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Others
{
    class LegalMemoNoteValidator : AbstractValidator<LegalMemoNoteDto>
    {
        public LegalMemoNoteValidator()
        {
            RuleFor(x => x.LegalMemoId)
                .GreaterThan(0)
                .WithMessage($"{ValidatorsMessages.LegalMemoId} {ValidatorsMessages.Required}");

            RuleFor(x => x.ReviewNumber)
                .GreaterThan(0)
                .WithMessage($"{ValidatorsMessages.ReviewNumber} {ValidatorsMessages.Required}");

            RuleFor(x => x.Text)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.Text} {ValidatorsMessages.Required}")
                .MaximumLength(2000)
                .WithMessage($"{ValidatorsMessages.Text} {ValidatorsMessages.MaxLength}");

            RuleFor(x => x.IsClosed)
                .NotNull()
                .WithMessage($"{ValidatorsMessages.IsClosed} {ValidatorsMessages.Required}");

        }
    }
}
