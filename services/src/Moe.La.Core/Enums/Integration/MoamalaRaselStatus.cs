using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums.Integration
{
    public enum MoamalaRaselStatuses
    {
        /// <summary>
        /// واردة
        /// </summary>
        [LocalizedDescription("Imported", typeof(EnumsLocalization))]
        Imported = 1,

        /// <summary>
        /// مستلمة
        /// </summary>
        [LocalizedDescription("Received", typeof(EnumsLocalization))]
        Received = 2,
    }
}
