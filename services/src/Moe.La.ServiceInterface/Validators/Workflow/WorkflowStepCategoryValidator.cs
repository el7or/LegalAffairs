using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos.Workflow;

namespace Moe.La.ServiceInterface.Validators.Workflow
{
    class WorkflowStepCategoryValidator : AbstractValidator<WorkflowStepCategoryDto>
    {
        public WorkflowStepCategoryValidator()
        {
            RuleFor(x => x.CategoryArName)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.CategoryArName} {ValidatorsMessages.Required}")
                .MaximumLength(50)
                .WithMessage($"{ValidatorsMessages.CategoryArName} {ValidatorsMessages.MaxLength}");
        }
    }
}
