using System;

namespace Moe.La.Core.Entities
{
    public class CaseAttachment
    {
        public Guid Id { get; set; }

        public int CaseId { get; set; }

        public Case Case { get; set; }

        public Attachment Attachment { get; set; }
    }
}
