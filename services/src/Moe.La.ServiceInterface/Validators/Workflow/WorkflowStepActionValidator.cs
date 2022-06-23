using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos.Workflow;

namespace Moe.La.ServiceInterface.Validators.Workflow
{
    class WorkflowStepActionValidator : AbstractValidator<WorkflowStepActionDto>
    {
        public WorkflowStepActionValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.Description} {ValidatorsMessages.Required}")
                .MaximumLength(100)
                .WithMessage($"{ValidatorsMessages.Description} {ValidatorsMessages.MaxLength}");
        }
    }
}
