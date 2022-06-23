using System;

namespace Moe.La.Core.Entities
{
    public class HearingLegalMemo : BaseEntity<int>
    {
        public int HearingId { get; set; }

        public Hearing Hearing { get; set; }

        public int LegalMemoId { get; set; }

        public LegalMemo LegalMemo { get; set; }

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
