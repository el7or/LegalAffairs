using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Others
{
    public class PartyValidator : AbstractValidator<PartyDto>
    {
        public PartyValidator()
        {
            RuleFor(x => x.IdentityValue)
                .MaximumLength(30)
                .WithMessage($"{ValidatorsMessages.IdentityValue}{ValidatorsMessages.MaxLength }");

            RuleFor(x => x.IdentitySource)
                .MaximumLength(30)
                .When(x => x.IdentitySource != null)
                .WithMessage($"{ValidatorsMessages.IdentitySource }{ ValidatorsMessages.MaxLength}");

            RuleFor(x => x.Mobile)
                .MaximumLength(30)
                .When(x => x.Mobile != null)
                .WithMessage($"{ValidatorsMessages.Mobile }{ValidatorsMessages.MaxLength }");

            RuleFor(x => x.Street)
                .MaximumLength(20)
                .When(x => x.Street != null)
                .WithMessage($"{ ValidatorsMessages.Street}{ ValidatorsMessages.MaxLength}");

            RuleFor(x => x.BuidlingNumber)
                .MaximumLength(10)
                .When(x => x.BuidlingNumber != null)
                .WithMessage($"{ValidatorsMessages.BuidlignNumber }{ValidatorsMessages.MaxLength }");

            RuleFor(x => x.PostalCode)
                .MaximumLength(8)
                .When(x => x.PostalCode != null)
                .WithMessage($"{ ValidatorsMessages.PostalCode}{ ValidatorsMessages.MaxLength}");

            RuleFor(x => x.AddressDetails)
                .MaximumLength(50)
                .When(x => x.AddressDetails != null)
                .WithMessage($"{ValidatorsMessages.AddressDetails }{ValidatorsMessages.MaxLength }");

            RuleFor(x => x.TelephoneNumber)
                .MaximumLength(15)
                .When(x => x.TelephoneNumber != null)
                .WithMessage($"{ValidatorsMessages.TelephoneNumber }{ ValidatorsMessages.MaxLength}");

            //RuleFor(x => x.AttorneyNumber)
            //    .MaximumLength(20)
            //    .When(x => x.TelephoneNumber != null)
            //    .WithMessage($"{ValidatorsMessages.AttorneyNumber }{ValidatorsMessages.MaxLength }");
        }
    }
}
