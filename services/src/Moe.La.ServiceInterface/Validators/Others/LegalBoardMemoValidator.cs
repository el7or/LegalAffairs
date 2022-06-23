using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Others
{
    class LegalBoardMemoValidator : AbstractValidator<LegalBoardMemoDto>
    {
        public LegalBoardMemoValidator()
        {
            RuleFor(x => x.LegalMemoId)
              .GreaterThan(0)
              .WithMessage($"{ValidatorsMessages.LegalMemoId} {ValidatorsMessages.Required}");

        }
    }
}
