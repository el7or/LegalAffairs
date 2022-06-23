using Moe.La.Core.Enums;
using Moe.La.Core.Enums.Integration;
using System;

namespace Moe.La.Core.Entities
{
    public class MoamalaRasel : BaseEntity<int>
    {
        /// <summary>
        /// رقم المعاملة
        /// </summary>
        public int ItemNumber { get; set; }

        /// <summary>
        /// الرقم الموحد
        /// </summary>
        public string UnifiedNumber { get; set; }

        /// <summary>
        /// الرقم القيد
        /// </summary>
        public string CustomNumber { get; set; }

        /// <summary>
        /// رقم المعاملة السابقة المرتبطة
        /// </summary>
        public int? PreviousItemNumber { get; set; }

        /// <summary>
        /// عنوان المعاملة
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// ملاحظات المعاملة
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        ///  تاريخ انشاء المعاملة بالهجرى
        /// </summary>
        public int HijriCreatedDate { get; set; }

        /// <summary>
        /// تاريخ انشاء المعاملة بالميلادى
        /// </summary>
        public DateTime GregorianCreatedDate { get; set; }

        /// <summary>
        ///  رمز المركز المرسل اليها
        /// </summary>
        public int? CenterIdTo { get; set; }

        /// <summary>
        /// اسم المركز المرسل اليها
        /// </summary>
        public string CenterArabicNameTo { get; set; }

        /// <summary>
        /// رمز المركز المرسل
        /// </summary>
        public int CenterIdFrom { get; set; }

        /// <summary>
        /// اسم المركز المرسل
        /// </summary>
        public string CenterArabicNameFrom { get; set; }

        /// <summary>
        /// رمز الادارة المرسلة اليها
        /// </summary>
        public int GroupIdTo { get; set; }

        /// <summary>
        /// اسم الادارة المرسلة اليها
        /// </summary>
        public string GroupNameTo { get; set; }

        /// <summary>
        /// رمز الادارة المرسلة
        /// </summary>
        public int? GroupIdFrom { get; set; }

        /// <summary>
        /// اسم الادارة المرسلة
        /// </summary>
        public string GroupNameFrom { get; set; }

        /// <summary>
        /// رمز نوع المعاملة
        /// </summary>
        public int? ItemType { get; set; }

        /// <summary>
        /// اسم نوع المعاملة
        /// </summary>
        public string ItemTypeName { get; set; }

        /// <summary>
        /// رمز سرية بالمعاملة
        /// </summary>
        public ConfidentialDegrees ItemPrivacy { get; set; }

        /// <summary>
        /// اسم سرية المعاملة
        /// </summary>
        public string ItemPrivacyName { get; set; }

        /// <summary>
        /// حالة المعاملة
        /// </summary>
        public MoamalaRaselStatuses Status { get; set; }

        /// <summary>
        /// The user's id who updated it.
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// The update datetime.
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
    }
}
