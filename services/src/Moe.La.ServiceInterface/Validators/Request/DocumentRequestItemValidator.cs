using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Request
{
    class DocumentRequestItemValidator : AbstractValidator<CaseSupportingDocumentRequestItemDto>
    {
        public DocumentRequestItemValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.Name } {ValidatorsMessages.Required}")
                .MaximumLength(100)
                .WithMessage($"{ValidatorsMessages.Name } {ValidatorsMessages.MaxLength}");
        }
    }
}
