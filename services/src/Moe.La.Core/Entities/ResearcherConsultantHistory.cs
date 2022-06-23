using System;

namespace Moe.La.Core.Entities
{
    public class ResearcherConsultantHistory : BaseEntity<int>
    {
        public int ResearcherConsultantId { get; set; }

        public ResearcherConsultant ResearcherConsultant { get; set; }

        public Guid ConsultantId { get; set; }

        public AppUser Consultant { get; set; }

        public Guid ResearcherId { get; set; }

        public AppUser Researcher { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Boolean IsActive { get; set; }

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
