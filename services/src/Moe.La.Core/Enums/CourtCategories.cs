using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    /// <summary>
    /// Used to determine the court category.
    /// </summary>
    public enum CourtCategories
    {
        /// <summary>
        /// وزارة العدل
        /// </summary>
        [LocalizedDescription("MinistryOfJustice", typeof(EnumsLocalization))]
        MinistryOfJustice = 1,

        /// <summary>
        /// ديوان المظالم
        /// </summary>
        [LocalizedDescription("HouseOfGrievances", typeof(EnumsLocalization))]
        HouseOfGrievances = 2,


        /// <summary>
        /// لجان شبه قضائية
        /// </summary>
        [LocalizedDescription("QuasiJudicialCommittees", typeof(EnumsLocalization))]
        QuasiJudicialCommittees = 3
    }
}
