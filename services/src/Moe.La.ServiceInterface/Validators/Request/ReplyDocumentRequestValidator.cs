using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Request
{
    class ReplyDocumentRequestValidator : AbstractValidator<ReplyCaseSupportingDocumentRequestDto>
    {
        public ReplyDocumentRequestValidator()
        {
            RuleFor(x => x.ReplyNote)
                .MaximumLength(400)
                .When(x => x.ReplyNote != null)
                .WithMessage($"{ValidatorsMessages.Note} {ValidatorsMessages.MaxLength}");
        }
    }
}
