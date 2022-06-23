using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    public class InvestigationRecord : BaseEntity<int>
    {
        /// <summary>
        /// The related investigation ID.
        /// </summary>
        public int InvestigationId { get; set; }

        /// <summary>
        /// The related investigation Entity.
        /// </summary>
        public Investigation Investigation { get; set; }

        /// <summary>
        /// Investigation start date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Investigation end date.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// The investigation record number.
        /// </summary>
        /// <remarks>This is an auto generated field.</remarks>
        public string RecordNumber { get; set; }

        /// <summary>
        /// المرئيات
        /// </summary>
        public string Visuals { get; set; }

        /// <summary>
        /// Investigation record status.
        /// </summary>
        public RecordStatuses RecordStatus { get; set; }

        public bool IsRemote { get; set; }
        public string Subject { get; set; }

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

        public ICollection<InvestigationRecordAttachment> Attachments { get; set; } = new List<InvestigationRecordAttachment>();

        public ICollection<InvestigationRecordParty> InvestigationRecordParties { get; set; } = new List<InvestigationRecordParty>();

        public ICollection<InvestigationRecordQuestion> InvestigationRecordQuestions { get; set; } = new List<InvestigationRecordQuestion>();

        public ICollection<InvestigationRecordInvestigator> InvestigationRecordInvestigators { get; set; } = new List<InvestigationRecordInvestigator>();

        public ICollection<InvestigationRecordAttendant> Attendants { get; set; } = new List<InvestigationRecordAttendant>();

    }
}
