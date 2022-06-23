using System;

namespace Moe.La.Core.Entities
{
    public class ConsultationSupportingDocumentRequest
    {
        public int ConsultationId { get; set; }

        public Consultation Consultation { get; set; }

        public int RequestId { get; set; }

        public Request Request { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }

    }
}
