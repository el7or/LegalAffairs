using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    public class Consultation : BaseEntity<int>
    {
        /// <summary>
        /// الموضوع
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// الرقم
        /// </summary>
        public string ConsultationNumber { get; set; }

        /// <summary>
        /// نوع العمل
        /// </summary>
        public int? WorkItemTypeId { get; set; }
        public WorkItemType WorkItemType { get; set; }

        /// <summary>
        /// نوع العمل الفرعي 
        /// </summary>
        public int? SubWorkItemTypeId { get; set; }
        public SubWorkItemType SubWorkItemType { get; set; }

        /// <summary>
        /// تاريخ الاستشارة 
        /// </summary>
        public DateTime? ConsultationDate { get; set; }

        /// <summary>
        /// التحليل القانوني/دراسة التحليلية
        /// </summary>
        public string LegalAnalysis { get; set; }

        /// <summary>
        /// أهم العناصر
        /// </summary>
        public string ImportantElements { get; set; }

        /// <summary>
        /// هل توجد ملاحظات
        /// </summary>
        public bool? IsWithNote { get; set; }

        /// <summary>
        /// حالة النموذج
        /// </summary>
        public ConsultationStatus Status { get; set; }

        /// <summary>
        /// بيان رؤية الادارة
        /// </summary>
        public string DepartmentVision { get; set; }

        /// <summary>
        /// الفرع/الإدارة العامة
        /// </summary>
        public int BranchId { get; set; }
        public Branch Branch { get; set; }

        /// <summary>
        /// الإدارة المتخصصة
        /// </summary>
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }

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
        public AppUser UpdatedUser { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public ReturnedConsultationTypes? ReturnedType { get; set; }

        /// <summary>
        /// المرئيات
        /// </summary>
        public ICollection<ConsultationVisual> ConsultationVisuals { get; set; } = new List<ConsultationVisual>();

        /// <summary>
        /// الحيثيات
        /// </summary>
        public ICollection<ConsultationMerits> ConsultationMerits { get; set; } = new List<ConsultationMerits>();

        /// <summary>
        /// الاسانيد النظامية
        /// </summary>
        public ICollection<ConsultationGrounds> ConsultationGrounds { get; set; } = new List<ConsultationGrounds>();

        /// <summary>
        /// حركات النموذج
        /// </summary>
        public ICollection<ConsultationTransaction> ConsultationTransactions { get; set; } = new List<ConsultationTransaction>();

        /// <summary>
        /// المعاملات المرتبطة
        /// </summary>
        public ICollection<ConsultationMoamalat> ConsultationMoamalat { get; set; } = new List<ConsultationMoamalat>();

        /// <summary>
        /// طلبات النواقص
        /// </summary>
        public ICollection<ConsultationSupportingDocumentRequest> ConsultationSupportingDocuments { get; set; } = new List<ConsultationSupportingDocumentRequest>();
    }
}
