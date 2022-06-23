using AutoMapper;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Infrastructure.DbContexts;
using System;
using System.Threading.Tasks;

namespace Moe.La.Infrastructure.Repositories
{
    public class NotificationRepository : RepositoryBase, INotificationRepository
    {
        public NotificationRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider)
            : base(context, mapperConfig, userProvider)
        {
        }

        public async Task AddAsync(NotificationDto notificationDto)
        {
            var entityToAdd = mapper.Map<Notification>(notificationDto);
            entityToAdd.CreatedBy = CurrentUser.UserId;
            entityToAdd.CreatedOn = DateTime.Now;

            if (!string.IsNullOrEmpty(notificationDto.URL))
                foreach (var userId in notificationDto.UserIds)
                {
                    var notificationSystem = new NotificationSystem()
                    {
                        UserId = userId,
                        URL = notificationDto.URL,
                        Type = notificationDto.Type
                    };

                    entityToAdd.Systems.Add(notificationSystem);
                }

            if (notificationDto.SendSMSMessage)
                foreach (var userId in notificationDto.UserIds)
                {
                    var notificationSMS = new NotificationSMS()
                    {
                        UserId = userId,
                    };

                    entityToAdd.SMSs.Add(notificationSMS);
                }

            if (notificationDto.SendEmailMessage)
                foreach (var userId in notificationDto.UserIds)
                {
                    var notificationEmail = new NotificationEmail()
                    {
                        UserId = userId,
                    };

                    entityToAdd.Emails.Add(notificationEmail);
                }

            await db.Notifications.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            mapper.Map(entityToAdd, notificationDto);
        }
    }
}
