using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Case
{
    class CaseValidator : AbstractValidator<BasicCaseDto>
    {
        public CaseValidator()
        {
            RuleFor(x => x.CaseNumberInSource)
                .MaximumLength(30)
                .WithMessage($"{ValidatorsMessages.CaseSourceNumber} {ValidatorsMessages.MaxLength}");

            RuleFor(x => x.CaseSource)
                .IsInEnum()
                .WithMessage($"{ValidatorsMessages.CaseSource} {ValidatorsMessages.Required}");

            //RuleFor(x => x.LitigationType)
            //    .IsInEnum()
            //    .WithMessage($"{ValidatorsMessages.LitigationType} {ValidatorsMessages.Required}");

            //RuleFor(x => x.ReferenceCaseNo)
            //    .MaximumLength(30)
            //    .When(x => x.ReferenceCaseNo != null)
            //    .WithMessage($"{ValidatorsMessages.ReferenceCaseNo} {ValidatorsMessages.MaxLength}");

            //RuleFor(x => x.MainNo)
            //    .MaximumLength(50)
            //    .When(x => x.MainNo != null)
            //    .WithMessage($"{ValidatorsMessages.MainNo} {ValidatorsMessages.MaxLength}");

            RuleFor(x => x.SecondSubCategoryId)
                .GreaterThan(0)
                .WithMessage($"{ValidatorsMessages.SecondSubCategory} {ValidatorsMessages.Required}");

            RuleFor(x => x.CourtId)
                .GreaterThan(0)
                .WithMessage($"{ValidatorsMessages.CourtId} {ValidatorsMessages.Required}");

            RuleFor(x => x.CircleNumber)
                .MaximumLength(50).WithMessage($"{ValidatorsMessages.CircleNumber} {ValidatorsMessages.MaxLength}")
                .When(x => x.CircleNumber != null)
                .NotEmpty().WithMessage($"{ValidatorsMessages.CircleNumber} {ValidatorsMessages.Required}");

            RuleFor(x => x.Subject)
                .MaximumLength(100)
                .When(x => x.Subject != null)
                .WithMessage($"{ValidatorsMessages.Subject} {ValidatorsMessages.MaxLength}")
                .NotEmpty().WithMessage($"{ValidatorsMessages.Subject} {ValidatorsMessages.Required}");

            RuleFor(x => x.LegalStatus)
                .IsInEnum()
                .WithMessage($"{ValidatorsMessages.LegalStatus} {ValidatorsMessages.Required}");

            RuleFor(x => x.CaseDescription)
                .MaximumLength(2000)
                .When(x => x.CaseDescription != null)
                .WithMessage($"{ValidatorsMessages.CaseDescription} {ValidatorsMessages.MaxLength}")
                .NotEmpty().WithMessage($"{ValidatorsMessages.CaseDescription} {ValidatorsMessages.Required}");

            //RuleFor(x => x.OrderDescription)
            //    .MaximumLength(400)
            //    .When(x => x.OrderDescription != null)
            //    .WithMessage($"{ValidatorsMessages.OrderDescription} {ValidatorsMessages.MaxLength}");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage($"{ValidatorsMessages.Status} {ValidatorsMessages.Required}");

            //RuleFor(x => x.FileNo)
            //    .MaximumLength(20)
            //    .When(x => x.FileNo != null)
            //    .WithMessage($"{ValidatorsMessages.FileNo } {ValidatorsMessages.MaxLength }");

            RuleFor(x => x.JudgeName)
                .MaximumLength(100)
                .When(x => x.JudgeName != null)
                .WithMessage($"{ValidatorsMessages.JudgeName} {ValidatorsMessages.MaxLength}");
        }
    }
}
