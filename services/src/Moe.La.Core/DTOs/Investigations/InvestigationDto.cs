using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Dtos
{
    public class InvestigationListItemDto : BaseDto<int>
    {
        public int InvestigationNumber { get; set; }

        public DateTime StartDate { get; set; }
        public string StartDateHigri { get; set; }
        public string StartTime { get; set; }

        public string Subject { get; set; }

        public string InvestigatorFullName { get; set; }

        public string InvestigationStatus { get; set; }

        public bool IsHasCriminalSide { get; set; }
    }

    public class InvestigationDetailsDto
    {
        public int Id { get; set; }
        public int InvestigationNumber { get; set; }
        public ICollection<InvestigationRecordDto> InvestigationRecords { get; set; }


    }
    public class InvestigationDto
    {
        public int Id { get; set; }
        public int InvestigationNumber { get; set; }
        public DateTime StartDate { get; set; }
        public string StartDateHijri { get; set; }
        public string Subject { get; set; }
        public KeyValuePairsDto<Guid> Investigator { get; set; }
        public InvestigationStatuses InvestigationStatus { get; set; }
        public bool IsHasCriminalSide { get; set; }
        public KeyValuePairsDto<Guid> CreatedBy { get; set; }
        public KeyValuePairsDto<Guid> UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedOnHigri { get; set; }
    }

}
