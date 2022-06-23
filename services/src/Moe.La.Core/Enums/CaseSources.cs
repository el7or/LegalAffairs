using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum CaseSources
    {
        /// <summary>
        /// نظام ناجز من وزارة العدل
        /// </summary>
        [LocalizedDescription("Najiz", typeof(EnumsLocalization))]
        Najiz = 1,

        /// <summary>
        /// نظام معين من ديوان المظالم
        /// </summary>
        [LocalizedDescription("Moeen", typeof(EnumsLocalization))]
        Moeen = 2,

        /// <summary>
        /// اللجان شبة قضائية
        /// </summary>
        [LocalizedDescription("QuasiJudicialCommittee", typeof(EnumsLocalization))]
        QuasiJudicialCommittee = 3
    }
}
