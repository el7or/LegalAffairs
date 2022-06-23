using System;

namespace Moe.La.Core.Entities
{
    public class ResearcherConsultant : BaseEntity<int>
    {
        /// <summary>
        /// Consultant user id.
        /// </summary>
        public Guid ConsultantId { get; set; }

        /// <summary>
        /// Consultant navigation property.
        /// </summary>
        public AppUser Consultant { get; set; }

        /// <summary>
        /// Researcher user id who is working for the consultant.
        /// </summary>
        public Guid ResearcherId { get; set; }

        /// <summary>
        /// Researcher navigation property.
        /// </summary>
        public AppUser Researcher { get; set; }

        /// <summary>
        /// The work start date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The work end date.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Determine that the relation between consultant and researcher still active.
        /// </summary>
        public bool IsActive { get; set; }

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
    }
}
