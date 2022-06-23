using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum CaseTransactionTypes
    {

        [LocalizedDescription("SendToResearcher", typeof(EnumsLocalization))]
        SendToResearcher = 1,

        [LocalizedDescription("SendToRegionsSupervisor", typeof(EnumsLocalization))]
        SendToRegionsSupervisor = 2,

        [LocalizedDescription("ReceivedByRegionsSupervisor", typeof(EnumsLocalization))]
        ReceivedByRegionsSupervisor = 3,

        [LocalizedDescription("SendToBranchManager", typeof(EnumsLocalization))]
        SendToBranchManager = 4,

        [LocalizedDescription("ReturnToLitigationManager", typeof(EnumsLocalization))]
        ReturnToLitigationManager = 5,

        [LocalizedDescription("ReceivedByBranchManager", typeof(EnumsLocalization))]
        ReceivedByBranchManager = 6,

        [LocalizedDescription("ReturnToRegionsSupervisor", typeof(EnumsLocalization))]
        ReturnToRegionsSupervisor = 7,

        [LocalizedDescription("RecordReceivingJudgmentDate", typeof(EnumsLocalization))]
        RecordReceivingJudgmentDate = 8,

        [LocalizedDescription("AddHearingSummary", typeof(EnumsLocalization))]
        AddHearingSummary = 9,

        [LocalizedDescription("EditReceivingJudgmentDate", typeof(EnumsLocalization))]
        EditReceivingJudgmentDate = 10,

        [LocalizedDescription("ReceivedByLitigationManager", typeof(EnumsLocalization))]
        ReceivedByLitigationManager = 11,

        [LocalizedDescription("RecordObjectionRequest", typeof(EnumsLocalization))]
        RecordObjectionRequest = 12,

        [LocalizedDescription("ReceiveJudmentInstrument", typeof(EnumsLocalization))]
        ReceiveJudmentInstrument = 13,

        [LocalizedDescription("EditReceiveJudmentInstrument", typeof(EnumsLocalization))]
        EditReceiveJudmentInstrument = 14,

        [LocalizedDescription("DoneJudgment", typeof(EnumsLocalization))]
        DoneJudgment = 15,
        [LocalizedDescription("AcceptChangeResearcherRequest", typeof(EnumsLocalization))]
        AcceptChangeResearcherRequest = 16,

        [LocalizedDescription("RejectChangeResearcherRequest", typeof(EnumsLocalization))]
        RejectChangeResearcherRequest = 17,

        [LocalizedDescription("RemoveReceivingJudgmentDate", typeof(EnumsLocalization))]
        RemoveReceivingJudgmentDate = 18,

        [LocalizedDescription("AddObjectionPermitRequest", typeof(EnumsLocalization))]
        AddObjectionPermitRequest = 19,

        [LocalizedDescription("ReplyObjectionPermitRequest", typeof(EnumsLocalization))]
        ReplyObjectionPermitRequest = 20,

        [LocalizedDescription("ReplyObjectionLegalMemoRequest", typeof(EnumsLocalization))]
        ReplyObjectionLegalMemoRequest = 21,

        [LocalizedDescription("FinalJudgment", typeof(EnumsLocalization))]
        FinalJudgment = 22,
    }
}
