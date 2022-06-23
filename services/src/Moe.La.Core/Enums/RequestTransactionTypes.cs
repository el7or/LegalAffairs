using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum RequestTransactionTypes
    {
        [LocalizedDescription("CreateRequest", typeof(EnumsLocalization))]
        Create = 1,

        [LocalizedDescription("Related", typeof(EnumsLocalization))]
        Related = 2,

        [LocalizedDescription("Modified", typeof(EnumsLocalization))]
        Modified = 3,

        [LocalizedDescription("RequestAccepted", typeof(EnumsLocalization))]
        RequestAccepted = 4,

        [LocalizedDescription("RequestRejected", typeof(EnumsLocalization))]
        RequestRejected = 5,

        [LocalizedDescription("AddLegalMemoToHearing", typeof(EnumsLocalization))]
        AddLegalMemoToHearing = 6,

        [LocalizedDescription("AttachedLetter", typeof(EnumsLocalization))]
        AttachedLetter = 7,

        [LocalizedDescription("Returned", typeof(EnumsLocalization))]
        Returned = 8,

        [LocalizedDescription("Exported", typeof(EnumsLocalization))]
        Exported = 9,

        [LocalizedDescription("Request.Approved", typeof(EnumsLocalization))]
        Approved = 10,


        [LocalizedDescription("Request.CaseCreation", typeof(EnumsLocalization))]
        CaseCreation = 11,

        [LocalizedDescription("AddObjectionLegalMemoToCase", typeof(EnumsLocalization))]
        AddObjectionLegalMemoToCase = 12,

        [LocalizedDescription("Request.Abandoned", typeof(EnumsLocalization))]
        Abandoned = 13,
        [LocalizedDescription("Request.Canceled", typeof(EnumsLocalization))]
        Canceled = 14,
    }
}
