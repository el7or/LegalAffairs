using System;

namespace Moe.La.Core.Entities
{
    public class CaseResearcher : BaseEntity<int>
    {
        public int CaseId { get; set; }

        public Case Case { get; set; }

        public Guid ResearcherId { get; set; }

        public AppUser Researcher { get; set; }

        public string Note { get; set; }

        /// <summary>
        /// The user's id who created it.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Navigation property to the user who created it.
        /// </summary>
        public AppUser CreatedByUser { get; set; }
    }
}
