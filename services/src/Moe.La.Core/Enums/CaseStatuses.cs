using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum CaseStatuses
    {
        [LocalizedDescription("IncomingCase", typeof(EnumsLocalization))]
        IncomingCase = 0,

        [LocalizedDescription("NewCase", typeof(EnumsLocalization))]
        NewCase = 1,

        [LocalizedDescription("ReceivedByResearcher", typeof(EnumsLocalization))]
        ReceivedByResearcher = 2,

        [LocalizedDescription("DoneJudgment", typeof(EnumsLocalization))]
        DoneJudgment = 3,

        [LocalizedDescription("ReceivedByLitigationManager", typeof(EnumsLocalization))]
        ReceivedByLitigationManager = 6,

        [LocalizedDescription("SentToRegionsSupervisor", typeof(EnumsLocalization))]
        SentToRegionsSupervisor = 7,

        [LocalizedDescription("ReceivedByRegionsSupervisor", typeof(EnumsLocalization))]
        ReceivedByRegionsSupervisor = 8,

        [LocalizedDescription("SentToBranchManager", typeof(EnumsLocalization))]
        SentToBranchManager = 9,

        [LocalizedDescription("ReceivedByBranchManager", typeof(EnumsLocalization))]
        ReceivedByBranchManager = 10,

        [LocalizedDescription("ReturnedToRegionsSupervisor", typeof(EnumsLocalization))]
        ReturnedToRegionsSupervisor = 11,

        [LocalizedDescription("ObjectionRecorded", typeof(EnumsLocalization))]
        ObjectionRecorded = 12,

        [LocalizedDescription("ReturnedToLitigationManager", typeof(EnumsLocalization))]
        ReturnedToLitigationManager = 14,

        [LocalizedDescription("Draft", typeof(EnumsLocalization))]
        Draft = 20,

        [LocalizedDescription("Deleted", typeof(EnumsLocalization))]
        Deleted = 21,

        [LocalizedDescription("ReturnedToDataEntry", typeof(EnumsLocalization))]
        ReturnedToDataEntry = 22,

        [LocalizedDescription("ClosedCase", typeof(EnumsLocalization))]
        ClosedCase = 30
    }
}
