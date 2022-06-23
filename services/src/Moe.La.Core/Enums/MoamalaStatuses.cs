using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum MoamalaStatuses
    {
        /// <summary>
        /// جديدة
        /// </summary>
        [LocalizedDescription("New", typeof(EnumsLocalization))]
        New = 1,

        /// <summary>
        /// محالة
        /// </summary>
        [LocalizedDescription("Referred", typeof(EnumsLocalization))]
        Referred = 2,

        /// <summary>
        /// مسندة
        /// </summary>
        [LocalizedDescription("Assigned", typeof(EnumsLocalization))]
        Assigned = 3,

        /// <summary>
        /// معادة
        /// </summary>
        [LocalizedDescription("MoamalaReturned", typeof(EnumsLocalization))]
        MoamalaReturned = 4,
    }
}
