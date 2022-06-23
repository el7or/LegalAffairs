using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum SuggestedOpinon
    {
        [LocalizedDescription("NoFutherAction", typeof(EnumsLocalization))]
        NoFutherAction = 1,

        [LocalizedDescription("Objection", typeof(EnumsLocalization))]
        ObjectionAction = 2,

    }
}
