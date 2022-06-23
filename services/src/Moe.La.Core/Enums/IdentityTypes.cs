using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum IdentityTypes
    {
        [LocalizedDescription("IdentityTypes.National", typeof(EnumsLocalization))]
        National = 1,

        [LocalizedDescription("IdentityTypes.Iqama", typeof(EnumsLocalization))]
        Iqama = 2,

        [LocalizedDescription("IdentityTypes.Gulf", typeof(EnumsLocalization))]
        Gulf = 3,

        [LocalizedDescription("IdentityTypes.Visitor", typeof(EnumsLocalization))]
        Visitor = 4,

        [LocalizedDescription("IdentityTypes.Passport", typeof(EnumsLocalization))]
        Passport = 5,

        [LocalizedDescription("IdentityTypes.Other", typeof(EnumsLocalization))]
        Other = 6,

        [LocalizedDescription("IdentityTypes.Border", typeof(EnumsLocalization))]
        Border = 7

    }
}
