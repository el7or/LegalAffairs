using FluentValidation;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;

namespace Moe.La.ServiceInterface.Validators.Others
{
    public class AttachmentValidator : AbstractValidator<AttachmentDto>
    {
        public AttachmentValidator()
        {
            //RuleFor(x => x.File)
            //    .NotEmpty()
            //    .WithMessage($"{ValidatorsMessages.File} {ValidatorsMessages.Required}");

            //RuleFor(x => x.AttachmentTypeId)
            //    .NotEmpty()
            //    .WithMessage($"{ValidatorsMessages.AttachmentType} {ValidatorsMessages.Required}");

        }
    }

    public class AttachmentQueryObjectValidator : AbstractValidator<AttachmentQueryObject>
    {
        public AttachmentQueryObjectValidator()
        {
            RuleFor(x => x.GroupName)
                .IsInEnum()
                .WithMessage($"{ValidatorsMessages.GroupName} {ValidatorsMessages.Required}");

            RuleFor(x => x.GroupId)
                .GreaterThan(0)
                .WithMessage($"{ValidatorsMessages.GroupId} {ValidatorsMessages.Required}");
        }
    }
}
