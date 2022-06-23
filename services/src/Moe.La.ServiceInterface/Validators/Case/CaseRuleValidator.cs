using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Case
{
    public class CaseRuleValidator : AbstractValidator<CaseRuleDto>
    {
        public CaseRuleValidator()
        {
            RuleFor(x => x.CaseId)
                .GreaterThan(0)
                .WithMessage($"{ValidatorsMessages.CaseId} {ValidatorsMessages.Required}");

            //RuleFor(x => x.LitigationType)
            //    .IsInEnum()
            //    .WithMessage($"{ValidatorsMessages.LitigationType} {ValidatorsMessages.Required}");

            RuleFor(x => x.JudgementResult)
                .IsInEnum()
                .WithMessage($"{ValidatorsMessages.JudgementResult} {ValidatorsMessages.Required}");

            RuleFor(x => x.JudgementText)
                .MaximumLength(2000)
                .When(x => x.JudgementText != null)
                .WithMessage($"{ValidatorsMessages.JudgementText} {ValidatorsMessages.MaxLength}")
                .NotEmpty().WithMessage($"{ValidatorsMessages.JudgementText} {ValidatorsMessages.Required}");

            //RuleFor(x => x.IsFinalJudgment)
            //    .NotEmpty().WithMessage($"{ValidatorsMessages.IsFinalJudgment} {ValidatorsMessages.Required}");

            //RuleFor(x => x.CaseRuleGeneralManagementIds)
            //    .NotEmpty().WithMessage($"{ValidatorsMessages.CaseRuleGeneralManagementIds} {ValidatorsMessages.Required}");

            RuleFor(x => x.JudgmentBrief)
                .MaximumLength(200)
                .When(x => x.JudgmentBrief != null)
                .WithMessage($"{ValidatorsMessages.JudgmentBrief} {ValidatorsMessages.MaxLength}");
        }
    }
}
