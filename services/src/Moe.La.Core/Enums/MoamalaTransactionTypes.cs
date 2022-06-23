using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum MoamalaTransactionTypes
    {
        /// <summary>
        /// اسناد المعاملة
        /// </summary>
        [LocalizedDescription("AssignTransaction", typeof(EnumsLocalization))]
        AssignTransaction = 1,

        /// <summary>
        /// اعادة المعاملة
        /// </summary>
        [LocalizedDescription("ReturnTransaction", typeof(EnumsLocalization))]
        ReturnTransaction = 2,

        /// <summary>
        /// احالة المعاملة
        /// </summary>
        [LocalizedDescription("ReferredTransaction", typeof(EnumsLocalization))]
        ReferredTransaction = 3,
    }
}
