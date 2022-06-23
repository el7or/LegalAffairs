using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Dtos
{
    public class AttachmentTypeListItemDto
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedOnHigri { get; set; } = null;

        public string Name { get; set; }

        public string GroupName { get; set; }
    }

    public class AttachmentTypeDetailsDto
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }

        public GroupNames GroupName { get; set; }
    }

    public class AttachmentTypeDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? GroupName { get; set; }
    }
}
