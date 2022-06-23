using Moe.La.Core.Dtos.Consultations;
using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Dtos
{
    public class ConsultationListItemDto
    {
        public int MoamlaId { get; set; }

        public int ConsultationId { get; set; }

        public string MoamalaNumber { get; set; }

        public DateTime MoamalaDate { get; set; }

        public string MoamalaDateOnHijri { get; set; }

        public KeyValuePairsDto<int> WorkItemType { get; set; }

        public string Subject { get; set; }

        public KeyValuePairsDto<Guid> User { get; set; }

        public KeyValuePairsDto<int> Department { get; set; }

        public KeyValuePairsDto<int> Status { get; set; }
    }

    public class ConsultationDetailsDto
    {
        public int Id { get; set; }

        public int MoamalaId { get; set; }

        public string Subject { get; set; }

        public string ConsultationNumber { get; set; }

        public string LegalAnalysis { get; set; }

        public string ImportantElements { get; set; }

        public bool? IsWithNote { get; set; }

        public KeyValuePairsDto<int> Status { get; set; }

        public string DepartmentVision { get; set; }

        public KeyValuePairsDto<int> Branch { get; set; }

        public KeyValuePairsDto<int> Department { get; set; }

        public DateTime? ConsultationDate { get; set; }

        public string ConsultationDateHijri { get; set; }

        public KeyValuePairsDto<int> WorkItemType { get; set; }

        public KeyValuePairsDto<int> SubWorkItemType { get; set; }

        public ICollection<ConsultationMeritsDto> ConsultationMerits { get; set; } = new List<ConsultationMeritsDto>();

        public ICollection<ConsultationGroundsDto> ConsultationGrounds { get; set; } = new List<ConsultationGroundsDto>();

        public ICollection<ConsultationVisualDto> ConsultationVisuals { get; set; } = new List<ConsultationVisualDto>();

        public ICollection<ConsultationTransactionListDto> ConsultationTransactions { get; set; } = new List<ConsultationTransactionListDto>();

        public ICollection<ConsultationSupportingDocumentRequestDto> ConsultationSupportingDocuments { get; set; } = new List<ConsultationSupportingDocumentRequestDto>();

    }

    public class ConsultationDto
    {
        public int? Id { get; set; }

        public string Subject { get; set; }

        public string ConsultationNumber { get; set; }

        public string LegalAnalysis { get; set; }

        public ConsultationStatus Status { get; set; }

        public string DepartmentVision { get; set; }

        public int BranchId { get; set; }

        public int? DepartmentId { get; set; }

        public int MoamalaId { get; set; }

        public int? WorkItemTypeId { get; set; }

        public int? SubWorkItemTypeId { get; set; }

        public DateTime? ConsultationDate { get; set; }

        public string ImportantElements { get; set; }

        public bool? IsWithNote { get; set; }

        public ICollection<ConsultationMeritsDto> ConsultationMerits { get; set; } = new List<ConsultationMeritsDto>();

        public ICollection<ConsultationGroundsDto> ConsultationGrounds { get; set; } = new List<ConsultationGroundsDto>();

        public ICollection<ConsultationVisualDto> ConsultationVisuals { get; set; } = new List<ConsultationVisualDto>();

    }
    public class ConsultationReviewDto
    {
        public int Id { get; set; }

        public string DepartmentVision { get; set; }

        public ConsultationDto Consultation { get; set; }

        public ConsultationStatus ConsultationStatus { get; set; }

        public ReturnedConsultationTypes? ReturnedType { get; set; }
    }
}
