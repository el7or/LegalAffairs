using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum LegalBoardTypes
    {
        [LocalizedDescription("Major", typeof(EnumsLocalization))]
        Major = 1,

        [LocalizedDescription("Secondary", typeof(EnumsLocalization))]
        Secondary = 2
    }
}
