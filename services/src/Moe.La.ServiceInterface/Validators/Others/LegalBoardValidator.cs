using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Others
{
    class LegalBoardValidator : AbstractValidator<LegalBoardDto>
    {
        public LegalBoardValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.Name} {ValidatorsMessages.Required}")
                .MaximumLength(50)
                .WithMessage($"{ValidatorsMessages.Name} {ValidatorsMessages.MaxLength}");

            RuleFor(x => x.TypeId)
                .IsInEnum()
                .WithMessage($"{ValidatorsMessages.TypeId} {ValidatorsMessages.Required}");

            RuleForEach(x => x.LegalBoardMembers)
                .SetValidator(new LegalBoradMemberValidator())
                .When(x => x.LegalBoardMembers != null);
        }
    }
}
