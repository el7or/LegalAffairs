using System;
using System.Collections.Generic;

namespace Moe.La.Core.Dtos
{
    public class HearingUpdateListItemDto
    {
        public int Id { get; set; }

        public int HearingId { get; set; }

        public HearingDto Hearing { get; set; }

        public string Text { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedOnHigri { get; set; }

        public string CreationTime { get; set; } = null;

        public DateTime UpdateDate { get; set; }

        public string UpdateDateHijri { get; set; }

        public string UpdateTime { get; set; } = null;

        public KeyValuePairsDto<Guid> Attachment { get; set; }

        public KeyValuePairsDto<Guid> CreatedBy { get; set; }

        public List<AttachmentListItemDto> Attachments { get; set; }


    }

    public class HearingUpdateDetailsDto
    {
        public int Id { get; set; }

        public int HearingId { get; set; }

        public HearingDto Hearing { get; set; }

        public string Text { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedOnHigri { get; set; }

        public string CreationTime { get; set; } = null;

        public DateTime UpdateDate { get; set; }

        public string UpdateDateHijri { get; set; }

        public string UpdateTime { get; set; } = null;

        public KeyValuePairsDto<Guid> CreatedBy { get; set; }

        public List<AttachmentListItemDto> Attachments { get; set; }


    }

    public class HearingUpdateDto
    {
        public int Id { get; set; }

        public int HearingId { get; set; }

        public string Text { get; set; }

        public List<AttachmentDto> Attachments { get; set; } = new List<AttachmentDto>();
    }



}




