using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Infrastructure.DbContexts;
using Moe.La.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Moe.La.Infrastructure.Repositories
{
    public class NotificationEmailRepository : RepositoryBase, INotificationEmailRepository
    {
        public NotificationEmailRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider)
            : base(context, mapperConfig, userProvider)
        {
        }

        public async Task<QueryResultDto<NotificationEmailListItemDto>> GetAllAsync(NotificationEmailQueryObject queryObject)
        {
            var result = new QueryResult<NotificationEmail>();
            var query = db.NotificationEmails
                .Include(s => s.Notification)
                .Include(s => s.User)
                 .AsQueryable();

            if (queryObject.IsSent == true)
                query = query.Where(n => n.IsSent == queryObject.IsSent);

            if (queryObject.IsForCurrentUser == true)
                query = query.Where(n => n.UserId == CurrentUser.UserId);

            var columnsMap = new Dictionary<string, Expression<Func<NotificationEmail, object>>>()
            {
                ["text"] = n => n.Notification.Text,
                ["createdOn"] = n => n.Notification.CreatedOn
            };

            query = query.ApplySorting(queryObject, columnsMap);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObject);

            result.Items = await query.ToListAsync();

            return mapper.Map<QueryResult<NotificationEmail>, QueryResultDto<NotificationEmailListItemDto>>(result);
        }

        public async Task SentSuccessufullyAsync(NotificationEmailListItemDto notificationEmail)
        {
            var entityToUpdate = await db.NotificationEmails
                .Where(n => n.NotificationId == notificationEmail.NotificationId && n.UserId == notificationEmail.UserId).FirstOrDefaultAsync();

            entityToUpdate.IsSent = true;
            entityToUpdate.AttemptsCount = ++entityToUpdate.AttemptsCount;
            await db.SaveChangesAsync();
        }

    }
}
