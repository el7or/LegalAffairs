using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Entities
{
    public class NotificationSystemListItemDto : BaseDto<int>
    {
        public string Text { get; set; }

        public string Type { get; set; }

        public string URL { get; set; }

        public bool IsRead { get; set; }

        public string CreatedOnHigri { get; set; }

        public string CreationTime { get; set; } = null;
    }

    public class NotificationSystemDetailsDto : BaseDto<int>
    {
        public string Text { get; set; }
        public string Type { get; set; }
        public string URL { get; set; }
        public bool IsRead { get; set; }
    }

    public class NotificationSystemDto
    {
        public int NotificationId { get; set; }
        public Guid UserId { get; set; }
        public NotificationTypes Type { get; set; }
        public string URL { get; set; }
        public bool? IsRead { get; set; }
    }

}
