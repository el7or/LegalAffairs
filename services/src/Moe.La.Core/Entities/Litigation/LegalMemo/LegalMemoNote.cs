using System;

namespace Moe.La.Core.Entities
{
    public class LegalMemoNote : BaseEntity<int>
    {
        public int LegalMemoId { get; set; }

        public LegalMemo LegalMemo { get; set; }

        public int ReviewNumber { get; set; }

        public string Text { get; set; }

        public bool IsClosed { get; set; }

        /// <summary>
        /// The board ID.
        /// </summary>
        public int? LegalBoardId { get; set; }

        /// <summary>
        /// The board object.
        /// </summary>
        public LegalBoard LegalBoard { get; set; }

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
