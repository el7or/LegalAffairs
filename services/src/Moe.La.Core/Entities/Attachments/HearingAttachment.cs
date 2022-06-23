using System;

namespace Moe.La.Core.Entities
{
    public class HearingAttachment
    {
        public Guid Id { get; set; }

        public int? HearingId { get; set; }

        public Hearing Hearing { get; set; }

        public Attachment Attachment { get; set; }
    }
}
