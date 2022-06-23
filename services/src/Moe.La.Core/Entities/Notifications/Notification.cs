using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    public class Notification : BaseEntity<int>
    {
        public string Text { get; set; }

        /// <summary>
        /// The user's id who created it.
        /// </summary>
        public Guid CreatedBy { get; set; }

        public ICollection<NotificationSystem> Systems { get; set; } = new List<NotificationSystem>();
        public ICollection<NotificationEmail> Emails { get; set; } = new List<NotificationEmail>();
        public ICollection<NotificationSMS> SMSs { get; set; } = new List<NotificationSMS>();

    }
}