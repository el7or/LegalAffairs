using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Dtos
{
    public class HearingListItemDto
    {
        public int Id { get; set; }

        public CaseListItemDto Case { get; set; }

        public string Court { get; set; }

        public KeyValuePairsDto<Guid> AssignedTo { get; set; }

        public string Status { get; set; }

        public string Type { get; set; }

        public int? HearingNumber { get; set; }

        public DateTime HearingDate { get; set; }

        public string HearingDateHigri { get; set; } = null;

        public string HearingTime { get; set; }

        public string HearingDesc { get; set; }

        public string ClosingReport { get; set; }

        public string CreatedOnHigri { get; set; } = null;

        public CaseSupportingDocumentRequestListItemDto CaseSupportingDocumentRequest { get; set; }
    }

    public class HearingDetailsDto
    {
        public int Id { get; set; }

        public CaseDetailsDto Case { get; set; }

        public KeyValuePairsDto<int> Court { get; set; }

        public KeyValuePairsDto<Guid> AssignedTo { get; set; }

        public KeyValuePairsDto<int> Status { get; set; }

        public KeyValuePairsDto<int> Type { get; set; }

        public string CircleNumber { get; set; }

        public int? HearingNumber { get; set; }
        public Guid? AssignedToId { get; set; }


        public DateTime HearingDate { get; set; }

        public string HearingDateHigri { get; set; } = null;

        public string CreatedOnHigri { get; set; } = null;

        public string HearingTime { get; set; }

        public string HearingDesc { get; set; }

        public string Motions { get; set; }

        public string Summary { get; set; }

        public string Attendees { get; set; }

        public string SessionMinutes { get; set; }

        public bool? IsPronouncedJudgment { get; set; } = false;

        public bool? IsEditable { get; set; } = true;

        public string ClosingReport { get; set; }

        public bool IsHasNextHearing { get; set; } = false;

        public DateTime? UpdatedOn { get; set; }

        public ICollection<KeyValuePairsDto<int>> LegalMemos { get; set; } = new List<KeyValuePairsDto<int>>();

        public ICollection<CaseSupportingDocumentRequestListItemDto> CaseSupportingDocumentRequests { get; set; }

        public AddingLegalMemoToHearingRequestListItemDto HearingLegalMemoReviewRequest { get; set; }

        public List<AttachmentListItemDto> Attachments { get; set; }


    }

    public class HearingDto
    {
        public int Id { get; set; }

        public int CaseId { get; set; }

        public HearingStatuses Status { get; set; }

        public HearingTypes Type { get; set; }

        public int CourtId { get; set; }

        public string CircleNumber { get; set; }

        public int HearingNumber { get; set; }

        public DateTime HearingDate { get; set; }

        public string HearingTime { get; set; }

        public string HearingDesc { get; set; }

        public string Motions { get; set; }

        public string Summary { get; set; }

        public int? LitigationTypeId { get; set; }

        public string Attendees { get; set; }

        public string SessionMinutes { get; set; }

        public bool? IsPronouncedJudgment { get; set; } = false;

        public string ClosingReport { get; set; }

        public bool? WithNewHearing { get; set; }

        public HearingDto NewHearing { get; set; }

        public int BranchId { get; set; }

        public Guid? AssignedToId { get; set; }
        public List<AttachmentDto> Attachments { get; set; } = new List<AttachmentDto>();
    }

    public class HearingNumberDto
    {
        public int Id { get; set; }

        public int CaseId { get; set; }

        public int HearingNumber { get; set; }
    }

    public class ResearcherHearingDto
    {
        public Guid ResearcherId { get; set; }
        public IEnumerable<Hearing> Hearings { get; set; }
    }

}
