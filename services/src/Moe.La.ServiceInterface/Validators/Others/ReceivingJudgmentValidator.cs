using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Others
{
    class ReceivingJudgmentValidator : AbstractValidator<ReceivingJudgmentDto>
    {
        public ReceivingJudgmentValidator()
        {
            RuleFor(x => x.PronouncingJudgmentDate.Value.Date)
            .NotEmpty()
            .WithMessage($"{ValidatorsMessages.PronouncingJudgmentDate} {ValidatorsMessages.Required}")
            .GreaterThanOrEqualTo(x => x.HearingDate.Date)
            .WithMessage($"{ValidatorsMessages.PronouncingJudgmentDate} يجب ان يكون أكبر من او يساوى {ValidatorsMessages.HearingDate}");


            RuleFor(x => x.ReceivingJudgmentDate.Value.Date)
            .NotEmpty()
            .WithMessage($"{ValidatorsMessages.ReceivingJudgmentDate} {ValidatorsMessages.Required}")
            .GreaterThanOrEqualTo(x => x.HearingDate.Date)
            .WithMessage($"{ValidatorsMessages.ReceivingJudgmentDate} يجب ان يكون أكبر من او يساوى {ValidatorsMessages.HearingDate}");

        }
    }
}
