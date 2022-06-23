using System;

namespace Moe.La.Core.Entities
{
    public class CaseRuleAttachment
    {
        public Guid Id { get; set; }

        public int? CaseRuleId { get; set; }

        public CaseRule CaseRule { get; set; }

        public Attachment Attachment { get; set; }

    }
}
