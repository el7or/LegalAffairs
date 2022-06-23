using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos.Workflow;

namespace Moe.La.ServiceInterface.Validators.Workflow
{
    class WorkflowInstanceValidator : AbstractValidator<WorkflowInstanceDto>
    {
        public WorkflowInstanceValidator()
        {
            RuleFor(x => x.WorkflowTypeId)
               .NotEmpty()
               .WithMessage($"{ValidatorsMessages.Required}");

            RuleFor(x => x.CurrentStepId)
               .NotEmpty()
               .WithMessage($"{ValidatorsMessages.Required}");

            RuleFor(x => x.CurrentStatusId)
               .NotEmpty()
               .WithMessage($"{ValidatorsMessages.Required}");

        }
    }
}
