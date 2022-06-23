using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum PartyEntityTypes
    {
        /// <summary>
        /// فرد
        /// </summary>
        [LocalizedDescription("PartyEntityTypes.Person", typeof(EnumsLocalization))]
        Person = 1,

        /// <summary>
        /// جهة حكومية
        /// </summary>
        [LocalizedDescription("PartyEntityTypes.Government", typeof(EnumsLocalization))]
        Government = 2,

        /// <summary>
        /// شركة
        /// </summary>
        [LocalizedDescription("PartyEntityTypes.Organization", typeof(EnumsLocalization))]
        Organization = 3
    }
}
