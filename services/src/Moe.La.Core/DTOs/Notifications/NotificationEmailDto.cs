using System;

namespace Moe.La.Core.Dtos
{
    public class NotificationEmailListItemDto : BaseDto<int>
    {
        public Guid UserId { get; set; }

        public int NotificationId { get; set; }

        public string Text { get; set; }

        public bool IsSent { get; set; }
        public string Email { get; set; }
    }

    public class NotificationEmailDetailsDto : BaseDto<int>
    {
        public int? NotificationId { get; set; }

        public string Text { get; set; }

        public bool IsSent { get; set; }
    }

    public class NotificationEmailDto
    {
        public int NotificationId { get; set; }

        public Guid UserId { get; set; }

        public bool? IsSent { get; set; }
    }
}
