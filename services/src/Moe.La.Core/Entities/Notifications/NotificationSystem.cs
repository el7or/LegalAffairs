using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Entities
{
    public class NotificationSystem
    {
        public int NotificationId { get; set; }

        public Notification Notification { get; set; }

        public Guid UserId { get; set; }

        public AppUser User { get; set; }

        public string URL { get; set; }

        public NotificationTypes Type { get; set; }

        public bool IsRead { get; set; }

    }
}
