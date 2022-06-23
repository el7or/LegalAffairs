using System.Collections.Generic;

namespace Moe.La.Core.Dtos
{
    public class CaseSupportingDocumentRequestHistoryListItemDto
    {
        public int Id { get; set; }

        public int CaseId { get; set; }

        public int? ParentId { get; set; }

        public int HearingId { get; set; }

        public RequestListItemDto Request { get; set; }

        public KeyValuePairsDto<int> ConsigneeDepartment { get; set; }

        public string ReplyNote { get; set; }

        public int AttachedLetterRequestCount { get; set; }

        public int? AttachedLetterRequestId { get; set; }

        public ICollection<CaseSupportingDocumentRequestItemDto> Documents { get; set; } = new List<CaseSupportingDocumentRequestItemDto>();

    }

}
