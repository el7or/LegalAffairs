using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    public class CaseSupportingDocumentRequestHistory
    {
        public int Id { get; set; }

        /// <summary>
        /// The related case id.
        /// </summary>
        public int? CaseId { get; set; }

        /// <summary>
        /// The related case.
        /// </summary>
        public Case Case { get; set; }
        /// <summary>
        /// The CaseSupportingDocumentRequest related.
        /// </summary>
        public int CaseSupportingDocumentRequestId { get; set; }

        /// <summary>
        /// A navigation property to CaseSupportingDocumentRequest.
        /// </summary>
        public CaseSupportingDocumentRequest CaseSupportingDocumentRequest { get; set; }

        public RequestHistory Request { get; set; }

        public int? ParentId { get; set; }

        public int HearingId { get; set; }

        public Hearing Hearing { get; set; }

        public CaseSupportingDocumentRequest Parent { get; set; }

        public string ReplyNote { get; set; }

        public DateTime? ReplyDate { get; set; }

        public int ConsigneeDepartmentId { get; set; }
        public MinistryDepartment ConsigneeDepartment { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<CaseSupportingDocumentRequestItemHistory> Documents { get; set; }
    }
}
