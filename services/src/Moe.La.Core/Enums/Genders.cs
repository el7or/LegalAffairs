using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum Genders
    {
        [LocalizedDescription("Male", typeof(EnumsLocalization))]
        Male = 1,

        [LocalizedDescription("Female", typeof(EnumsLocalization))]
        Female = 2
    }
}
