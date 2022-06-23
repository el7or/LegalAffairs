using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.UnitTests.Builders
{
    public class NotificationBuilder
    {
        private NotificationDto _notificationDto = new NotificationDto();

        public NotificationBuilder Id(int id)
        {
            _notificationDto.Id = id;
            return this;
        }

        public NotificationBuilder Text(string text)
        {
            _notificationDto.Text = text;
            return this;
        }

        public NotificationBuilder URL(string url)
        {
            _notificationDto.URL = url;
            return this;
        }

        public NotificationBuilder Type(NotificationTypes type)
        {
            _notificationDto.Type = type;
            return this;
        }

        public NotificationBuilder UserIds(ICollection<Guid> userIds)
        {
            _notificationDto.UserIds = userIds;
            return this;
        }

        public NotificationBuilder SendSMSMessage(bool sendSMSMessage = false)
        {
            _notificationDto.SendSMSMessage = sendSMSMessage;
            return this;
        }

        public NotificationBuilder SendEmailMessage(bool sendEmailMessage = true)
        {
            _notificationDto.SendEmailMessage = sendEmailMessage;
            return this;
        }

        public NotificationBuilder WithDefaultValues()
        {
            _notificationDto = new NotificationDto
            {
                Id = 0,
                Text = "تم إضافة مستخدم جديد",
                URL = "/users/view/",
                Type = NotificationTypes.Info,
                UserIds = new List<Guid> { TestUsers.AdminId },
                SendEmailMessage = true,
                SendSMSMessage = false
            };

            return this;
        }

        public NotificationDto Build() => _notificationDto;
    }
}
