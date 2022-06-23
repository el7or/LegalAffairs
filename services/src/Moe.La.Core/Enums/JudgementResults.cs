using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum JudgementResults
    {
        [LocalizedDescription("Favor", typeof(EnumsLocalization))]
        Favor = 1,

        [LocalizedDescription("Against", typeof(EnumsLocalization))]
        Against = 2,

        [LocalizedDescription("Peace", typeof(EnumsLocalization))]
        Peace = 3
    }
}
