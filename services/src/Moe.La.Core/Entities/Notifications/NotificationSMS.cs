using System;

namespace Moe.La.Core.Entities
{
    public class NotificationSMS
    {
        public int NotificationId { get; set; }

        public Notification Notification { get; set; }

        public Guid UserId { get; set; }

        public AppUser User { get; set; }

        public bool IsSent { get; set; }

        public int AttemptsCount { get; set; }
    }
}
