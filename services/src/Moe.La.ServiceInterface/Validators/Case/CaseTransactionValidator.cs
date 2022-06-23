using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators
{
    class CaseTransactionValidator : AbstractValidator<CaseTransactionDto>
    {
        public CaseTransactionValidator()
        {
            RuleFor(x => x.Note)
                .MaximumLength(200)
                .When(x => x.Note != null)
                .WithMessage($"{ValidatorsMessages.Note} {ValidatorsMessages.MaxLength}");

            RuleFor(x => x.TransactionType)
                .IsInEnum()
                .WithMessage($"{ValidatorsMessages.TransactionType} {ValidatorsMessages.Required}");

            RuleFor(x => x.CaseId)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.CaseId} {ValidatorsMessages.Required}");
        }
    }
}
