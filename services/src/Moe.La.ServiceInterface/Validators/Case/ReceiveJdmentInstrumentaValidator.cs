using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Case
{
    public class ReceiveJdmentInstumentaValidator : AbstractValidator<ReceiveJdmentInstrumentDto>
    {
        public ReceiveJdmentInstumentaValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage($"{ValidatorsMessages.CaseId} {ValidatorsMessages.Required}");

            //RuleFor(x => x.CaseRuleMinistryDepartments)
            //   .NotEmpty().WithMessage($"{ValidatorsMessages.CaseRuleGeneralManagementIds} {ValidatorsMessages.Required}");

            RuleFor(x => x.JudgementResult)
               .NotEmpty()
               .WithMessage($"{ValidatorsMessages.JudgementResult} {ValidatorsMessages.Required}");

            RuleFor(x => x.JudgementText)
                .MaximumLength(2000)
                .When(x => x.JudgementText != null)
                .WithMessage($"{ValidatorsMessages.PronouncedJudgmentText} {ValidatorsMessages.MaxLength}")
                .NotEmpty().WithMessage($"{ValidatorsMessages.JudgementText} {ValidatorsMessages.Required}");

            RuleFor(x => x.JudgmentBrief)
                .MaximumLength(500)
                .When(x => x.JudgmentBrief != null)
                .WithMessage($"{ValidatorsMessages.JudgmentInstrumentSummary} {ValidatorsMessages.MaxLength}");

            RuleFor(x => x.JudgmentReasons)
                .MaximumLength(500)
                .When(x => x.JudgmentReasons != null)
                .WithMessage($"{ValidatorsMessages.JudgmentReasons} {ValidatorsMessages.MaxLength}");

            RuleFor(x => x.Feedback)
                .MaximumLength(500)
                .When(x => x.Feedback != null)
                .WithMessage($"{ValidatorsMessages.Feedback} {ValidatorsMessages.MaxLength}");

            RuleFor(x => x.FinalConclusions)
                .MaximumLength(500)
                .When(x => x.FinalConclusions != null)
                .WithMessage($"{ValidatorsMessages.FinalConclusions} {ValidatorsMessages.MaxLength}");

            //RuleFor(x => x.JudgmentReceiveDate.Date)
            //.NotEmpty()
            //.WithMessage($"{ValidatorsMessages.ReceivingJudgmentDate} {ValidatorsMessages.Required}");            
        }
    }
}
