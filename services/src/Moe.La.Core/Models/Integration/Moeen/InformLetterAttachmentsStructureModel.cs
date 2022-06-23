using System.Collections.Generic;

namespace Moe.La.Core.Models.Integration.Moeen
{
    public class InformLetterAttachmentsStructureModel
    {
        /// <summary>
        /// قائمة الوثائق المرفقة بالخطاب
        /// </summary>
        public IList<object> SupportingDocumentList { get; set; }
    }
}
