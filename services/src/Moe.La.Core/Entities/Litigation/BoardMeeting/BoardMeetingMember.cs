using System;

namespace Moe.La.Core.Entities.Litigation.BoardMeeting
{
    public class BoardMeetingMember : BaseEntity<int>
    {
        public int BoardMeetingId { get; set; }

        public BoardMeeting BoardMeeting { get; set; }

        public int BoardMemberId { get; set; }

        public LegalBoardMember BoardMember { get; set; }

        public Guid CreatedBy { get; set; }

        public AppUser CreatedByUser { get; set; }

    }
}
