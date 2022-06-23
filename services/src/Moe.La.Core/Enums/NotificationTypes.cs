using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum NotificationTypes
    {
        [LocalizedDescription("info", typeof(EnumsLocalization))]
        Info = 1,

        [LocalizedDescription("warning", typeof(EnumsLocalization))]
        Warning = 2,

        [LocalizedDescription("danger", typeof(EnumsLocalization))]
        Danger = 3,

        [LocalizedDescription("success", typeof(EnumsLocalization))]
        Success = 4,
    }
}
