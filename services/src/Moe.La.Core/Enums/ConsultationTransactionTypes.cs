using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum ConsultationTransactionTypes
    {
        [LocalizedDescription("ConsultationTransactionTypes.Created", typeof(EnumsLocalization))]
        Created = 1,

        [LocalizedDescription("ConsultationTransactionTypes.Drafted", typeof(EnumsLocalization))]
        Drafted = 2,

        [LocalizedDescription("ConsultationTransactionTypes.Modified", typeof(EnumsLocalization))]
        Modified = 3,

        [LocalizedDescription("ConsultationTransactionTypes.Accepted", typeof(EnumsLocalization))]
        Accepted = 4,

        [LocalizedDescription("ConsultationTransactionTypes.Approved", typeof(EnumsLocalization))]
        Approved = 5,

        [LocalizedDescription("ConsultationTransactionTypes.Returned", typeof(EnumsLocalization))]
        Returned = 6,

        [LocalizedDescription("ConsultationTransactionTypes.Exported", typeof(EnumsLocalization))]
        Exported = 7,
    }

}
