using Moe.La.Core.Entities.Litigation.BoardMeeting;
using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    public class LegalMemo : BaseEntity<int>
    {
        public string Name { get; set; }

        public LegalMemoStatuses Status { get; set; }

        public LegalMemoTypes Type { get; set; }

        public string Text { get; set; }

        public int ReviewNumber { get; set; }

        public bool? IsRead { get; set; }

        public int InitialCaseId { get; set; }

        public Case InitialCase { get; set; }

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

        public DateTime? RaisedOn { get; set; }

        public string DeletionReason { get; set; }

        public int SecondSubCategoryId { get; set; }

        public SecondSubCategory SecondSubCategory { get; set; }

        public ICollection<LegalBoardMemo> LegalBoardMemos { get; set; } = new List<LegalBoardMemo>();

        public ICollection<BoardMeeting> BoardMeetings { get; set; } = new List<BoardMeeting>();
    }
}

