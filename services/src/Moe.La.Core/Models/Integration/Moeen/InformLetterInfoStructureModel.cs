using System.Collections.Generic;

namespace Moe.La.Core.Models.Integration.Moeen
{
    public class InformLetterInfoStructureModel
    {
        /// <summary>
        /// رقم الدعوى
        /// </summary>
        public string CaseNo { get; set; }

        /// <summary>
        /// تاريخ قيد الدعوى
        /// </summary>
        public GHDateStructureModel CaseDate { get; set; }

        /// <summary>
        /// موضوع الدعوى
        /// </summary>
        public string CaseSubject { get; set; }

        /// <summary>
        /// أسانيد الدعوى
        /// </summary>
        public string CaseReferences { get; set; }

        /// <summary>
        /// معرف المحكمة المقام بها الدعوى حسب معرفات ديوان المظالم
        /// </summary>
        public string CourtID { get; set; }

        /// <summary>
        /// اسم المحكمة المقام بها الدعوى
        /// </summary>
        public string CourtDescription { get; set; }

        /// <summary>
        /// معرف الدائرة ناظرة الدعوى حسب معرفات ديوان المظالم
        /// </summary>
        public string CircuitID { get; set; }

        /// <summary>
        /// اسم الدائرة ناظرة الدعوى
        /// </summary>
        public string CircuitDescription { get; set; }

        /// <summary>
        /// معرف نوع خطاب الإبلاغ حسب معرفات ديوان المظالم
        /// </summary>
        public string InformLetterTypeCode { get; set; }

        /// <summary>
        /// وصف نوع خطاب الإبلاغ
        /// </summary>
        public string InformLetterTypeDescription { get; set; }

        /// <summary>
        /// بيانات المدعين
        /// </summary>
        public IList<ProsecutorInfoStructureModel> ProsecutorList { get; set; }

        /// <summary>
        /// بيانات المدعى عليهم
        /// </summary>
        public IList<DefendantInfoStructureModel> DefendantList { get; set; }

        /// <summary>
        /// بيانات الجلسة القادمة
        /// </summary>
        public NextSessionInfoStructureModel NextSessionInfo { get; set; }

        /// <summary>
        /// بيانات الجلسة السابقة
        /// </summary>
        public NextSessionInfoStructureModel OldSessionInfo { get; set; }

        /// <summary>
        /// قائمة طلبات المدعي
        /// </summary>
        public IList<ProsecutorRequestStructureModel> ProsecutorRequestList { get; set; }

        /// <summary>
        /// هل خطاب الإبلاغ به مرفقات (نعم / لا)
        /// </summary>
        public string InformLetterHasAttachment { get; set; }

        /// <summary>
        /// وصف سبب إرسال الخطاب (جلسة في دعوى -جلسة في طلب -تبليغ حكم-......)
        /// </summary>
        public string InformLetterRelevantDesc { get; set; }

        /// <summary>
        /// معرف سبب إرسال الخطاب حسب معرفات ديوان المظالم
        /// </summary>
        public string InformLetterRelevantCode { get; set; }

        /// <summary>
        /// بيانات الطلب في حالة كان سبب إرسال الخطاب طلب
        /// </summary>
        public RequestInfoStructureModel RequestInfo { get; set; }

        /// <summary>
        /// بيانات الحكم في حالة كان سبب إرسال الخطاب تبليغ حكم
        /// </summary>
        public RulingInfoStructureModel RulingInfo { get; set; }
    }
}
