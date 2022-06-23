using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Others
{
    class ResearcherConsultantValidator : AbstractValidator<ResearcherConsultantDto>
    {
        public ResearcherConsultantValidator()
        {
            RuleFor(x => x.ResearcherId)
                       .NotNull()
                       .WithMessage($"{ValidatorsMessages.ResearcherId} {ValidatorsMessages.Required}");
        }
    }
}
