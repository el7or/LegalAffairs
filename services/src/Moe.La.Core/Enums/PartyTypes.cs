using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum PartyTypes
    {
        /// <summary>
        /// شخص
        /// </summary>
        [LocalizedDescription("Person", typeof(EnumsLocalization))]
        Person = 1,

        /// <summary>
        /// جهة حكومية
        /// </summary>
        [LocalizedDescription("GovernmentalEntity", typeof(EnumsLocalization))]
        GovernmentalEntity = 2,

        /// <summary>
        /// شركة او مؤسسة
        /// </summary>
        [LocalizedDescription("CompanyOrInstitution", typeof(EnumsLocalization))]
        CompanyOrInstitution = 3

    }
}
