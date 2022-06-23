using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Dtos
{
    public class CaseSupportingDocumentRequestListItemDto
    {
        public int Id { get; set; }

        public int? CaseId { get; set; }

        public int? ParentId { get; set; }

        public int HearingId { get; set; }

        public RequestListItemDto Request { get; set; }

        public KeyValuePairsDto<int> ConsigneeDepartment { get; set; }

        public string ReplyNote { get; set; }

        public string TransactionNumberInAdministrativeCommunications { get; set; }

        public DateTime? TransactionDateInAdministrativeCommunications { get; set; }

        public int AttachedLetterRequestCount { get; set; }

        public int? AttachedLetterRequestId { get; set; }

        public RequestStatuses AttachedLetterRequestStatus { get; set; }

        public string CaseSource { get; set; }

        public string CaseNumberInSource { get; set; }

        public int CaseYearInSource { get; set; }

        public int CaseYearInSourceHijri { get; set; }

        public ICollection<CaseSupportingDocumentRequestItemDto> Documents { get; set; } = new List<CaseSupportingDocumentRequestItemDto>();
        public ICollection<CaseSupportingDocumentRequestHistoryListItemDto> History { get; set; } = new List<CaseSupportingDocumentRequestHistoryListItemDto>();
    }

    public class CaseSupportingDocumentRequestDto
    {
        public int Id { get; set; }

        public int CaseId { get; set; }

        public int HearingId { get; set; }

        public int? ParentId { get; set; }

        public RequestDto Request { get; set; }

        public int? ConsigneeDepartmentId { get; set; }

        public string ReplyNote { get; set; }

        public ICollection<CaseSupportingDocumentRequestItemDto> Documents { get; set; } = new List<CaseSupportingDocumentRequestItemDto>();
    }


    public class ReplyCaseSupportingDocumentRequestDto
    {
        public int Id { get; set; }
        public int CaseId { get; set; }

        public string ReplyNote { get; set; }

        public RequestStatuses RequestStatus { get; set; }
        public string TransactionNumberInAdministrativeCommunications { get; set; }
        public DateTime? TransactionDateInAdministrativeCommunications { get; set; }
        public int? ConsigneeDepartmentId { get; set; }
    }

    public class AttachedLetterRequestDto
    {
        public int? Id { get; set; }

        public int HearingId { get; set; }

        public int? ParentId { get; set; }

        public RequestDto Request { get; set; }

        public int consigneeDepartmentId { get; set; }

        public string CaseNumberInSource { get; set; }

        public string CaseSource { get; set; }

        public int CaseYearInSourceHijri { get; set; }

    }
    public class CaseSupportingDocumentRequestForPrintDto
    {
        public int CaseId { get; set; }
        public string Defendant { get; set; }
        public string Plaintiff { get; set; }
        public DateTime CaseDate { get; set; }
        public ICollection<CaseSupportingDocumentRequestItemDto> Documents { get; set; } = new List<CaseSupportingDocumentRequestItemDto>();
    }

}
