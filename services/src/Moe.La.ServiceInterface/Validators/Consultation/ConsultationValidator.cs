using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Case
{
    public class ConsultationValidator : AbstractValidator<ConsultationDto>
    {
        public ConsultationValidator()
        {
            RuleFor(x => x.MoamalaId)
                .GreaterThan(0)
                .WithMessage($"{ValidatorsMessages.MoamalaNumber} {ValidatorsMessages.Required}");

            RuleFor(x => x.Subject)
                .MaximumLength(100)
                .When(x => x.Subject != null)
                .WithMessage($"{ValidatorsMessages.ConsultationSubject} {ValidatorsMessages.MaxLength}")
                .NotEmpty().WithMessage($"{ValidatorsMessages.ConsultationSubject} {ValidatorsMessages.Required}");

            RuleFor(x => x.LegalAnalysis)
                .MaximumLength(2000)
                .When(x => x.LegalAnalysis != null)
                .WithMessage($"{ValidatorsMessages.LegalAnalysis} {ValidatorsMessages.MaxLength}")
                .NotEmpty().WithMessage($"{ValidatorsMessages.LegalAnalysis} {ValidatorsMessages.Required}");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage($"{ValidatorsMessages.Status} {ValidatorsMessages.Required}");
        }
    }
}
