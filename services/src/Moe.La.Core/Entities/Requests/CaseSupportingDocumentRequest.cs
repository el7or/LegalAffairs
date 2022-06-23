using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    public class CaseSupportingDocumentRequest
    {
        public int Id { get; set; }

        public Request Request { get; set; }

        public int? ParentId { get; set; }

        public int HearingId { get; set; }

        public Hearing Hearing { get; set; }

        /// <summary>
        /// The related case id.
        /// </summary>
        public int? CaseId { get; set; }

        /// <summary>
        /// The related case.
        /// </summary>
        public Case Case { get; set; }

        public CaseSupportingDocumentRequest Parent { get; set; }

        public string ReplyNote { get; set; }

        public DateTime? ReplyDate { get; set; }

        public int? ConsigneeDepartmentId { get; set; }
        public MinistryDepartment ConsigneeDepartment { get; set; }

        public bool IsDeleted { get; set; }
        /// <summary>
        /// رقم المعاملة في الاتصالات الادارية
        /// </summary>
        public string TransactionNumberInAdministrativeCommunications { get; set; }
        /// <summary>
        /// تاريخ المعاملة في الاتصالات الادارية
        /// </summary>
        public DateTime? TransactionDateInAdministrativeCommunications { get; set; }

        public ICollection<CaseSupportingDocumentRequestItem> Documents { get; set; }
        public ICollection<CaseSupportingDocumentRequestHistory> History { get; set; }
    }
}
