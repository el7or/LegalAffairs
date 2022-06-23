using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Request
{
    class ExportCaseJudgmentRequestValidator : AbstractValidator<ExportCaseJudgmentRequestDto>
    {
        public ExportCaseJudgmentRequestValidator()
        {
            RuleFor(x => x.CaseId)
               .GreaterThan(0)
               .WithMessage($"{ValidatorsMessages.CaseId} {ValidatorsMessages.Required}");

            RuleFor(x => x.Request.Note)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.Note } {ValidatorsMessages.Required}")
                .MaximumLength(4000)
                .WithMessage($"{ValidatorsMessages.Note} {ValidatorsMessages.MaxLength}");

        }
    }
}
