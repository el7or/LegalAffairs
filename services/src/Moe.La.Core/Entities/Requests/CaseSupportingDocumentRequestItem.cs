namespace Moe.La.Core.Entities
{
    public class CaseSupportingDocumentRequestItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CaseSupportingDocumentRequestId { get; set; }

        public bool IsDeleted { get; set; }

        public CaseSupportingDocumentRequest CaseSupportingDocumentRequest { get; set; }
    }
}
