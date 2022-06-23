using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum StaffType
    {
        [LocalizedDescription("Administrative", typeof(EnumsLocalization))]
        Administrative = 1,

        [LocalizedDescription("Educational", typeof(EnumsLocalization))]
        Educational = 2,
    }
}
