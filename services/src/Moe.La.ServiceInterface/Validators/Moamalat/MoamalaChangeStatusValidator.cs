using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;

namespace Moe.La.ServiceInterface.Validators
{
    class MoamalaChangeStatusValidator : AbstractValidator<MoamalaChangeStatusDto>
    {
        public MoamalaChangeStatusValidator()
        {
            RuleFor(x => x.MoamalaId)
                .GreaterThan(0)
                .When(x => x.MoamalaId != 0);

            RuleFor(x => x.AssignedToId)
                .NotNull()
                .When(x => x.Status == MoamalaStatuses.Assigned);

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0)
                .When(x => x.DepartmentId != null);

            RuleFor(x => x.Note)
                .MaximumLength(400)
                .When(x => x.Note != null)
                .WithMessage($"{ValidatorsMessages.Note} {ValidatorsMessages.MaxLength}");
        }
    }

}
