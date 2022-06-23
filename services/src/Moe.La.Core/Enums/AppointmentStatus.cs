using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum AppointmentStatus
    {
        /// <summary>
        /// على رأس العمل
        /// </summary>
        [LocalizedDescription("OnTheJob", typeof(EnumsLocalization))]
        OnTheJob = 1,
        /// <summary>
        /// مكفوف اليد
        /// </summary>
        [LocalizedDescription("BlindHand", typeof(EnumsLocalization))]
        BlindHand = 2,
        /// <summary>
        /// مطوي القيد
        /// </summary>
        [LocalizedDescription("RecordPleated", typeof(EnumsLocalization))]
        RecordPleated = 3,
        /// <summary>
        /// متقاعد
        /// </summary>
        [LocalizedDescription("Retired", typeof(EnumsLocalization))]
        Retired = 4,
    }
}
