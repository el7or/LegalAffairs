using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum ReceivedStatuses
    {
        /// <summary>
        /// استلام ناجح
        /// </summary>
        [LocalizedDescription("Successful", typeof(EnumsLocalization))]
        Successful = 1,

        /// <summary>
        /// استلام فاشل
        /// </summary>
        [LocalizedDescription("Failure", typeof(EnumsLocalization))]
        Failure = 2,
    }
}
