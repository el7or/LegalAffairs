using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Entities
{
    public class QueryObject
    {
        public string SortBy { get; set; }

        public bool IsSortAscending { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }
    }

    public class UserQueryObject : QueryObject
    {
        public bool Enabled { get; set; } = false;
        public string Roles { get; set; }
        public int? BranchId { get; set; }
        public int? DepartmetId { get; set; }
        public int? WorkItemTypeId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string IdentityNumber { get; set; }
        public string SearchText { get; set; }

        public bool? HasConfidentialPermission { get; set; }
    }
    public class ResearcherQueryObject : QueryObject
    {
        public Guid? ResearcherId { get; set; }
        public Guid? ConsultantId { get; set; }
        public bool HasConsultant { get; set; } = false;
        public bool AllDepartments { get; set; } = false;
        public bool? IsSameGeneralManagement { get; set; }

    }

    public class BranchQueryObject : QueryObject
    {
        public int? SectorId { get; set; }
        public bool IsParent { get; set; }
        public string Name { get; set; }

    }

    public class ProvinceQueryObject : QueryObject
    {
        public bool? WithCities { get; set; }
    }
    public class CityQueryObject : QueryObject
    {
        public int? ProvinceId { get; set; }
    }

    public class DistrictQueryObject : QueryObject
    {
        public int? CityId { get; set; }
    }

    public class CaseCategoryQueryObject : QueryObject
    {
        public CaseSources? CaseSource { get; set; }
        public string Name { get; set; }
    }
    public class AdversaryQueryObject : QueryObject
    {
        public int? PartyTypeId { get; set; }

        public int? ProvinceId { get; set; }

        public int? CityId { get; set; }

        public bool? Enabled { get; set; }
    }
    public class AttachmentQueryObject : QueryObject
    {
        /// <summary>
        /// Case or Hearing or LegalMemo ...etc
        /// </summary>
        public GroupNames GroupName { get; set; } = 0;

        /// <summary>
        /// CaseId or HearingId or LegalMemoId ...etc
        /// </summary>
        public int GroupId { get; set; }
    }

    public class PartyQueryObject : QueryObject
    {
        public PartyTypes? PartyType { get; set; }
        public int? IdentityTypeId { get; set; }
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        public string Name { get; set; }
        public string PartyTypeName { get; set; }
        public string IdentityValue { get; set; }
    }

    public class NotificationSystemQueryObject : QueryObject
    {
        public bool? IsRead { get; set; }
        public bool? IsForCurrentUser { get; set; }
    }
    public class NotificationSMSQueryObject : QueryObject
    {
        public bool? IsSent { get; set; }
        public bool? IsForCurrentUser { get; set; }
    }
    public class NotificationEmailQueryObject : QueryObject
    {
        public bool? IsSent { get; set; }
        public bool? IsForCurrentUser { get; set; }
    }

    public class CaseQueryObject : QueryObject
    {
        public DateTime? StartDateFrom { get; set; }
        public DateTime? StartDateTo { get; set; }
        public int? CaseSource { get; set; }
        public int? LegalStatus { get; set; }
        public int? LitigationType { get; set; }
        public int? ReceivedStatus { get; set; }
        public int? Status { get; set; }
        public string CourtId { get; set; }
        public string CircleNumber { get; set; }
        public string ReferenceCaseNo { get; set; }
        public int? Id { get; set; }
        public Guid? AddUserId { get; set; }
        public string Subject { get; set; }
        public int? CourtTypeId { get; set; }
        public int? Period { get; set; }
        public string SearchText { get; set; }
        public bool? IsFinalJudgment { get; set; }
        public bool? IsClosedCase { get; set; }
        public bool? IsCaseDataCompleted { get; set; }
        public bool? IsForHearingAddition { get; set; }
        public bool? IsManual { get; set; }
        public string PartyName { get; set; }
        public bool? IsForChooseRelatedCase { get; set; }
        public LegalMemoTypes? LegalMemoType { get; set; }
    }

    public class HearingQueryObject : QueryObject
    {
        public int? CaseId { get; set; }
        public int? CourtId { get; set; }
        public int? Status { get; set; }

        public int? HearingNumber { get; set; }
        public string HearingDate { get; set; }
        public Guid? Consultant‏Id‏ { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public bool? Closed { get; set; }
        public string SearchText { get; set; }
    }

    public class HearingUpdateQueryObject : QueryObject
    {
        public string SearchText { get; set; }
        public int HearingId { get; set; }

        public string UpdateDate { get; set; }

        public Guid? CreatedBy { get; set; }

        public string Attachment { get; set; }

    }


    public class MoamalaTransactionQueryObject : QueryObject
    {
        public int? TransactionType { get; set; }
        public string Subject { get; set; }
        public string ReferenceNo { get; set; }
        public string UnifiedNo { get; set; }
        public string SendingDepartment { get; set; }
        public string SearchText { get; set; }
    }

    public class RequestQueryObject : QueryObject
    {
        public int? RequestType { get; set; }
        public int? RequestStatus { get; set; }
        public int? CaseId { get; set; }
    }
    public class TransactionQueryObject : QueryObject
    {
        public int? RequestType { get; set; }
        public int? RequestStatus { get; set; }
        public int? RequestId { get; set; }
    }

    public class ConsultationTransactionQueryObject : QueryObject
    {
        public int? ConsultationStatus { get; set; }
        public int? ConsultationId { get; set; }
    }
    public class CaseChangeConsultantRequestQueryObject : QueryObject
    {
        public int? CaseId { get; set; }
        public string AddUserId { get; set; }
        public bool? IsAccept { get; set; }
    }

    public class ExecutiveCaseQueryObject : QueryObject
    {
        public string AddUserId { get; set; }
        public string AdversaryName { get; set; }
        public string CircleNumber { get; set; }
        public bool? Closed { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string ClosedFrom { get; set; }
        public string ClosedTo { get; set; }
    }
    public class PoliceCaseQueryObject : QueryObject
    {
        public string AddUserId { get; set; }
        public string AdversaryName { get; set; }
        public string DepartmentName { get; set; }
        public bool? Closed { get; set; }
        public int? TypeId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string ClosedFrom { get; set; }
        public string ClosedTo { get; set; }
    }
    public class ExecutiveCaseProcedureQueryObject : QueryObject
    {
        public int? ExecutiveCaseId { get; set; }
        public string AddUserId { get; set; }
    }
    public class PoliceCaseProcedureQueryObject : QueryObject
    {
        public int? PoliceCaseId { get; set; }
        public string AddUserId { get; set; }
    }

    //public class ConsultationQueryObject : QueryObject
    //{
    //    public string AddUserId { get; set; }
    //    public int? TypeId { get; set; }
    //    public string Subject { get; set; }
    //    public int? SectorId { get; set; }
    //    public int? GeneralManagementId { get; set; }
    //    public string From { get; set; }
    //    public string To { get; set; }
    //    public string StageId { get; set; }
    //    public Guid? Consultant‏Id‏ { get; set; }
    //    public string ConsultantAssignmentDate { get; set; }
    //    public string QuerySummary { get; set; }
    //}

    public class ConsultationRequestFileQueryObject : QueryObject
    {
        public int? ConsultationId { get; set; }
        public string Subject { get; set; }
        public string AssignedUserId { get; set; }
        public bool? Closed { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public Guid? Consultant‏Id‏ { get; set; }
        public bool? ConsultationApproved { get; set; }
    }


    public class LegalMemoQueryObject : QueryObject
    {
        public string Name { get; set; }

        public int? SecondSubCategoryId { get; set; }

        public string Status { get; set; }

        public LegalMemoTypes? Type { get; set; }

        public Guid? CreatedBy { get; set; }

        public string CreatedOn { get; set; }

        public string ApprovalFrom { get; set; }

        public string ApprovalTo { get; set; }

        public string UpdatedOn { get; set; }

        public string SearchText { get; set; }

        public bool IsReview { get; set; } = false;

        public bool isBoardReview { get; set; }

        public int? InitialCaseId { get; set; }
    }
    public class LegalBoardQueryObject : QueryObject
    {

    }

    public class LegalBoardMemberQueryObject : QueryObject
    {
        public Guid? UserId { get; set; }
        public int? LegalBoardId { get; set; }
    }
    public class BoardMeetingQueryObject : QueryObject
    {
        public DateTime? MeetingDateFrom { get; set; }
        public DateTime? MeetingDateTo { get; set; }
        public string MeetingPlace { get; set; }
        public string SearchText { get; set; }
    }

    public class LegalMemoNoteQueryObject : QueryObject
    {
        public int? LegalMemoId { get; set; }
    }

    public class InvestiationRecordQueryObject : QueryObject
    {
        public int? InvestigationId { get; set; }
        public string SearchText { get; set; }
    }

    public class InvestigationQueryObject : QueryObject
    {
        public string SearchText { get; set; }
    }

    public class InvestigationQuestionQueryObject : QueryObject
    {
        public string Question { get; set; }

    }

    public class MoamalatQueryObject : QueryObject
    {
        public string SearchText { get; set; }
        public int? Status { get; set; }
        public int? ConfidentialDegree { get; set; }
        public int? SenderDepartmentId { get; set; }
        public int? ReceiverDepartmentId { get; set; }
        public Guid? AssignedToId { get; set; }
        public DateTime? CreatedOnFrom { get; set; }
        public DateTime? CreatedOnTo { get; set; }
        public bool HasConfidentialAccess { get; set; } = false;
        public int? RelatedId { get; set; }

    }

    public class MoamalatRaselQueryObject : QueryObject
    {
        public string SearchText { get; set; }
        public int? Status { get; set; }
        public ConfidentialDegrees? ItemPrivacy { get; set; }
        public DateTime? CreatedOnFrom { get; set; }
        public DateTime? CreatedOnTo { get; set; }
    }

    public class SubWorkItemTypeQueryObject : QueryObject
    {
        public int? WorkItemTypeId { get; set; }
    }

    public class WorkItemTypeQueryObject : QueryObject
    {
        public int? DepartmentId { get; set; }
    }

    public class ConsultationQueryObject : QueryObject
    {
        public string SearchText { get; set; }
        public int? DepartmentId { get; set; }

        public Guid? AssignedTo { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public int? WorkItemTypeId { get; set; }

        public ConsultationStatus? Status { get; set; }

        public bool HasConfidentialAccess { get; set; } = false;


    }
    public class CourtQueryObject : QueryObject
    {
        public LitigationTypes? LitigationType { get; set; }
        public CourtCategories? CourtCategory { get; set; }

    }
    public class MinistryDepartmentQueryObject : QueryObject
    {
        public int? MinistrySectorId { get; set; }
    }

    public class MainCategoryQueryObject : QueryObject
    {
        public CaseSources CaseSource { get; set; }
        public bool? WithFirstSubCategories { get; set; }
    }
    public class FirstSubCategoriesQueryObject : QueryObject
    {
        public int? MainCategoryId { get; set; }
    }

    public class SecondSubCategoryQueryObject : QueryObject
    {
        public int? FirstSubCategoryId { get; set; }
        public bool? IsActive { get; set; }
    }

    public class TemplateQueryObject : QueryObject
    {
        public string Name { get; set; }
        public LetterTemplateTypes? Type { get; set; }
    }
}