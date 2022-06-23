using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Others
{
    class BoardMeetingValidator : AbstractValidator<BoardMeetingDto>
    {
        public BoardMeetingValidator()
        {

            RuleFor(x => x.BoardId)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage($"{ValidatorsMessages.BoardId} {ValidatorsMessages.Required}");

            RuleFor(x => x.MemoId)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage($"{ValidatorsMessages.MemoId} {ValidatorsMessages.Required}");

            RuleFor(x => x.MeetingPlace)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.MeetingPlace} {ValidatorsMessages.Required}");

            RuleFor(x => x.MeetingDate)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.MeetingDate} {ValidatorsMessages.Required}");

            RuleFor(x => x.BoardMeetingMembers)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.BoardMeetingMembers} {ValidatorsMessages.Required}");
        }
    }
}
