using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Dtos
{
    public class CaseListItemDto : BaseDto<int>
    {
        public string AddUser { get; set; }

        public string RaselRef { get; set; }

        public string RaselUnifiedNo { get; set; }

        public string CaseNumberInSource { get; set; }

        public string NajizRef { get; set; }

        public string NajizId { get; set; }

        public string MoeenRef { get; set; }

        public string CreatedOnHigri { get; set; } = null;

        public string CreatedOnTime { get; set; }

        public DateTime? ReceivedDate { get; set; }

        public string ReceivedDateHigri { get; set; } = null;

        public string ReceivedTime { get; set; }

        public string ReceivedStatus { get; set; }

        public KeyValuePairsDto<int> CaseSource { get; set; }
        public KeyValuePairsDto<int> LitigationType { get; set; }

        public string MainNo { get; set; }

        public DateTime? StartDate { get; set; }

        public string StartDateHigri { get; set; } = null;

        public string Court { get; set; }

        public string CircleNumber { get; set; }

        public string Subject { get; set; }

        public string LegalStatus { get; set; }

        public string CaseDescription { get; set; }

        public bool IsManual { get; set; }

        public bool? IsDuplicated { get; set; }

        public string OrderDescription { get; set; }

        public string Branch { get; set; }

        public DateTime? RecordDate { get; set; }

        public string FileNo { get; set; }

        public string JudgeName { get; set; }

        public DateTime? CloseDate { get; set; }

        public KeyValuePairsDto<int> Status { get; set; }

        public int HearingsCount { get; set; }

        public int? RelatedCaseId { get; set; }

        public string RelatedCaseRef { get; set; }

        public CaseListItemDto RelatedCase { get; set; }

        public string Notes { get; set; }

        public DateTime? PronouncingJudgmentDate { get; set; } = null;
        public string PronouncingJudgmentDateHigri { get; set; } = null;

        public DateTime? ReceivingJudgmentDate { get; set; } = null;
        public string ReceivingJudgmentDateHigri { get; set; } = null;

        public string ObjectionJudgmentLimitDateHijri { get; set; } = null;

        public bool IsCaseDataCompleted { get; set; }
        public bool FinishedPronouncedHearing { get; set; }

        public bool IsFinalJudgment { get; set; }
        //عدد الايام المتبقية على مهلة الاعتراض
        public int? RemainingObjetcion { get; set; }

        public CaseClosinReasons? CaseClosingReason { get; set; }

        public string SecondSubCategory { get; set; }
        public int SecondSubCategoryId { get; set; }

        public ICollection<string> CaseGrounds { get; set; } = new List<string>();

        public ICollection<PartyDto> Parties { get; set; } = new List<PartyDto>();

        public ICollection<KeyValuePairsDto<Guid>> Researchers { get; set; } = new List<KeyValuePairsDto<Guid>>();

        public ReceiveJdmentInstrumentDetailsDto CaseRule { get; set; }

    }
}