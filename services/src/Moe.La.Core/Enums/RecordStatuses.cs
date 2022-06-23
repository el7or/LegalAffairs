using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum RecordStatuses
    {
        /// <summary>
        /// مسودة
        /// </summary>
        [LocalizedDescription("Draft", typeof(EnumsLocalization))]
        Draft = 1,
        /// <summary>
        /// مغلق
        /// </summary>
        [LocalizedDescription("Closed", typeof(EnumsLocalization))]
        Closed = 2,
    }

}
