using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos.Workflow;

namespace Moe.La.ServiceInterface.Validators.Workflow
{
    class WorkflowStepPermissionValidator : AbstractValidator<WorkflowStepPermissionDto>
    {
        public WorkflowStepPermissionValidator()
        {
            RuleFor(x => x.RoleId)
                .GreaterThan(0)
                .WithMessage($"{ValidatorsMessages.RoleId} {ValidatorsMessages.Required}");
        }
    }
}
