using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos.Workflow;

namespace Moe.La.ServiceInterface.Validators.Workflow
{
    class WorkflowStatusValidator : AbstractValidator<WorkflowStatusDto>
    {
        public WorkflowStatusValidator()
        {
            RuleFor(x => x.StatusArName)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.StatusArName} {ValidatorsMessages.Required}")
                .MaximumLength(50)
                .WithMessage($"{ValidatorsMessages.StatusArName} {ValidatorsMessages.MaxLength}");
        }
    }
}
