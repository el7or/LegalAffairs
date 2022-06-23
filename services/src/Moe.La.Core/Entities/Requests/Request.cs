using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Moe.La.Core.Entities
{
    public class Request : BaseEntity<int>
    {
        /// <summary>
        /// نوع الطلب
        /// </summary>
        public RequestTypes RequestType { get; set; }

        /// <summary>
        /// حالة الطلب
        /// </summary>
        public RequestStatuses RequestStatus { get; set; }


        /// <summary>
        /// The sending type.
        /// </summary>
        public SendingTypes SendingType { get; set; }

        /// <summary>
        /// The request receiver user
        /// </summary>
        public Guid? ReceiverId { get; set; }

        public AppUser Receiver { get; set; }

        /// <summary>
        /// The request receiver general department id.
        /// </summary>
        public int? ReceiverBranchId { get; set; }

        /// <summary>
        /// The request receiver department id.
        /// </summary>
        public int? ReceiverDepartmentId { get; set; }

        /// <summary>
        /// The role to receive the request.
        /// </summary>
        public Guid? ReceiverRoleId { get; set; }

        /// <summary>
        /// The related request id.
        /// </summary>
        public int? RelatedRequestId { get; set; }

        public Request RelatedRequest { get; set; }

        /// <summary>
        /// سبب الطلب / ملاحظات
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// للتصدير
        /// </summary>
        public bool IsExportable { get; set; }
        /// <summary>
        /// The user's id who created it.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Navigation property to the user who created it.
        /// </summary>
        public AppUser CreatedByUser { get; set; }

        /// <summary>
        /// The user's id who updated it.
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Navigation property to the user who updated it.
        /// </summary>
        public AppUser UpdatedByUser { get; set; }

        /// <summary>
        /// The update datetime.
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        public ICollection<RequestTransaction> RequestTransactions { get; set; } = new Collection<RequestTransaction>();

        public ICollection<ConsultationSupportingDocumentRequest> ConsultationSupportingDocuments { get; set; } = new Collection<ConsultationSupportingDocumentRequest>();

        public RequestLetter Letter { get; set; }

    }
}
