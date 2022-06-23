using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities.Litigation.BoardMeeting
{
    public class BoardMeeting : BaseEntity<int>
    {
        public int MemoId { get; set; }

        public LegalMemo Memo { get; set; }

        public int BoardId { get; set; }

        public LegalBoard Board { get; set; }

        public string MeetingPlace { get; set; }

        public DateTime MeetingDate { get; set; }

        public Guid CreatedBy { get; set; }

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


        public ICollection<BoardMeetingMember> BoardMeetingMembers { get; set; } = new List<BoardMeetingMember>();
    }
}
