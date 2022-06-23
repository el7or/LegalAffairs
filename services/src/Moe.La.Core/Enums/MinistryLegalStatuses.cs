using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    /// <summary>
    /// صفة الوزارة القضائية
    /// </summary>
    public enum MinistryLegalStatuses
    {
        /// <summary> 
        /// مدعى عليها
        /// </summary>
        [LocalizedDescription("Defendant", typeof(EnumsLocalization))]
        Defendant = 1,

        /// <summary>
        /// مدعية
        /// </summary>
        [LocalizedDescription("Plaintiff", typeof(EnumsLocalization))]
        Plaintiff = 2
    }
}
