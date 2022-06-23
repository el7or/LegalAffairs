using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators
{
    class RequestTransactionValidator : AbstractValidator<RequestTransactionDto>
    {
        public RequestTransactionValidator()
        {
            RuleFor(x => x.Description)
                .MaximumLength(100)
                .When(x => x.Description != null)
                .WithMessage($"{ValidatorsMessages.Note} {ValidatorsMessages.MaxLength}");

            RuleFor(x => x.TransactionType)
                .IsInEnum()
                .WithMessage($"{ValidatorsMessages.TransactionType} {ValidatorsMessages.Required}");

            RuleFor(x => x.RequestId)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.RequestId} {ValidatorsMessages.Required}");
        }
    }
}
