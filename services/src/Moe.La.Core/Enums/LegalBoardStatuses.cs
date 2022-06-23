using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum LegalBoardStatuses
    {
        [LocalizedDescription("Activated", typeof(EnumsLocalization))]
        Activated = 1,

        [LocalizedDescription("Unactivated", typeof(EnumsLocalization))]
        Unactivated = 2
    }
}
