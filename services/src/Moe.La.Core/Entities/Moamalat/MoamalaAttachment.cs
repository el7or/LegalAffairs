using System;

namespace Moe.La.Core.Entities
{
    public class MoamalaAttachment
    {
        public Guid Id { get; set; }

        public int? MoamalaId { get; set; }

        public Moamala Moamala { get; set; }

        public Attachment Attachment { get; set; }


    }
}
