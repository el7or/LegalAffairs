using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos.Workflow;

namespace Moe.La.ServiceInterface.Validators.Workflow
{
    class WorkflowActionValidator : AbstractValidator<WorkflowActionDto>
    {
        public WorkflowActionValidator()
        {
            RuleFor(x => x.ActionArName)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.ActionArName} {ValidatorsMessages.Required}")
                .MaximumLength(50)
                .WithMessage($"{ValidatorsMessages.ActionArName} {ValidatorsMessages.MaxLength}");
        }
    }
}
