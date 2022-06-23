using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Consultation
{
    class ConsultationReviewValidator : AbstractValidator<ConsultationReviewDto>
    {
        public ConsultationReviewValidator()
        {
            RuleFor(x => x.DepartmentVision)
                .MaximumLength(400)
                .When(x => x.DepartmentVision != null)
                .WithMessage($"{ValidatorsMessages.Note} {ValidatorsMessages.MaxLength}");
        }
    }
}
