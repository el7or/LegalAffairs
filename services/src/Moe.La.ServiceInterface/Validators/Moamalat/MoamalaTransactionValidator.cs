using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators
{
    class MoamalaTransactionValidator : AbstractValidator<MoamalaTransactionDto>
    {
        public MoamalaTransactionValidator()
        {
            RuleFor(x => x.Note)
                .MaximumLength(100)
                .When(x => x.Note != null)
                .WithMessage($"{ValidatorsMessages.Note} {ValidatorsMessages.MaxLength}");

            RuleFor(x => x.TransactionType)
                .IsInEnum()
                .WithMessage($"{ValidatorsMessages.TransactionType} {ValidatorsMessages.Required}");

            RuleFor(x => x.MoamalaId)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.MoamalaId} {ValidatorsMessages.Required}");
        }
    }
}
