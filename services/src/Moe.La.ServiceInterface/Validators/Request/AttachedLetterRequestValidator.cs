using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Request
{
    class AttachedLetterRequestValidator : AbstractValidator<AttachedLetterRequestDto>
    {
        public AttachedLetterRequestValidator()
        {
            RuleFor(x => x.Request)
                .SetValidator(new RequestValidator())
                .When(x => x.Request != null);

            RuleFor(x => x.Request.Letter.Text)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.Letter } {ValidatorsMessages.Required}");
        }
    }
}
