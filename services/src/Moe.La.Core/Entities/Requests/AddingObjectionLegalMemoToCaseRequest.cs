using System;

namespace Moe.La.Core.Entities
{
    public class AddingObjectionLegalMemoToCaseRequest : BaseEntity<int>
    {
        /// <summary>
        /// The related case id.
        /// </summary>
        public int? CaseId { get; set; }

        /// <summary>
        /// The related case.
        /// </summary>
        public Case Case { get; set; }

        public Request Request { get; set; }

        public int LegalMemoId { get; set; }

        public LegalMemo LegalMemo { get; set; }

        public string ReplyNote { get; set; }

        public DateTime? ReplyDate { get; set; }
    }
}
