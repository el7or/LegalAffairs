using System;

namespace Moe.La.Core.Entities
{
    public class HearingUpdateAttachment
    {
        public Guid Id { get; set; }

        public int? HearingUpdateId { get; set; }

        public HearingUpdate HearingUpdate { get; set; }

        public Attachment Attachment { get; set; }
    }
}
