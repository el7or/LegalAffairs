using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Dtos
{
    public class NotificationListItemDto : BaseDto<int>
    {
        public string Text { get; set; }

        public KeyValuePairsDto<int> Type { get; set; }

        public string URL { get; set; }

        public bool IsRead { get; set; }

        public string CreatedOnHigri { get; set; }

        public string CreatedOnTime { get; set; }
    }

    //public class NotificationDetailsDto : BaseDto<int>
    //{
    //    public string Type { get; set; }

    //    public string Text { get; set; }

    //    public string URL { get; set; }

    //    public string User { get; set; }

    //    public bool IsRead { get; set; }
    //}

    public class NotificationDto
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string URL { get; set; }

        public NotificationTypes Type { get; set; } = NotificationTypes.Info;

        public ICollection<Guid> UserIds { get; set; }

        public bool SendSMSMessage { get; set; } = false;

        public bool SendEmailMessage { get; set; } = true;
    }
}
