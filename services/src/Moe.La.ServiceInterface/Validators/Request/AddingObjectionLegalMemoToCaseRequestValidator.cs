using FluentValidation;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Request
{
    class AddingObjectionLegalMemoToCaseRequestValidator : AbstractValidator<AddingObjectionLegalMemoToCaseRequestDto>
    {
        public AddingObjectionLegalMemoToCaseRequestValidator()
        {
            //RuleFor(x => x.Request)
            //    .SetValidator(new RequestValidator())
            //    .When(x => x.Request != null);
        }
    }
}
