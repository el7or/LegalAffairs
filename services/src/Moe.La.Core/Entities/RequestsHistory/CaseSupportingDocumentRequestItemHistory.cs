namespace Moe.La.Core.Entities
{
    public class CaseSupportingDocumentRequestItemHistory
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

        public string Name { get; set; }

        public int CaseSupportingDocumentRequestId { get; set; }

        public bool IsDeleted { get; set; }

        public CaseSupportingDocumentRequestHistory CaseSupportingDocumentRequest { get; set; }
    }
}
