using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    public class Investigation : BaseEntity<int>
    {
        /// <summary>
        /// Investigation number.
        /// </summary>
        /// <remarks>This is an auto-generated value.</remarks>
        public int InvestigationNumber { get; set; }

        /// <summary>
        /// Investigation start date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Investigation subject.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The user's id who's conducting the investigation.
        /// </summary>
        public Guid InvestigatorId { get; set; }

        /// <summary>
        /// The related user entity.
        /// </summary>
        public AppUser Investigator { get; set; }

        /// <summary>
        /// Investigation status.
        /// </summary>
        public InvestigationStatuses InvestigationStatus { get; set; }

        /// <summary>
        /// هل يوجد تحقيق في النيابة؟
        /// </summary>
        public bool IsHasCriminalSide { get; set; }

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

        public ICollection<InvestigationRecord> InvestigationRecords { get; set; } = new List<InvestigationRecord>();


    }
}
