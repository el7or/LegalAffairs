using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Dtos
{
    public class MoamalaListItemDto : BaseDto<int>
    {
        public string UnifiedNo { get; set; }

        public string MoamalaNumber { get; set; }

        public DateTime PassDate { get; set; }

        public string PassDateHigri { get; set; }

        public string PassTime { get; set; }

        public string CreatedOnHigri { get; set; }

        public string CreatedOnTime { get; set; }

        public KeyValuePairsDto<int> SenderDepartment { get; set; }

        public KeyValuePairsDto<int> ConfidentialDegree { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }

        public KeyValuePairsDto<int> Status { get; set; }

        public KeyValuePairsDto<int> WorkItemType { get; set; }

        public KeyValuePairsDto<int?> SubWorkItemType { get; set; }

        public bool IsRead { get; set; } = false;

        public bool IsManual { get; set; }

        public KeyValuePairsDto<int> Branch { get; set; }

        public KeyValuePairsDto<int> ReceiverDepartment { get; set; }

        public KeyValuePairsDto<Guid> AssignedTo { get; set; }

        public string ReferralNote { get; set; }

        public string AssigningNote { get; set; }

        public string ReturningReason { get; set; }

        public string CreatedByFullName { get; set; }

        public MoamalaSteps CurrentStep { get; set; }

        public MoamalaSteps? PreviousStep { get; set; }

        public List<int> Consultations { get; set; }

    }

    public class MoamalaDetailsDto : BaseDto<int>
    {
        public string UnifiedNo { get; set; }

        public string MoamalaNumber { get; set; }

        public string Subject { get; set; }

        public string CreatedOnHigri { get; set; }

        public string CreatedOnTime { get; set; }

        public string CreatedBy { get; set; }

        public DateTime PassDate { get; set; }

        public string PassDateHigri { get; set; }

        public string PassTime { get; set; }

        public KeyValuePairsDto<int> ConfidentialDegree { get; set; }

        public KeyValuePairsDto<int> PassType { get; set; }

        public KeyValuePairsDto<int> SenderDepartment { get; set; }

        public KeyValuePairsDto<int> Status { get; set; }

        public KeyValuePairsDto<int> WorkItemType { get; set; }

        public KeyValuePairsDto<int> SubWorkItemType { get; set; }

        public string Description { get; set; }

        public KeyValuePairsDto<int> Branch { get; set; }

        public KeyValuePairsDto<int> ReceiverDepartment { get; set; }

        public string ReferralNote { get; set; }

        public Guid? AssignedToId { get; set; }

        public string AssignedToFullName { get; set; }

        public string AssigningNote { get; set; }

        public MoamalaSteps CurrentStep { get; set; }

        public MoamalaSteps? PreviousStep { get; set; }

        public string ReturningReason { get; set; }

        public int? RelatedId { get; set; }

        public string ReleatedItemsTitle { get; set; }

        public List<KeyValuePairsDto<int>> ReleatedItems { get; set; }

        public List<AttachmentListItemDto> Attachments { get; set; }

        public List<MoamalaListItemDto> RelatedMoamalat { get; set; }

        public int? ConsultationId { get; set; }

        public ConsultationStatus ConsultationStatus { get; set; }

        public bool IsManual { get; set; }
    }

    public class MoamalaDto
    {
        public int Id { get; set; }

        public string UnifiedNo { get; set; }

        public string MoamalaNumber { get; set; }

        public int? SenderDepartmentId { get; set; }

        public ConfidentialDegrees? ConfidentialDegree { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }

        public PassTypes PassType { get; set; }

        public DateTime PassDate { get; set; }

        public MoamalaStatuses Status { get; set; }

        public int? WorkItemTypeId { get; set; }

        public int? SubWorkItemType { get; set; }

        public bool IsRead { get; set; }

        public bool IsManual { get; set; }

        public int? BranchId { get; set; }

        public int? ReceiverDepartmentId { get; set; }

        public Guid? AssignedToId { get; set; }

        public string ReferralNote { get; set; }

        public string AssigningNote { get; set; }

        public string ReturningReason { get; set; }

        public int? RelatedId { get; set; }

        public List<AttachmentDto> Attachments { get; set; } = new List<AttachmentDto>();
    }


    public class MoamalaChangeStatusDto
    {
        public int MoamalaId { get; set; }

        public MoamalaStatuses Status { get; set; }

        public Guid? AssignedToId { get; set; }

        public int? WorkItemTypeId { get; set; }

        public int? subWorkItemTypeId { get; set; }

        public int BranchId { get; set; }

        public int? DepartmentId { get; set; }

        public MoamalaSteps CurrentStep { get; set; }

        public MoamalaSteps? PreviousStep { get; set; }

        public string Note { get; set; }
    }

    public class MoamalaNotificationDto
    {
        public int MoamalaId { get; set; }
        public Guid UserId { get; set; }
        public bool IsRead { get; set; } = false;

    }

    public class MoamalaUpdateWorkItemType
    {
        public int Id { get; set; }

        public int? WorkItemTypeId { get; set; }
        public int? SubWorkItemTypeId { get; set; }
    }

    public class MoamalaUpdateRelatedId
    {
        public int Id { get; set; }

        public int? RelatedId { get; set; }
    }
}
