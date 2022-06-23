using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Dtos
{
    public class CaseDetailsDto : BaseDto<int>
    {
        public string RaselRef { get; set; }

        public string RaselUnifiedNo { get; set; }

        public string CaseNumberInSource { get; set; }

        public string NajizRef { get; set; }

        public string NajizId { get; set; }

        public string MoeenRef { get; set; }

        public KeyValuePairsDto<int> CaseSource { get; set; }

        public KeyValuePairsDto<int> LitigationType { get; set; }

        public string ReferenceCaseNo { get; set; }

        public string MainNo { get; set; }

        public DateTime? StartDate { get; set; }

        public string StartDateHigri { get; set; } = null;

        public string ReceivedDateHigri { get; set; } = null;

        public int? CaseYearInSourceHijri { get; set; }

        public int? CourtId { get; set; }

        public KeyValuePairsDto<int> Court { get; set; }

        public string CircleNumber { get; set; }

        public string Subject { get; set; }

        public bool IsManual { get; set; }

        public KeyValuePairsDto<int> LegalStatus { get; set; }

        public string CaseDescription { get; set; }

        public string OrderDescription { get; set; }

        public KeyValuePairsDto<int> Status { get; set; }

        public int? BranchId { get; set; }

        public string BranchName { get; set; }

        public DateTime? RecordDate { get; set; }

        public string FileNo { get; set; }

        public string JudgeName { get; set; }

        public DateTime? CloseDate { get; set; }

        public int? RelatedCaseId { get; set; }

        public CaseListItemDto RelatedCase { get; set; }

        public bool FinishedPronouncedHearing { get; set; }

        //عدد الايام المتبقية على مهلة الاعتراض
        public int? RemainingObjetcion { get; set; }

        public DateTime? PronouncingJudgmentDate { get; set; }

        public string PronouncingJudgmentDateHijri { get; set; } = null;
        public DateTime? ReceivingJudgmentDate { get; set; }

        public string ReceivingJudgmentDateHijri { get; set; } = null;
        public string ObjectionJudgmentLimitDateHijri { get; set; } = null;

        public ReceiveJdmentInstrumentDetailsDto CaseRule { get; set; }

        public int CaseRuleId { get; set; }

        public bool? IsJudgementImport { get; set; }

        public DateTime? JudgementImportDate { get; set; }

        public string JudgementText { get; set; }

        public bool? IsFinalJudgment { get; set; }
        public JudgementResults JudgementResults { get; set; }
        public string JudgementResult { get; set; }

        public string JudgmentBrief { get; set; }

        public int AttachmentsCount { get; set; }

        public string Notes { get; set; }

        //public DateTime? ObjectionJudgmentLimitDate { get; set; }

        public CaseClosinReasons? CaseClosingReason { get; set; }

        public List<AttachmentListItemDto> Attachments { get; set; }

        public KeyValuePairsDto<int> SecondSubCategory { get; set; }

        public SecondSubCategoryDto SubCategory { get; set; }

        public ICollection<CaseTransactionDetailsDto> CaseTransactions { get; set; } = new List<CaseTransactionDetailsDto>();

        public ICollection<CaseGroundsDto> CaseGrounds { get; set; } = new List<CaseGroundsDto>();

        public ICollection<HearingDetailsDto> Hearings { get; set; } = new List<HearingDetailsDto>();

        public ICollection<KeyValuePairsDto<Guid>> Researchers { get; set; } = new List<KeyValuePairsDto<Guid>>();

        public ICollection<CasePartyDto> CaseParties { get; set; } = new List<CasePartyDto>();

        public ICollection<CaseMoamalatDto> CaseMoamalat { get; set; } = new List<CaseMoamalatDto>();

        public ICollection<PartyDto> Parties { get; set; } = new List<PartyDto>();

        public ICollection<CaseRuleProsecutorRequestDto> ProsecutorRequests { get; set; }
    }
    public class CaseRuleProsecutorRequestDto
    {
        public int Id { get; set; }
        public string ProsecutorRequestSubject { get; set; }
        public string ProsecutorRequestOrder { get; set; }

        public int CaseId { get; set; }

    }
}