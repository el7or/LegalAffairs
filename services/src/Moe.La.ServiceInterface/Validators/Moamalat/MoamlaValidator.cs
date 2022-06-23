using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators
{
    class MoamlaValidator : AbstractValidator<MoamalaDto>
    {
        public MoamlaValidator()
        {
            RuleFor(x => x.Subject)
                .MaximumLength(100)
                .When(x => x.Subject != null)
                .WithMessage($"{ValidatorsMessages.Subject} {ValidatorsMessages.MaxLength}")
                .NotEmpty().WithMessage($"{ValidatorsMessages.Subject} {ValidatorsMessages.Required}");

            RuleFor(x => x.Description)
                .MaximumLength(2000)
                .When(x => x.Description != null)
                .WithMessage($"{ValidatorsMessages.MoamlaDescription} {ValidatorsMessages.MaxLength}")
                .NotEmpty().WithMessage($"{ValidatorsMessages.MoamlaDescription} {ValidatorsMessages.Required}");

            RuleFor(x => x.MoamalaNumber)
              .MaximumLength(20)
              .When(x => x.MoamalaNumber != null)
              .WithMessage($"{ValidatorsMessages.MoamalaNumber} {ValidatorsMessages.MaxLength}")
              .NotEmpty().WithMessage($"{ValidatorsMessages.MoamalaNumber} {ValidatorsMessages.Required}");

            RuleFor(x => x.UnifiedNo)
              .MaximumLength(20)
              .When(x => x.UnifiedNo != null)
              .WithMessage($"{ValidatorsMessages.UnifiedNo} {ValidatorsMessages.MaxLength}");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage($"{ValidatorsMessages.MoamlaStatus} {ValidatorsMessages.Required}");

            RuleFor(x => x.ConfidentialDegree)
                .IsInEnum()
                .WithMessage($"{ValidatorsMessages.ConfidentialDegree} {ValidatorsMessages.Required}");

            RuleFor(x => x.PassType)
                .IsInEnum()
                .WithMessage($"{ValidatorsMessages.PassType} {ValidatorsMessages.Required}");

            RuleFor(x => x.PassDate)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.PassDate} {ValidatorsMessages.Required}");
        }
    }
}
