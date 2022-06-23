using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Case
{
    class CaseSendToBranchManagerValidator : AbstractValidator<CaseSendToBranchManagerDto>
    {
        public CaseSendToBranchManagerValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .When(x => x.Id != 0);

            RuleFor(x => x.BranchId)
                .GreaterThan(0)
                .When(x => x.BranchId != 0);

            RuleFor(x => x.Note)
                .MaximumLength(400)
                .When(x => x.Note != null)
                .WithMessage($"{ValidatorsMessages.Note} {ValidatorsMessages.MaxLength}");
        }
    }

}
