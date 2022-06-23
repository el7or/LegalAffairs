using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Entities
{
    public class LegalMemoHistory : BaseEntity<int>
    {
        /// <summary>
        /// The legal memo related to.
        /// </summary>
        public int LegalMemoId { get; set; }

        /// <summary>
        /// A navigation property to legal memo.
        /// </summary>
        public LegalMemo LegalMemo { get; set; }

        /// <summary>
        /// The memo's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The memo's status.
        /// </summary>
        public LegalMemoStatuses Status { get; set; }

        /// <summary>
        /// The memo's text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The revision number.
        /// </summary>
        public int ReviewNumber { get; set; }

        /// <summary>
        /// Determine that the memo has been read or not.
        /// </summary>
        public bool? IsRead { get; set; }
        /// <summary>
        /// The hearing Id  of  the memo.
        /// </summary>
        public int InitialCaseId { get; set; }

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
