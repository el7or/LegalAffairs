using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum HearingStatuses
    {
        [LocalizedDescription("Scheduled", typeof(EnumsLocalization))]
        Scheduled = 1,

        [LocalizedDescription("Finished", typeof(EnumsLocalization))]
        Finished = 2,

        [LocalizedDescription("Closed", typeof(EnumsLocalization))]
        Closed = 3
    }

    public enum HearingTypes
    {
        [LocalizedDescription("Pleading", typeof(EnumsLocalization))]
        Pleading = 1,

        [LocalizedDescription("PronouncingJudgment", typeof(EnumsLocalization))]
        PronouncingJudgment = 2,

        //[LocalizedDescription("Pleading", typeof(EnumsLocalization))]
        //Pleading = 3
    }
}
