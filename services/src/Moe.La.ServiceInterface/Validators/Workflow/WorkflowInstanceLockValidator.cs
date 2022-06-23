using FluentValidation;
using Moe.La.Core.Dtos.Workflow;

namespace Moe.La.ServiceInterface.Validators.Workflow
{
    class WorkflowInstanceLockValidator : AbstractValidator<WorkflowInstanceLockDto>
    {
        public WorkflowInstanceLockValidator()
        {

        }
    }
}
