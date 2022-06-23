using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    public class LegalBoardMember : BaseEntity<int>
    {
        public Guid UserId { get; set; }

        public AppUser User { get; set; }

        public int LegalBoardId { get; set; }

        public LegalBoard LegalBoard { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public LegalBoardMembershipTypes MembershipType { get; set; }

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

        public ICollection<LegalBoardMemberHistory> BoardMemberHistory { get; set; } = new List<LegalBoardMemberHistory>();
    }
}
