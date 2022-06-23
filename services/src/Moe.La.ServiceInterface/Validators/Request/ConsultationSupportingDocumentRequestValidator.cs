using FluentValidation;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Request
{
    class ConsultationSupportingDocumentValidator : AbstractValidator<ConsultationSupportingDocumentRequestDto>
    {
        public ConsultationSupportingDocumentValidator()
        {
            RuleFor(x => x.Request)
                .SetValidator(new RequestValidator())
                .When(x => x.Request != null);

        }
    }
}
