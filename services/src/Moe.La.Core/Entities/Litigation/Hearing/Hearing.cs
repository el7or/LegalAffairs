using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    public class Hearing : BaseEntity<int>
    {
        /// <summary>
        /// The related case ID.
        /// </summary>
        public int CaseId { get; set; }

        /// <summary>
        /// The related case object.
        /// </summary>
        public Case Case { get; set; }

        /// <summary>
        /// The user ID who is responsible for attending and closing this hearing.
        /// </summary>
        public Guid? AssignedToId { get; set; }

        /// <summary>
        /// The user object who is responsible for attending and closing this hearing.
        /// </summary>
        public AppUser AssignedTo { get; set; }

        public HearingStatuses Status { get; set; }

        public HearingTypes Type { get; set; }

        public int CourtId { get; set; }

        public Court Court { get; set; }

        public string CircleNumber { get; set; }

        public int? HearingNumber { get; set; }

        public DateTime HearingDate { get; set; }

        public string HearingTime { get; set; }

        public string HearingDesc { get; set; }

        /// <summary>
        /// طلبات الجلسة
        /// </summary>
        public string Motions { get; set; }

        /// <summary>
        /// ملخص الجلسة يتم اضافتها بعد حضور الجلسة
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// هل تم النطق بالحكم ام لا
        /// </summary>
        public bool? IsPronouncedJudgment { get; set; }

        /// <summary>
        /// الحضور
        /// </summary>
        public string Attendees { get; set; }

        /// <summary>
        /// The hearing report.
        /// </summary>
        public string SessionMinutes { get; set; }

        /// <summary>
        /// The hearing closing report.
        /// </summary>
        public string ClosingReport { get; set; }

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

        public ICollection<CaseSupportingDocumentRequest> CaseSupportingDocumentRequests { get; set; } = new List<CaseSupportingDocumentRequest>();

        public ICollection<HearingLegalMemo> HearingLegalMemos { get; set; } = new List<HearingLegalMemo>();

        public ICollection<AddingLegalMemoToHearingRequest> HearingLegalMemoReviewRequests { get; set; } = new List<AddingLegalMemoToHearingRequest>();

        public ICollection<HearingAttachment> Attachments { get; set; } = new List<HearingAttachment>();
    }
}
