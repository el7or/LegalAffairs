using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Entities
{
    public class AttachmentType
    {
        /// <summary>
        /// The primary key.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The creation datetime.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }

        public GroupNames GroupName { get; set; }

        //public ICollection<Attachment> Attachments { get; set; }
    }
}
