using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum RequestStatuses
    {
        [LocalizedDescription("New", typeof(EnumsLocalization))]
        New = 1,

        [LocalizedDescription("Request.Accepted", typeof(EnumsLocalization))]
        Accepted = 2,

        [LocalizedDescription("Rejected", typeof(EnumsLocalization))]
        Rejected = 3,

        [LocalizedDescription("Returned", typeof(EnumsLocalization))]
        Returned = 4,

        [LocalizedDescription("Modified", typeof(EnumsLocalization))]
        Modified = 5,

        [LocalizedDescription("Draft", typeof(EnumsLocalization))]
        Draft = 6,

        [LocalizedDescription("Exported", typeof(EnumsLocalization))]
        Exported = 7,

        [LocalizedDescription("Request.Approved", typeof(EnumsLocalization))]
        Approved = 8,

        [LocalizedDescription("Request.Closed", typeof(EnumsLocalization))]
        Closed = 9,

        [LocalizedDescription("Request.CaseCreation", typeof(EnumsLocalization))]
        CaseCreation = 10,

        [LocalizedDescription("Request.Abandoned", typeof(EnumsLocalization))]
        Abandoned = 11,

        [LocalizedDescription("AcceptedFromConsultant", typeof(EnumsLocalization))]
        AcceptedFromConsultant = 12,

        [LocalizedDescription("Request.AcceptedFromLitigationManager", typeof(EnumsLocalization))]
        AcceptedFromLitigationManager = 13,

        [LocalizedDescription("Request.Canceled", typeof(EnumsLocalization))]
        Canceled = 14
    }
}
