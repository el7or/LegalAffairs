using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    public class Moamala : BaseEntity<int>
    {
        /// <summary>
        /// الرقم الموحد او المرجعى للمعاملة
        /// </summary>
        public string UnifiedNo { get; set; }

        /// <summary>
        /// رقم المعاملة
        /// </summary>
        public string MoamalaNumber { get; set; }

        /// <summary>
        /// الإدارة الداخلية أو الجهة الخارجية المرسلة للمعاملة
        /// </summary>
        public int? SenderDepartmentId { get; set; }

        public MinistryDepartment SenderDepartment { get; set; }

        /// <summary>
        /// درجة السرية
        /// </summary>
        public ConfidentialDegrees? ConfidentialDegree { get; set; }

        /// <summary>
        /// عنوان المعاملة
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// تفاصيل المعاملة
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// نوع المرور واردة ام صادرة
        /// </summary>
        public PassTypes PassType { get; set; }

        /// <summary>
        /// تاريخ ووقت المرور
        /// </summary>
        public DateTime PassDate { get; set; }

        /// <summary>
        /// حالة المعاملة
        /// </summary>
        public MoamalaStatuses Status { get; set; }

        /// <summary>
        /// نوع العمل المرتبط بهذه المعاملة
        /// </summary>
        public int? WorkItemTypeId { get; set; }

        public WorkItemType WorkItemType { get; set; }

        /// <summary>
        /// النوع الفرعي للمعاملة المرتبط بهذه المعاملة
        /// </summary>
        public int? SubWorkItemTypeId { get; set; }

        public SubWorkItemType SubWorkItemType { get; set; }

        /// <summary>
        /// الادارة العامة/المنطقة المحالة اليها
        /// </summary>
        public int? BranchId { get; set; }

        public Branch Branch { get; set; }

        /// <summary>
        /// الادارة الداخلية المحالة اليها
        /// </summary>
        public int? ReceiverDepartmentId { get; set; }

        public Department ReceiverDepartment { get; set; }

        /// <summary>
        /// ملاحظات الإحالة
        /// </summary>
        public string ReferralNote { get; set; }

        /// <summary>
        /// الموظف المسند اليه المعاملة
        /// </summary>
        public Guid? AssignedToId { get; set; }

        public AppUser AssignedTo { get; set; }

        /// <summary>
        /// ملاحظات الإسناد
        /// </summary>
        public string AssigningNote { get; set; }

        /// <summary>
        /// الخطوة الحالية
        /// </summary>
        public MoamalaSteps CurrentStep { get; set; }

        /// <summary>
        /// الخطوة السابقة
        /// </summary>
        public MoamalaSteps? PreviousStep { get; set; }

        /// <summary>
        /// سبب الإعادة
        /// </summary>
        public string ReturningReason { get; set; }

        /// <summary>
        /// العنصر المرتبط بالمعاملة
        /// </summary>
        public int? RelatedId { get; set; }

        /// <summary>
        /// المعاملة يدوية أو من خلال خدمة الربط
        /// </summary>
        public bool IsManual { get; set; }

        /// <summary>
        /// The user's id who created it.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Navigation property to the user who created it.
        /// </summary>
        public AppUser CreatedByUser { get; set; }

        /// <summary>
        /// المرفقات الخاصة بالمعاملة
        /// </summary>
        public ICollection<MoamalaAttachment> Attachments { get; set; } = new List<MoamalaAttachment>();

        /// <summary>
        /// حركات المعاملة
        /// </summary>
        public ICollection<MoamalaTransaction> MoamalaTransactions { get; set; } = new List<MoamalaTransaction>();

        /// <summary>
        /// إشعارات المعاملة
        /// </summary>
        public ICollection<MoamalaNotification> MoamalaNotifications { get; set; } = new List<MoamalaNotification>();

        public ICollection<ConsultationMoamalat> ConsultationMoamalat { get; set; } = new List<ConsultationMoamalat>();

        public ICollection<CaseMoamala> CaseMoamalat { get; set; } = new List<CaseMoamala>();
    }
}
