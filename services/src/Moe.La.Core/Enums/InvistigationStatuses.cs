using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum InvestigationStatuses
    {
        [LocalizedDescription("New", typeof(EnumsLocalization))]
        New = 1,

        [LocalizedDescription("Closed", typeof(EnumsLocalization))]
        Closed = 2,
    }
}
