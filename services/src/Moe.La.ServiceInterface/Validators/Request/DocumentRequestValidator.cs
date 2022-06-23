using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Request
{
    class DocumentRequestValidator : AbstractValidator<CaseSupportingDocumentRequestDto>
    {
        public DocumentRequestValidator()
        {
            RuleFor(x => x.Request)
                .SetValidator(new RequestValidator())
                .When(x => x.Request != null);

            //RuleFor(x => x.ConsigneeDepartmentId)
            //    .NotEmpty()
            //    .WithMessage($"{ValidatorsMessages.ConsigneeDepartment } {ValidatorsMessages.Required}")
            //    .MaximumLength(100)
            //    .WithMessage($"{ValidatorsMessages.ConsigneeDepartment} {ValidatorsMessages.MaxLength}");

            RuleFor(x => x.ReplyNote)
                .MaximumLength(400)
                .When(x => x.ReplyNote != null)
                .WithMessage($"{ValidatorsMessages.Note} {ValidatorsMessages.MaxLength}");

            RuleForEach(x => x.Documents)
                .SetValidator(new DocumentRequestItemValidator())
                .When(x => x.Documents != null);
        }
    }
}
