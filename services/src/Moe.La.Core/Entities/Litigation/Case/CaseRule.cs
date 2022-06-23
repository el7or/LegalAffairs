using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    public class CaseRule : BaseEntity<int>
    {
        /// <summary>
        /// رقم الحكم في النظام
        /// </summary>
        public string RuleNumber { get; set; }
        /// <summary>
        /// رقم القضية في النظام
        /// </summary>
        public string CaseNumber { get; set; }

        /// <summary>
        /// مختصر الحكم
        /// ملخص  القضية
        /// </summary>
        public string JudgmentBrief { get; set; }

        /// <summary>
        /// نص الحكم
        /// منطوق الحكم
        /// </summary>
        public string JudgementText { get; set; }

        /// <summary>
        /// حكم نهائي
        /// </summary>
        /// <remarks>تستخدم مع عمليات النظام</remarks>
        public bool? IsFinalJudgment { get; set; }

        /// <summary>
        /// نوع الحكم
        /// </summary>
        public JudgementResults JudgementResult { get; set; }

        /// <summary> 
        /// الجهة المعنية بالحكم
        /// </summary>
        /// 

        public int MinistrySectorId { get; set; }

        public MinistrySector MinistrySector { get; set; }

        public ICollection<CaseRuleMinistryDepartment> CaseRuleMinistryDepartments { get; set; } = new List<CaseRuleMinistryDepartment>();

        //public string TargetMoeDepartment { get; set; }

        /// <summary>
        /// الأسباب التي بني عليها الحكم
        /// </summary>
        public string JudgmentReasons { get; set; }

        /// <summary>
        /// التحليل والرأي
        /// </summary>
        public string Feedback { get; set; }

        /// <summary>
        /// الاستنتاجات النهائية
        /// </summary>
        public string FinalConclusions { get; set; }

        ///// <summary>
        ///// طلبات المدعي
        ///// </summary>
        //public IList<ProsecutorRequest> ProsecutorRequestList { get; set; }

        /// <summary>
        /// رقم المعاملة الصادر في نظام أعمالي (راسل)
        /// </summary>
        public string ImportRefNo { get; set; }

        /// <summary>
        /// تاريخ المعاملة الصادر في نظام أعمالي (راسل)
        /// </summary>
        public DateTime? ImportRefDate { get; set; }
        /// <summary>
        /// رقم المعاملة الوارد في نظام أعمالي (راسل)
        /// </summary>
        public string ExportRefNo { get; set; }

        /// <summary>
        /// تاريخ المعاملة الوارد في نظام أعمالي (راسل)
        /// </summary>
        public DateTime? ExportRefDate { get; set; }

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
        /// The Judgment Receive Date.
        /// </summary>
        //public DateTime JudgmentReceiveDate { get; set; }
        public ICollection<CaseRuleAttachment> Attachments { get; set; } = new List<CaseRuleAttachment>();
    }
}
