using FluentValidation;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators
{
    public class BaseValidator : AbstractValidator<BaseDto<int>>
    {
        public BaseValidator()
        {

        }
    }
}
