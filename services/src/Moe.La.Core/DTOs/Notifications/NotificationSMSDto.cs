using System;

namespace Moe.La.Core.Dtos
{
    public class NotificationSMSListItemDto : BaseDto<int>
    {
        public int NotificationId { get; set; }

        public string Text { get; set; }

        public bool IsSent { get; set; }

        public string PhoneNumber { get; set; }

        public Guid UserId { get; set; }
    }

    public class NotificationSMSDetailsDto : BaseDto<int>
    {
        public int? NotificationId { get; set; }

        public string Text { get; set; }

        public bool IsSent { get; set; }
    }

    public class NotificationSMSDto
    {
        public int NotificationId { get; set; }

        public Guid UserId { get; set; }

        public bool? IsSent { get; set; }

    }
}
