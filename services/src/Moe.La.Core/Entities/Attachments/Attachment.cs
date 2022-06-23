using System;

namespace Moe.La.Core.Entities
{
    public class Attachment : BaseEntity<Guid>
    {
        /// <summary>
        /// File name which came from user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// File size
        /// </summary>
        public int? Size { get; set; }

        public bool IsDraft { get; set; } = true;

        public int? AttachmentTypeId { get; set; }

        public AttachmentType AttachmentType { get; set; }

        public Guid CreatedBy { get; set; }
    }
}
