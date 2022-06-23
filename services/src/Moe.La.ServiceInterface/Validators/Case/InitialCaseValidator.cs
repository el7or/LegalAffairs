using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;
using System;

namespace Moe.La.ServiceInterface.Validators.Case
{
    class InitialCaseValidator : AbstractValidator<InitialCaseDto>
    {
        public InitialCaseValidator()
        {

            RuleFor(x => x.CaseNumberInSource)
                 .MaximumLength(30).WithMessage($"{ValidatorsMessages.CaseSourceNumber} {ValidatorsMessages.MaxLength}")
                 .When(c => !String.IsNullOrWhiteSpace(c.CaseNumberInSource))
                 .NotEmpty()
                .WithMessage($"{ValidatorsMessages.CaseSourceNumber} {ValidatorsMessages.Required}");

            RuleFor(x => x.CaseSource)
                .IsInEnum()
                .WithMessage($"{ValidatorsMessages.CaseSource} {ValidatorsMessages.Required}");

        }
    }
}