using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities.Integration.Moeen
{
    public class InformLetter : BaseEntity<int>
    {
        /// <summary>
        /// رقم الدعوى
        /// </summary>
        public string CaseNo { get; set; }

        /// <summary>
        /// تاريخ قيد الدعوى
        /// </summary>
        public DateTime CaseDate { get; set; }

        /// <summary>
        /// تاريخ قيد الدعوى هجري
        /// </summary>
        public string CaseDateHijri { get; set; }

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
        public IList<Prosecutor> ProsecutorList { get; set; }

        /// <summary>
        /// بيانات المدعى عليهم
        /// </summary>
        public IList<Defendant> DefendantList { get; set; }

        /// <summary>
        /// بيانات الجلسة القادمة
        /// </summary>
        public Session NextSessionInfo { get; set; }

        ///// <summary>
        ///// بيانات الجلسة السابقة
        ///// </summary>
        //public Session OldSessionInfo { get; set; }

        /// <summary>
        /// قائمة طلبات المدعي
        /// </summary>
        public IList<ProsecutorRequest> ProsecutorRequestList { get; set; }

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
        public Request RequestInfo { get; set; }

        /// <summary>
        /// بيانات الحكم في حالة كان سبب إرسال الخطاب تبليغ حكم
        /// </summary>
        public Ruling RulingInfo { get; set; }

        /// <summary>
        /// The user's id who created it.
        /// </summary>
        public Guid CreatedBy { get; set; }
    }
}
