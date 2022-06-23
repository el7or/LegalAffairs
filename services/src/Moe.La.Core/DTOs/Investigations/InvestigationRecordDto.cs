using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Dtos
{
    public class InvestigationRecordListItemDto : BaseDto<int>
    {
        public int InvestigationId { get; set; }

        public DateTime StartDate { get; set; }

        public string StartDateHigri { get; set; }
        public string StartTime { get; set; }

        public DateTime EndDate { get; set; }
        public string EndDateHigri { get; set; }
        public string EndTime { get; set; }
        public string RecordNumber { get; set; }

        public KeyValuePairsDto<int> RecordStatus { get; set; }

        public string createdBy { get; set; }

    }

    public class InvestigationRecordDetailsDto : BaseDto<int>
    {
        public int InvestigationId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string RecordNumber { get; set; }

        public string Visuals { get; set; }

        public RecordStatuses RecordStatus { get; set; }

        public bool IsRemote { get; set; }
        public string Subject { get; set; }

        public List<AttachmentListItemDto> Attachments { get; set; }

        public ICollection<InvestigationRecordPartyDto> InvestigationRecordParties { get; set; }

        public ICollection<InvestigationRecordQuestionDto> InvestigationRecordQuestions { get; set; }

        public ICollection<InvestigationRecordInvestigatorDto> InvestigationRecordInvestigators { get; set; }

        public ICollection<InvestigationRecordAttendantDto> Attendants { get; set; }

    }

    public class InvestigationRecordDto
    {
        public int? Id { get; set; }

        public int InvestigationId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string RecordNumber { get; set; }

        public string Visuals { get; set; }

        public RecordStatuses RecordStatus { get; set; }

        public bool IsRemote { get; set; }
        public string Subject { get; set; }

        public List<AttachmentDto> Attachments { get; set; } = new List<AttachmentDto>();

        public ICollection<InvestigationRecordPartyDto> InvestigationRecordParties { get; set; }

        public ICollection<InvestigationRecordQuestionDto> InvestigationRecordQuestions { get; set; }

        public ICollection<Guid> InvestigationRecordInvestigators { get; set; }

        public ICollection<InvestigationRecordAttendantDto> Attendants { get; set; }
    }


}
