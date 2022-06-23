using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Others
{
    class RequestTransactionValidator : AbstractValidator<RequestTransactionDto>
    {
        public RequestTransactionValidator()
        {
            RuleFor(x => x.RequestId)
                .GreaterThan(0)
                .WithMessage($"{ValidatorsMessages.RequestId} {ValidatorsMessages.Required}");

            RuleFor(x => x.RequestStatus)
                .IsInEnum()
                .WithMessage($"{ValidatorsMessages.RequestStatus} {ValidatorsMessages.Required}");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.Description} {ValidatorsMessages.Required}");
        }
    }
}
