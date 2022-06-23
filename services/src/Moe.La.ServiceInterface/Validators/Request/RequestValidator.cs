using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Request
{
    class RequestValidator : AbstractValidator<RequestDto>
    {
        public RequestValidator()
        {
            RuleFor(x => x.Note)
                .MaximumLength(400)
                .When(x => x.ReceiverId != null)
                .WithMessage($"{ValidatorsMessages.Note} {ValidatorsMessages.MaxLength}");
        }
    }
}
