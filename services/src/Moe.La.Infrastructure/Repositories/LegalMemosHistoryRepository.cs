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
    public class LegalMemosHistoryRepository : RepositoryBase, ILegalMemosHistoryRepository
    {
        public LegalMemosHistoryRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider)
            : base(context, mapperConfig, userProvider)
        {
        }

        public async Task<QueryResultDto<LegalMemosHistoryListItemDto>> GetAllAsync(LegalMemoQueryObject queryObject)
        {
            var result = new QueryResult<LegalMemoHistory>();
            var query = db.LegalMemosHistory
                 .Include(a => a.UpdatedByUser)
                .OrderBy(n => n.Name)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(queryObject.Name))
            {
                query = query.Where(m => m.Name.Contains(queryObject.Name));
            }

            if (!string.IsNullOrEmpty(queryObject.Status))
            {
                var status = queryObject.Status.Split(new char[] { ',' });
                query = query.Where(s => status.Contains(((int)s.Status).ToString()));
            }

            var columnsMap = new Dictionary<string, Expression<Func<LegalMemoHistory, object>>>()
            {
                ["name"] = v => v.Name,
                ["status"] = v => v.Status,
                ["updatedOn"] = v => v.UpdatedOn,
                ["updatedByUser"] = v => v.UpdatedByUser.FirstName
            };

            query = query.ApplySorting(queryObject, columnsMap);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObject);

            result.Items = await query.ToListAsync();

            return mapper.Map<QueryResult<LegalMemoHistory>, QueryResultDto<LegalMemosHistoryListItemDto>>(result);
        }

        public async Task AddAsync(LegalMemosHistoryDto legalMemoDto)
        {
            var entityToAdd = mapper.Map<LegalMemoHistory>(legalMemoDto);

            await db.LegalMemosHistory.AddAsync(entityToAdd);
            await db.SaveChangesAsync();
        }
    }
}
