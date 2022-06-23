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
    public class NotificationSystemRepository : RepositoryBase, INotificationSystemRepository
    {
        public NotificationSystemRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider)
            : base(context, mapperConfig, userProvider)
        {
        }

        public async Task<QueryResultDto<NotificationSystemListItemDto>> GetAllAsync(NotificationSystemQueryObject queryObject)
        {
            var result = new QueryResult<NotificationSystem>();
            var query = db.NotificationSystems
                .Include(s => s.Notification)
                .Where(u => u.UserId == CurrentUser.UserId)
                .AsQueryable();

            if (queryObject.IsRead.HasValue)
                query = query.Where(n => n.IsRead == queryObject.IsRead);

            if (queryObject.IsForCurrentUser == true)
                query = query.Where(n => n.UserId == CurrentUser.UserId);

            var columnsMap = new Dictionary<string, Expression<Func<NotificationSystem, object>>>()
            {
                ["text"] = n => n.Notification.Text,
                ["createdOn"] = n => n.Notification.CreatedOn
            };

            query = query.ApplySorting(queryObject, columnsMap);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObject);

            result.Items = await query.ToListAsync();

            return mapper.Map<QueryResult<NotificationSystem>, QueryResultDto<NotificationSystemListItemDto>>(result);
        }

        public async Task<NotificationSystemDetailsDto> GetAsync(int id)
        {
            var notification = await db.NotificationSystems
                 .Include(n => n.Notification)
                .FirstOrDefaultAsync(n => n.NotificationId == id && n.UserId == CurrentUser.UserId);

            return mapper.Map<NotificationSystemDetailsDto>(notification);
        }

        public async Task ReadAsync(int notificationId)
        {
            var entityToUpdate = await db.NotificationSystems
                .Where(n => n.NotificationId == notificationId && n.UserId == CurrentUser.UserId).FirstOrDefaultAsync();

            entityToUpdate.IsRead = true;
            await db.SaveChangesAsync();
        }
    }
}
