using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Case
{
    public class CasePartyValidator : AbstractValidator<CasePartyDto>
    {
        public CasePartyValidator()
        {
            RuleFor(x => x.CaseId)
                           .NotNull()
                           .WithMessage($"{ValidatorsMessages.CaseId} {ValidatorsMessages.Required}");

            RuleFor(x => x.PartyId)
                           .NotNull()
                           .WithMessage($"{ValidatorsMessages.PartyId} {ValidatorsMessages.Required}");
        }
    }
}
