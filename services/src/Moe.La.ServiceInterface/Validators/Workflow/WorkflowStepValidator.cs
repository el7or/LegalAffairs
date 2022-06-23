using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos.Workflow;

namespace Moe.La.ServiceInterface.Validators.Workflow
{
    class WorkflowStepValidator : AbstractValidator<WorkflowStepDto>
    {
        public WorkflowStepValidator()
        {
            RuleFor(x => x.StepArName)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.StepArName} {ValidatorsMessages.Required}")
                .MaximumLength(50)
                .WithMessage($"{ValidatorsMessages.StepArName} {ValidatorsMessages.MaxLength}");
        }
    }
}
