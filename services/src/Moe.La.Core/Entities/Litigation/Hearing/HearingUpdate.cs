using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    public class HearingUpdate : BaseEntity<int>
    {
        public int HearingId { get; set; }

        public Hearing Hearing { get; set; }

        public string Text { get; set; }

        public DateTime UpdateDate { get; set; }

        public Guid CreatedBy { get; set; }

        public AppUser CreatedByUser { get; set; }

        public ICollection<HearingUpdateAttachment> Attachments { get; set; } = new List<HearingUpdateAttachment>();

    }
}