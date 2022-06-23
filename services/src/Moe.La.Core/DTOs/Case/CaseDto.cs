using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Dtos
{
    public class CaseDto
    {
        public int Id { get; set; }

        public int? RelatedCaseId { get; set; }

        public int? MoamalaId { get; set; }

        public int? HearingsCount { get; set; }

        public string RaselRef { get; set; }

        public string RaselUnifiedNo { get; set; }

        public string CaseNumberInSource { get; set; }

        public string NajizRef { get; set; }

        public string NajizId { get; set; }

        public string MoeenRef { get; set; }

        public CaseSources CaseSource { get; set; }

        public int SecondSubCategoryId { get; set; }

        public LitigationTypes LitigationType { get; set; }

        public string ReferenceCaseNo { get; set; }

        public string MainNo { get; set; }

        public DateTime StartDate { get; set; }

        public int CourtId { get; set; }

        public string CircleNumber { get; set; }

        public string Subject { get; set; }

        public MinistryLegalStatuses LegalStatus { get; set; }

        public string CaseDescription { get; set; }

        public ICollection<CaseGroundsDto> CaseGrounds { get; set; }

        public string OrderDescription { get; set; }

        public CaseStatuses Status { get; set; }

        public DateTime? RecordDate { get; set; }

        public string FileNo { get; set; }

        public string JudgeName { get; set; }

        public DateTime? CloseDate { get; set; }

        public Guid? WorkflowInstanceId { get; set; }

        public DateTime? PronouncingJudgmentDate { get; set; }

        public DateTime? ReceivingJudgmentDate { get; set; }

        public string Notes { get; set; }

        public bool IsManual { get; set; }

        public CaseClosinReasons? CaseClosingReason { get; set; }

        public List<AttachmentDto> Attachments { get; set; } = new List<AttachmentDto>();

        public ICollection<CasePartyDto> Parties { get; set; }

        public ICollection<HearingDto> Hearings { get; set; }
    }

    public class CaseAttachmentsListDto
    {
        public int CaseId { get; set; }

        public List<AttachmentDto> Attachments { get; set; } = new List<AttachmentDto>();

    }

    public class InitialCaseDto
    {
        public int Id { get; set; }
        public CaseSources CaseSource { get; set; }
        public string CaseNumberInSource { get; set; }
        public KeyValuePairsDto<int> CaseCategory { get; set; }

        public DateTime StartDate { get; set; }

    }

    public class NextCaseDto
    {
        public int? Id { get; set; }
        public string CaseNumberInSource { get; set; }
        public DateTime StartDate { get; set; }
        public string CircleNumber { get; set; }
        public int CourtId { get; set; }
        public int RelatedCaseId { get; set; }
        public LitigationTypes LitigationType { get; set; }
        public CaseStatuses Status { get; set; }
        public int BranchId { get; set; }
        public CaseSources CaseSource { get; set; }

    }

    public class ObjectionCaseDto
    {
        public int Id { get; set; }
        public CaseSources CaseSource { get; set; }

        public string CaseSourceNumber { get; set; }

        public ICollection<KeyValuePairsDto<int>> CaseCategories { get; set; }

        public LitigationTypes LitigationType { get; set; }

        public int CourtId { get; set; }

        public string CircleNumber { get; set; }
        public int RelatedCaseId { get; set; }

        public Guid ResearcherId { get; set; }

        public int GeneralManagmentId { get; set; }

        public DateTime StartDate { get; set; }
    }

    public class CasePartyDto : BaseDto<int>
    {
        public int CaseId { get; set; }

        public int PartyId { get; set; }

        public PartyDto Party { get; set; }

        public PartyStatus? PartyStatus { get; set; }

        public string PartyStatusName { get; set; }
    }

    public class BasicCaseDto
    {
        public int Id { get; set; }

        public int? RelatedCaseId { get; set; }

        public int? MoamalaId { get; set; }

        public string CaseNumberInSource { get; set; }

        public CaseSources CaseSource { get; set; }

        public int SecondSubCategoryId { get; set; }

        public LitigationTypes LitigationType { get; set; }

        public DateTime StartDate { get; set; }

        public int CourtId { get; set; }

        public string CircleNumber { get; set; }

        public string Subject { get; set; }

        public MinistryLegalStatuses LegalStatus { get; set; }

        public string CaseDescription { get; set; }

        public CaseStatuses Status { get; set; }

        public string JudgeName { get; set; }

        public string Notes { get; set; }

        public bool IsManual { get; set; }
    }
}