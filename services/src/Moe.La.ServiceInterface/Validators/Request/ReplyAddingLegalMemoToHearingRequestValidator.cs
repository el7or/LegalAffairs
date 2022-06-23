using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Request
{
    public class ReplyAddingLegalMemoToHearingRequestValidator : AbstractValidator<ReplyAddingLegalMemoToHearingRequestDto>
    {
        public ReplyAddingLegalMemoToHearingRequestValidator()
        {
            RuleFor(x => x.ReplyNote)
                 .MaximumLength(1500)
                .When(x => x.ReplyNote != null)
                .WithMessage($"{ValidatorsMessages.Note} {ValidatorsMessages.MaxLength}");
        }
    }
}
