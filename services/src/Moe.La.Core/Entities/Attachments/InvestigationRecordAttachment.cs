using System;

namespace Moe.La.Core.Entities
{
    public class InvestigationRecordAttachment
    {
        public Guid Id { get; set; }

        public int RecordId { get; set; }

        public InvestigationRecord Record { get; set; }

        public Attachment Attachment { get; set; }

    }


}
