using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Dtos
{
    public class NajizCaseDto
    {

        /// <summary>
        /// رقم المعاملة في ناجز
        /// </summary>
        public string NajizRef { get; set; }

        /// <summary>
        /// تاريخ المعاملة في ناجز
        /// </summary>
        public DateTime? NajizRefDate { get; set; }

        /// <summary>
        ///  رقم القضية في ناجز
        /// </summary>
        public string NajizCaseNo { get; set; }

        /// <summary>
        /// رقم الطلب في ناجز
        /// </summary>
        public string NajizId { get; set; }

        /// <summary>
        /// تاريخ الاستلام
        /// </summary>
        public DateTime? ReceivedDate { get; set; }

        /// <summary>
        /// حالة الاستلام
        /// </summary>
        public ReceivedStatuses? ReceivedStatus { get; set; }


        /// <summary>
        /// درجة الترافع
        /// </summary>
        public string LitigationType { get; set; }

        /// <summary>
        /// الرقم الرئيسي
        /// </summary>
        public string MainNo { get; set; }

        /// <summary>
        /// تاريخ بداية القضية
        /// </summary>
        public DateTime StartDate { get; set; }


        public string Court { get; set; }

        /// <summary>
        /// رقم الدائرة
        /// </summary>
        public string CircleNumber { get; set; }

        /// <summary>
        /// عنوان الدعوى
        /// </summary>
        public string Subject { get; set; }


        /// <summary>
        /// صفة الوزارة القانونية
        /// </summary>
        public string LegalStatus { get; set; }


        /// <summary>
        /// موضوع الدعوى
        /// </summary>
        public string CaseDescription { get; set; }

        /// <summary>
        /// ملاحظات 
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// مكررة أم لا
        /// </summary>
        public bool? IsDuplicated { get; set; }

        /// <summary>
        /// مرفقات اسانيد الدعوى
        /// </summary>
        //public ICollection<string> Attachments { get; set; } = new List<string>();

        /// <summary>
        /// وصف الطلب
        /// </summary>
        public string OrderDescription { get; set; }

        /// <summary>
        /// القضية الاساسية
        /// </summary>
        public int? RelatedCaseId { get; set; }


        public DateTime? RecordDate { get; set; }

        public string FileNo { get; set; }

        public string JudgeName { get; set; }

        /// <summary>
        /// The user's id who created it.
        /// </summary>
        public Guid CreatedBy { get; set; }

        public ICollection<NajizHearingDto> Hearings { get; set; } = new List<NajizHearingDto>();

        public ICollection<NajizPartyDto> Parties { get; set; } = new List<NajizPartyDto>();
    }
}
