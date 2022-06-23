using System;

namespace Moe.La.Core.Entities
{
    public class LegalBoardMemo : BaseEntity<int>
    {
        /// <summary>
        /// The legal memo ID.
        /// </summary>
        public int LegalMemoId { get; set; }

        /// <summary>
        /// The legal memo object.
        /// </summary>
        public LegalMemo LegalMemo { get; set; }

        /// <summary>
        /// The board ID.
        /// </summary>
        public int LegalBoardId { get; set; }

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
