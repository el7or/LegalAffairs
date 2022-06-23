using Moe.La.Core.Entities.Litigation.BoardMeeting;
using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    public class LegalBoard : BaseEntity<int>
    {
        public string Name { get; set; }

        public LegalBoardStatuses StatusId { get; set; }

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

        public LegalBoardTypes LegalBoardTypeId { get; set; }

        /// <summary>
        /// The legal board membership.
        /// </summary>
        public ICollection<LegalBoardMember> LegalBoardMembers { get; set; } = new List<LegalBoardMember>();

        /// <summary>
        /// The legal board memos.
        /// </summary>
        public ICollection<LegalBoardMemo> LegalBoardMemos { get; set; } = new List<LegalBoardMemo>();
        public ICollection<BoardMeeting> BoardMeetings { get; set; } = new List<BoardMeeting>();
    }
}
