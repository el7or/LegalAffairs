using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos.Workflow;

namespace Moe.La.ServiceInterface.Validators.Workflow
{
    class WorkflowInstanceLogValidator : AbstractValidator<WorkflowInstanceLogDto>
    {
        public WorkflowInstanceLogValidator()
        {
            RuleFor(x => x.WorkflowInstanceNote)
                .MaximumLength(1000)
                .WithMessage($"{ValidatorsMessages.MaxLength}");
        }
    }
}
