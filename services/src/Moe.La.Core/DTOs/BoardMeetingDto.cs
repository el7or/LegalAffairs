using System;
using System.Collections.Generic;

namespace Moe.La.Core.Dtos
{
    public class BoardMeetingListItemDto
    {
        public int Id { get; set; }

        public string Memo { get; set; }

        public string Board { get; set; }

        public string MeetingPlace { get; set; }

        public DateTime MeetingDate { get; set; }

        public string MeetingDateHigri { get; set; }

    }
    public class BoardMeetingDetailsDto
    {
        public int Id { get; set; }

        public string MeetingPlace { get; set; }

        public DateTime MeetingDate { get; set; }

        public string MeetingDateHigri { get; set; }

        public ICollection<int> BoardMeetingMembersIds { get; set; }

        public LegalMemoDetailsDto Memo { get; set; }

        public string Board { get; set; }

        public string LegalBoardType { get; set; }


    }
    public class BoardMeetingDto
    {
        public int? Id { get; set; }

        public int MemoId { get; set; }

        public int BoardId { get; set; }

        public string MeetingPlace { get; set; }

        public DateTime MeetingDate { get; set; }

        public ICollection<int> BoardMeetingMembers { get; set; } = new List<int>();
    }
}
