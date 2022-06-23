using Microsoft.AspNetCore.Http;
using System;

namespace Moe.La.Core.Dtos
{
    public class AttachmentListItemDto : BaseDto<Guid>
    {
        public string Name { get; set; }

        public int Size { get; set; }

        public string AttachmentType { get; set; }

        public int? AttachmentTypeId { get; set; }

        public bool IsDraft { get; set; }

        public string CreatedOnHigri { get; set; } = null;
    }


    public class AttachmentDto
    {
        public Guid? Id { get; set; }

        public IFormFile File { get; set; }

        public int? AttachmentTypeId { get; set; }

        public string Name { get; set; }

        //public bool IsDraft { get; set; }

        public bool IsDeleted { get; set; }

    }
}
