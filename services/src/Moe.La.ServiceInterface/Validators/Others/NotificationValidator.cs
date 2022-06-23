using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Others
{
    class NotificationValidator : AbstractValidator<NotificationDto>
    {
        public NotificationValidator()
        {
            //RuleFor(x => x.Type)
            //   .IsInEnum()
            //   .WithMessage($"{ValidatorsMessages.NotificationType} {ValidatorsMessages.Required}");

            RuleFor(x => x.Text)
                .NotEmpty()
                .WithMessage($"{ValidatorsMessages.Text} {ValidatorsMessages.Required}")
                .MaximumLength(500)
                .WithMessage($"{ValidatorsMessages.Text} {ValidatorsMessages.MaxLength}");

            //RuleFor(x => x.URL)
            //    .MaximumLength(100)
            //    .When(x => x.URL != null)
            //    .WithMessage($"{ValidatorsMessages.URL} {ValidatorsMessages.MaxLength}");

            //RuleFor(x => x.Users)
            //    .NotEmpty()
            //    .WithMessage($"{ValidatorsMessages.ToUserId} {ValidatorsMessages.Required}");

        }
    }
}
