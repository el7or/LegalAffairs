using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos.Workflow;

namespace Moe.La.ServiceInterface.Validators.Workflow
{
    class WorkflowTypeValidator : AbstractValidator<WorkflowTypeDto>
    {
        public WorkflowTypeValidator()
        {
            RuleFor(x => x.TypeArName)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.TypeArName} {ValidatorsMessages.Required}")
                .MaximumLength(50)
                .WithMessage($"{ValidatorsMessages.TypeArName} {ValidatorsMessages.MaxLength}");

            RuleFor(x => x.LockPeriod)
                .GreaterThan(0)
                .WithMessage($"{ValidatorsMessages.LockPeriod} {ValidatorsMessages.Required}");
        }
    }
}
