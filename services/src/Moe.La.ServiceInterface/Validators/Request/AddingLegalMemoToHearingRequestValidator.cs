using FluentValidation;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Request
{
    class AddingLegalMemoToHearingRequestValidator : AbstractValidator<AddingLegalMemoToHearingRequestDto>
    {
        public AddingLegalMemoToHearingRequestValidator()
        {
            RuleFor(x => x.Request)
                .SetValidator(new RequestValidator())
                .When(x => x.Request != null);
        }
    }
}
