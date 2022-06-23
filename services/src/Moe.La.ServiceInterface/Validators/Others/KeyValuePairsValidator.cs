using FluentValidation;
using Moe.La.Core.Dtos;

namespace Moe.La.ServiceInterface.Validators.Others
{
    class KeyValuePairsValidator<T> : AbstractValidator<KeyValuePairsDto<T>>
    {
        public KeyValuePairsValidator()
        {

        }
    }
}
