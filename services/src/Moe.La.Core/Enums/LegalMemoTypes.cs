using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum LegalMemoTypes
    {
        [LocalizedDescription("Pleading", typeof(EnumsLocalization))]
        Pleading = 1,

        [LocalizedDescription("Objection", typeof(EnumsLocalization))]
        Objection = 2,

    }

}
