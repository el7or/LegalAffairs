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
    public class NotificationSMSRepository : RepositoryBase, INotificationSMSRepository
    {
        public NotificationSMSRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider)
            : base(context, mapperConfig, userProvider)
        {
        }

        public async Task<QueryResultDto<NotificationSMSListItemDto>> GetAllAsync(NotificationSMSQueryObject queryObject)
        {
            var result = new QueryResult<NotificationSMS>();
            var query = db.NotificationSMSs
                .Include(s => s.Notification)
                .Include(s => s.User)
                 .AsQueryable();

            if (queryObject.IsSent.HasValue)
                query = query.Where(n => n.IsSent == queryObject.IsSent);

            if (queryObject.IsForCurrentUser == true)
                query = query.Where(n => n.UserId == CurrentUser.UserId);

            var columnsMap = new Dictionary<string, Expression<Func<NotificationSMS, object>>>()
            {
                ["text"] = n => n.Notification.Text,
                ["createdOn"] = n => n.Notification.CreatedOn
            };

            query = query.ApplySorting(queryObject, columnsMap);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObject);

            result.Items = await query.ToListAsync();

            return mapper.Map<QueryResult<NotificationSMS>, QueryResultDto<NotificationSMSListItemDto>>(result);
        }

        public async Task<NotificationSMSDetailsDto> GetAsync(int id)
        {
            var notification = await db.NotificationSMSs
                 .Include(n => n.Notification)
                .FirstOrDefaultAsync(n => n.NotificationId == id && n.UserId == CurrentUser.UserId);

            return mapper.Map<NotificationSMSDetailsDto>(notification);
        }

        public async Task SentSuccessufullyAsync(NotificationSMSListItemDto sms)
        {
            var entityToUpdate = await db.NotificationSMSs
                .Where(n => n.NotificationId == sms.NotificationId && n.UserId == sms.UserId).FirstOrDefaultAsync();

            entityToUpdate.IsSent = true;
            entityToUpdate.AttemptsCount = ++entityToUpdate.AttemptsCount;
            await db.SaveChangesAsync();
        }
    }
}
