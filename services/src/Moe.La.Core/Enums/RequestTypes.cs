using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum RequestTypes
    {
        [LocalizedDescription("RequestResearcherChange", typeof(EnumsLocalization))]
        RequestResearcherChange = 1,

        [LocalizedDescription("RequestSupportingDocuments", typeof(EnumsLocalization))]
        RequestSupportingDocuments = 2,

        [LocalizedDescription("RequestAttachedLetter", typeof(EnumsLocalization))]
        RequestAttachedLetter = 3,

        [LocalizedDescription("RequestAddHearingMemo", typeof(EnumsLocalization))]
        RequestAddHearingMemo = 4,

        [LocalizedDescription("RequestExportCaseJudgment", typeof(EnumsLocalization))]
        RequestExportCaseJudgment = 5,

        [LocalizedDescription("RequestObjection", typeof(EnumsLocalization))]
        RequestObjection = 6,

        [LocalizedDescription("ConsultationSupportingDocument", typeof(EnumsLocalization))]
        ConsultationSupportingDocument = 7,

        [LocalizedDescription("ObjectionPermitRequest", typeof(EnumsLocalization))]
        ObjectionPermitRequest = 8,

        [LocalizedDescription("ObjectionLegalMemoRequest", typeof(EnumsLocalization))]
        ObjectionLegalMemoRequest = 9,
        [LocalizedDescription("RequestResearcherChangeToHearing", typeof(EnumsLocalization))]
        RequestResearcherChangeToHearing = 10
    }
}
