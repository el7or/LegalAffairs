using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum ConsultationStatus
    {

        [LocalizedDescription("Draft", typeof(EnumsLocalization))]
        Draft = 1,

        [LocalizedDescription("Returned", typeof(EnumsLocalization))]
        Returned = 2,

        [LocalizedDescription("Request.Approved", typeof(EnumsLocalization))]
        Approved = 3,

        [LocalizedDescription("Modified", typeof(EnumsLocalization))]
        Modified = 4,

        [LocalizedDescription("Request.Accepted", typeof(EnumsLocalization))]
        Accepted = 5,

        [LocalizedDescription("New", typeof(EnumsLocalization))]
        New = 6,

        [LocalizedDescription("Exported", typeof(EnumsLocalization))]
        Exported = 7
    }
}
