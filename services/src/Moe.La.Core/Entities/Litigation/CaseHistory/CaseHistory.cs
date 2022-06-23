using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Entities
{
    public class CaseHistory : BaseEntity<int>
    {
        /// <summary>
        /// The related case ID.
        /// </summary>
        public int CaseId { get; set; }

        /// <summary>
        /// مصدر القضية
        /// </summary>
        public CaseSources CaseSource { get; set; }

        /// <summary>
        /// رقم القضية في المصدر
        /// </summary>
        public string CaseNumberInSource { get; set; }

        /// <summary>
        /// سنة القضية في المصدر
        /// </summary>
        public int? CaseYearInSource { get; set; }

        /// <summary>
        /// درجة الترافع
        /// </summary>
        public LitigationTypes? LitigationType { get; set; }

        /// <summary>
        /// رقم القضية المرجعية
        /// </summary>
        public string ReferenceCaseNo { get; set; }

        /// <summary>
        /// الرقم الرئيسي
        /// </summary>
        public string MainNo { get; set; }

        /// <summary>
        /// تاريخ بداية القضية
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// رقم معرف المحكمة
        /// </summary>
        public int? CourtId { get; set; }

        /// <summary>
        /// المحكمة
        /// </summary>
        public Court Court { get; set; }

        /// <summary>
        /// رقم الدائرة
        /// </summary>
        public string CircleNumber { get; set; }

        /// <summary>
        /// عنوان الدعوى
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// موضوع الدعوى
        /// </summary>
        public string CaseDescription { get; set; }

        /// <summary>
        /// صفة الوزارة القانونية
        /// </summary>
        public MinistryLegalStatuses? LegalStatus { get; set; }

        /// <summary>
        /// القضية السابقة المرتبطة
        /// </summary>
        public int? RelatedCaseId { get; set; }

        /// <summary> 
        /// The related case.
        /// </summary>
        /// <remarks>Used to chain the related cases.</remarks>
        public Case RelatedCase { get; set; }

        /// <summary>
        /// The status of the case
        /// </summary>
        public CaseStatuses Status { get; set; }

        /// <summary>
        /// The legal affair department related to this case.
        /// </summary>
        public int? BranchId { get; set; }

        /// <summary>
        /// The legal affair department object.
        /// </summary>
        public Branch Branch { get; set; }

        /// <summary>
        /// After the case is closed, it will be archived.
        /// </summary>
        public bool IsArchived { get; set; } = false;

        /// <summary>
        /// رقم ملف القضية
        /// </summary>
        public string FileNo { get; set; }

        /// <summary>
        /// اسم القاضي
        /// </summary>
        public string JudgeName { get; set; }

        /// <summary>
        /// تاريخ إغلاق القضية
        /// </summary>
        public DateTime? CloseDate { get; set; }

        /// <summary>
        /// تاريخ النطق بالحكم
        /// </summary>
        public DateTime? PronouncingJudgmentDate { get; set; }

        /// <summary>
        /// موعد استلام الحكم
        /// </summary>
        public DateTime? ReceivingJudgmentDate { get; set; }

        /// <summary>
        /// ملاحظات
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// The user's id who created it.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Navigation property to the user who created it.
        /// </summary>
        public AppUser CreatedByUser { get; set; }

        /// <summary>
        /// The user's id who updated it.
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Navigation property to the user who updated it.
        /// </summary>
        public AppUser UpdatedByUser { get; set; }

        /// <summary>
        /// The update datetime.
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// Case rule based on the final hearing of type "نطق بالحكم"
        /// </summary>
        public CaseRule CaseRule { get; set; }

        /// <summary>
        /// تصنيف القضية
        /// </summary>
        public int SecondSubCategoryId { get; set; }

        /// <summary>
        /// Second Sub Category "التصنيف الفرعى 2"
        /// </summary>

        public SecondSubCategory SecondSubCategory { get; set; }

        /// <summary>
        /// سبب إغلاق القضية
        /// </summary>
        public CaseClosinReasons? CaseClosingType { get; set; }

        /// <summary>
        /// يدوى
        /// </summary>
        public bool IsManual { get; set; } = false;

    }
}
