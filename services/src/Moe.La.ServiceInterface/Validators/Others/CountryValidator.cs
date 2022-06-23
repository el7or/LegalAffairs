using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Others
{
    class CountryValidator : AbstractValidator<CountryDto>
    {
        public CountryValidator()
        {
            RuleFor(x => x.NameAr)
                        .MaximumLength(60)
                        .When(x => x.NameAr != null)
                        .WithMessage($"{ValidatorsMessages.NameAr} {ValidatorsMessages.MaxLength}")
                        .NotEmpty().WithMessage($"{ValidatorsMessages.NameAr} {ValidatorsMessages.Required}");

            RuleFor(x => x.NameEn)
                        .MaximumLength(60)
                        .When(x => x.NameEn != null)
                        .WithMessage($"{ValidatorsMessages.NameEn} {ValidatorsMessages.NameAr}")
                        .NotEmpty().WithMessage($"{ValidatorsMessages.NameEn} {ValidatorsMessages.NameAr}");

            RuleFor(x => x.NationalityAr)
                        .MaximumLength(50)
                        .When(x => x.NationalityAr != null)
                        .WithMessage($"{ValidatorsMessages.NationalityAr} {ValidatorsMessages.NameAr}")
                        .NotEmpty().WithMessage($"{ValidatorsMessages.NationalityAr} {ValidatorsMessages.NameAr}");

            RuleFor(x => x.NationalityEn)
                        .MaximumLength(50)
                        .When(x => x.NationalityEn != null)
                        .WithMessage($"{ValidatorsMessages.NationalityEn} {ValidatorsMessages.NameAr}")
                        .NotEmpty().WithMessage($"{ValidatorsMessages.NationalityEn} {ValidatorsMessages.NameAr}");

            RuleFor(x => x.ISO31661CodeAlph3)
                        .MaximumLength(3)
                        .When(x => x.ISO31661CodeAlph3 != null)
                        .WithMessage($"{ValidatorsMessages.ISO31661CodeAlph3} {ValidatorsMessages.NameAr}")
                        .NotEmpty().WithMessage($"{ValidatorsMessages.ISO31661CodeAlph3} {ValidatorsMessages.NameAr}");
        }
    }
}
