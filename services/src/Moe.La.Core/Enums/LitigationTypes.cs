using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum LitigationTypes
    {
        [LocalizedDescription("FirstInstance", typeof(EnumsLocalization))]
        FirstInstance = 1,

        [LocalizedDescription("Appeal", typeof(EnumsLocalization))]
        Appeal = 2,

        [LocalizedDescription("Supreme", typeof(EnumsLocalization))]
        Supreme = 3
    }
}
