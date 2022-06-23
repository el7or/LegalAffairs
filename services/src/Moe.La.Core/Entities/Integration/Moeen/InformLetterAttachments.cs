using System.Collections.Generic;

namespace Moe.La.Core.Entities.Integration.Moeen
{
    public class InformLetterAttachments
    {
        /// <summary>
        /// قائمة الوثائق المرفقة بالخطاب
        /// </summary>
        public IList<SupportingDocument> SupportingDocumentList { get; set; } = new List<SupportingDocument>();
    }
}
