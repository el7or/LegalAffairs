using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Others
{
    class LegalMemoValidator : AbstractValidator<LegalMemoDto>
    {
        public LegalMemoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.Name} {ValidatorsMessages.Required}")
                .MaximumLength(150)
                .WithMessage($"{ValidatorsMessages.Name} {ValidatorsMessages.MaxLength}");

            RuleFor(x => x.Status)
                .NotNull()
                .WithMessage($"{ValidatorsMessages.Status} {ValidatorsMessages.Required}");

            RuleFor(x => x.Text)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.Text} {ValidatorsMessages.Required}");

            RuleFor(x => x.InitialCaseId)
                .GreaterThan(0)
                .WithMessage($"{ValidatorsMessages.InitialCaseId} {ValidatorsMessages.Required}");


        }
    }
}
