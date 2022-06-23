using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Others
{
    class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.FirstName} {ValidatorsMessages.Required}")
                .MaximumLength(50)
                .WithMessage($"{ValidatorsMessages.FirstName} {ValidatorsMessages.MaxLength}");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.LastName} {ValidatorsMessages.Required}")
                .MaximumLength(50)
                .WithMessage($"{ValidatorsMessages.LastName} {ValidatorsMessages.MaxLength}");

            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.UserName} {ValidatorsMessages.Required}")
                .MaximumLength(50)
                .WithMessage($"{ValidatorsMessages.UserName} {ValidatorsMessages.MaxLength}");

            RuleFor(x => x.IdentityNumber)
                .MaximumLength(10)
                .When(x => x.IdentityNumber != null)
                .WithMessage($"{ValidatorsMessages.IdentityNumber} {ValidatorsMessages.MaxLength}");
            //.Must(x => x.IdentityNumber.StartsWith("1"));
        }
    }
}
