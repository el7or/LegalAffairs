using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Dtos
{
    public class ObjectionPermitRequestListItemDto
    {
        public int Id { get; set; }

        public int? CaseId { get; set; }

        public RequestListItemDto Request { get; set; }

        public KeyValuePairsDto<int> SuggestedOpinon { get; set; }

        public string Note { get; set; }

        public string ReplyNote { get; set; }

        public string CaseNumberInSource { get; set; }

        public string CaseSource { get; set; }
        public int? CaseYearInSourceHijri { get; set; }


    }

    public class ObjectionPermitRequestDetailsDto
    {
        public int Id { get; set; }

        public int? CaseId { get; set; }

        public RequestDto Request { get; set; }

        public SuggestedOpinon SuggestedOpinon { get; set; }

        public string Note { get; set; }

        public string ReplyNote { get; set; }
    }

    public class ObjectionPermitRequestDto
    {
        public int Id { get; set; }

        public int CaseId { get; set; }

        public SuggestedOpinon SuggestedOpinon { get; set; }

        public RequestStatuses RequestStatus { get; set; }

        public string Note { get; set; }

        public RequestDto Request { get; set; }

        public Guid? ResearcherId { get; set; }
    }


    public class ObjectionPermitRequestForPrintDto
    {
        public int CaseId { get; set; }

        public CaseListItemDto Case { get; set; }

        public string Defendant { get; set; }

        public string Plaintiff { get; set; }

        public DateTime CaseDate { get; set; }

        public string Note { get; set; }

        public RequestListItemDto Request { get; set; }
    }
    public class ReplyObjectionPermitRequestDto
    {
        public int Id { get; set; }

        public int CaseId { get; set; }

        public string ReplyNote { get; set; }

        public RequestStatuses RequestStatus { get; set; }

        public Guid ResearcherId { get; set; }
    }

    public class AllowObjectionPermitRequestDto
    {
        public int CaseId { get; set; }
        public bool IsAllow { get; set; }
    }

    public class ReplyObjectionLegalMemoRequestDto
    {
        public int Id { get; set; }

        public string ReplyNote { get; set; }

        public RequestStatuses RequestStatus { get; set; }
    }
}
