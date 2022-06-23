using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum PassTypes
    {
        /// <summary>
        /// وارد
        /// </summary>
        [LocalizedDescription("Import", typeof(EnumsLocalization))]
        Import = 1,

        /// <summary>
        /// صادر
        /// </summary>
        [LocalizedDescription("Export", typeof(EnumsLocalization))]
        Export = 2,
    }
}
