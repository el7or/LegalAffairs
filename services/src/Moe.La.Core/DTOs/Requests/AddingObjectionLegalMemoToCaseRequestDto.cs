using System;

namespace Moe.La.Core.Dtos
{
    public class AddingObjectionLegalMemoToCaseRequestDto
    {
        public int Id { get; set; }

        public LegalMemoDetailsDto LegalMemo { get; set; }

        public RequestListItemDto Request { get; set; }

        public CaseDetailsDto Case { get; set; }

        public string ReplyNote { get; set; }
        public DateTime? ReplyDate { get; set; }

        public int CaseId { get; set; }
        public int LegalMemoId { get; set; }
    }
}
