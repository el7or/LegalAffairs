using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Dtos
{
    public class RequestListItemDto
    {
        public int Id { get; set; }
        public KeyValuePairsDto<int> RequestType { get; set; }

        public KeyValuePairsDto<int> RequestStatus { get; set; }

        public bool IsExportable { get; set; }

        public KeyValuePairsDto<Guid> CreatedBy { get; set; }
        public KeyValuePairsDto<Guid> UpdatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedOnHigri { get; set; }

        public DateTime? LastTransactionDate { get; set; }
        public string LastTransactionDateHigri { get; set; } = null;

        public ICollection<RequestTransactionListDto> RequestTransactions { get; set; }

        public string Note { get; set; }

        public RequestLetterHistoryDto Letter { get; set; }


    }

    public class RequestDetailsDto : BaseDto<int>
    {
        public KeyValuePairsDto<int> RequestType { get; set; }

        public KeyValuePairsDto<int> RequestStatus { get; set; }

        public bool IsExportable { get; set; }

        public string CreatedByFullName { get; set; }

        public string CreatedOnHigri { get; set; } = null;

        public DateTime? LastTransactionDate { get; set; }

        public string LastTransactionDateHigri { get; set; } = null;

        public int? ReceiverGeneralManagementId { get; set; }
    }

    public class RequestDto
    {
        /// <summary>
        /// Request ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Request type.
        /// </summary>
        public RequestTypes RequestType { get; set; }

        /// <summary>
        /// Request status.
        /// </summary>
        public RequestStatuses RequestStatus { get; set; }

        /// <summary>
        /// Determinse is this request will be exported via the administrative communication department.
        /// </summary>
        public bool IsExportable { get; set; } = false;

        /// <summary>
        /// To whom this request will be sent. 
        /// </summary>
        public SendingTypes SendingType { get; set; }

        /// <summary>
        /// If sending type is user, then this will be the user ID who receive the request.
        /// </summary>
        public Guid? ReceiverId { get; set; }

        /// <summary>
        /// If sending type is department, then this will be the department's branch ID who receive the request.
        /// </summary>
        public int? ReceiverBranchId { get; set; }

        /// <summary>
        /// If sending type is department, then this will be the department ID who receive the request.
        /// </summary>
        public int? ReceiverDepartmentId { get; set; }

        /// <summary>
        /// If sending type is role, then this will be the role ID who receive the request.
        /// </summary>
        public Guid? ReceiverRoleId { get; set; }

        /// <summary>
        /// The request ID that this request related to.
        /// </summary>
        public int? RelatedRequestId { get; set; }

        /// <summary>
        /// The request note.
        /// </summary>
        public string Note { get; set; }

        public RequestLetterDetailsDto Letter { get; set; }
    }


    public class ExportRequestDto
    {
        public int requestId { get; set; }

        public RequestStatuses RequestStatus { get; set; }

        public string MoamalaNo { get; set; }

        public DateTime MoamalaDate { get; set; }
    }

    public class RequestForPrintDto
    {
        public int Id { get; set; }

        public KeyValuePairsDto<int> RequestType { get; set; }

        public KeyValuePairsDto<int> RequestStatus { get; set; }

        public KeyValuePairsDto<Guid> CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedOnHigri { get; set; }

        public DateTime? LastTransactionDate { get; set; }

        public string LastTransactionDateHigri { get; set; } = null;

        public ICollection<RequestTransactionListDto> RequestTransactions { get; set; }

        public string Note { get; set; }

    }

}
