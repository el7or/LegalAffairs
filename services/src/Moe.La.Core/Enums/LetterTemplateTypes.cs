using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;

namespace Moe.La.Core.Enums
{
    public enum LetterTemplateTypes
    {
        [LocalizedDescription("Template.CaseSupportingDocumentLetter", typeof(EnumsLocalization))]
        CaseSupportingDocumentLetter = 1,

        [LocalizedDescription("Tempalte.AttachedLetter", typeof(EnumsLocalization))]
        AttachedLetter = 2,

        [LocalizedDescription("Tempalte.CaseClosingLetter", typeof(EnumsLocalization))]
        CaseClosingLetter = 3,
    }
}
