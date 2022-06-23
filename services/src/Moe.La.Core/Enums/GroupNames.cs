using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum GroupNames
    {
        [LocalizedDescription("Case", typeof(EnumsLocalization))]
        Case = 1,

        [LocalizedDescription("Hearing", typeof(EnumsLocalization))]
        Hearing = 2,

        [LocalizedDescription("Memo", typeof(EnumsLocalization))]
        Memo = 3,

        [LocalizedDescription("CaseRule", typeof(EnumsLocalization))]
        CaseRule = 4,

        [LocalizedDescription("HearingUpdate", typeof(EnumsLocalization))]
        HearingUpdate = 5,

        [LocalizedDescription("RepresentativeLetterImage", typeof(EnumsLocalization))]
        RepresentativeLetterImage = 6,

        [LocalizedDescription("InvestigationRecord", typeof(EnumsLocalization))]
        InvestigationRecord = 7,

        [LocalizedDescription("Moamala", typeof(EnumsLocalization))]
        Moamala = 8
    }
}
