using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum ConfidentialDegrees
    {
        /// <summary>
        /// عادية
        /// </summary>
        [LocalizedDescription("Normal", typeof(EnumsLocalization))]
        Normal = 1,

        /// <summary>
        /// سرية
        /// </summary>
        [LocalizedDescription("Confidential", typeof(EnumsLocalization))]
        Confidential = 2,
    }
}
