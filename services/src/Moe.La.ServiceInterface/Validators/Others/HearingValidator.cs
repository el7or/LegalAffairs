using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Others
{
    class HearingValidator : AbstractValidator<HearingDto>
    {
        public HearingValidator()
        {
            RuleFor(x => x.CaseId)
                .GreaterThan(0)
                .WithMessage($"{ValidatorsMessages.CaseId} {ValidatorsMessages.Required}");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage($"{ValidatorsMessages.Status} {ValidatorsMessages.Required}");

            RuleFor(x => x.CircleNumber)
                .MaximumLength(64)
                .When(x => x.CircleNumber != null)
                .WithMessage($"{ValidatorsMessages.CircleNumber} {ValidatorsMessages.MaxLength}");

            RuleFor(x => x.HearingNumber)
                .GreaterThan(0)
                .WithMessage($"{ValidatorsMessages.HearingNumber} {ValidatorsMessages.Required}");

            RuleFor(x => x.HearingDate)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.HearingDate} {ValidatorsMessages.Required}");

            RuleFor(x => x.HearingTime)
                .MaximumLength(50)
                .When(x => x.HearingTime != null)
                .WithMessage($"{ValidatorsMessages.HearingTime} {ValidatorsMessages.MaxLength}");

            RuleFor(x => x.Summary)
                .MaximumLength(512)
                .When(x => x.Summary != null)
                .WithMessage($"{ValidatorsMessages.Summary} {ValidatorsMessages.MaxLength}");

            RuleFor(x => x.ClosingReport)
                .MaximumLength(512)
                .When(x => x.ClosingReport != null)
                .WithMessage($"{ValidatorsMessages.ClosingReport} {ValidatorsMessages.MaxLength}");

            When(x => x.HearingDate.Date < new System.DateTime().Date, () =>
            {
                RuleFor(x => x.Motions)
               .NotEmpty()
               .WithMessage($"{ValidatorsMessages.Motions} {ValidatorsMessages.Required}");

            });

            //When(x => x.Status == Core.Enums.HearingStatuses.PronouncingJudgment, () =>
            //{
            //    RuleFor(x => x.PronouncingJudgmentDate)
            //    .NotEmpty()
            //    .WithMessage($"{ValidatorsMessages.PronouncingJudgmentDate} {ValidatorsMessages.Required}");

            //    RuleFor(x => x.ReceivingJudgmentDate)
            //    .NotEmpty()
            //    .WithMessage($"{ValidatorsMessages.ReceivingJudgmentDate} {ValidatorsMessages.Required}");
            //});

            When(x => x.Status == Core.Enums.HearingStatuses.Closed, () =>
            {
                RuleFor(x => x.Summary)
                    .NotEmpty()
                    .WithMessage($"{ValidatorsMessages.Summary} {ValidatorsMessages.Required}");

                //RuleFor(x => x.SessionMinutes)
                //    .NotEmpty()
                //    .WithMessage($"{ValidatorsMessages.SessionMinutes} {ValidatorsMessages.Required}");
            });
        }
    }
}
