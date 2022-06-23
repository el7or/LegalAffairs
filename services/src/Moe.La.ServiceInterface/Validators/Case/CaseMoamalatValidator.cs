using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Case
{
    public class CaseMoamalatValidator : AbstractValidator<CaseMoamalatDto>
    {
        public CaseMoamalatValidator()
        {
            RuleFor(x => x.CaseId)
                .NotNull()
                .WithMessage($"{ValidatorsMessages.CaseId} {ValidatorsMessages.Required}");

            RuleFor(x => x.MoamalaId)
                 .NotNull()
                 .WithMessage($"{ValidatorsMessages.MoamalaId} {ValidatorsMessages.Required}");
        }
    }
}
