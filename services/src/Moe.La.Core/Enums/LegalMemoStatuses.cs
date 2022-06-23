using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum LegalMemoStatuses
    {
        [LocalizedDescription("NewMemo", typeof(EnumsLocalization))]
        New = 1,

        [LocalizedDescription("Unactivated", typeof(EnumsLocalization))]
        Unactivated = 2,

        [LocalizedDescription("Approved", typeof(EnumsLocalization))]
        Approved = 3,

        [LocalizedDescription("Returned", typeof(EnumsLocalization))]
        Returned = 4,

        [LocalizedDescription("Rejected", typeof(EnumsLocalization))]
        Rejected = 5,

        [LocalizedDescription("AcceptedFromConsultant", typeof(EnumsLocalization))]
        Accepted = 6,

        [LocalizedDescription("RaisingConsultant", typeof(EnumsLocalization))]
        RaisingConsultant = 7,

        [LocalizedDescription("RaisingMainBoardHead", typeof(EnumsLocalization))]
        RaisingMainBoardHead = 8,

        [LocalizedDescription("RaisingSubBoardHead", typeof(EnumsLocalization))]
        RaisingSubBoardHead = 9,

        [LocalizedDescription("Modified", typeof(EnumsLocalization))]
        Modified = 10,

        [LocalizedDescription("RaisingAllBoardsHead", typeof(EnumsLocalization))]
        RaisingAllBoardsHead = 11




    }

}
