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
    public class RequestTransactionRepository : RepositoryBase, IRequestTransactionRepository
    {
        public RequestTransactionRepository(IMapper mapper, LaDbContext context, IUserProvider userProvider)
            : base(context, mapper, userProvider)
        {

        }

        public async Task AddAsync(RequestTransactionDto transactionDto)
        {
            var entityToAdd = mapper.Map<RequestTransaction>(transactionDto);

            entityToAdd.CreatedOn = DateTime.Now;
            entityToAdd.CreatedBy = CurrentUser.UserId;

            await db.RequestTransactions.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            mapper.Map<RequestTransactionDto>(entityToAdd);
        }

        public async Task<QueryResultDto<RequestTransactionListDto>> GetAllAsync(TransactionQueryObject queryObject)
        {
            var result = new QueryResult<RequestTransaction>();
            var query = db.RequestTransactions
                .Where(t => t.RequestId == queryObject.RequestId)
                //.Include(m => m.Case)                
                .AsQueryable();

            var columnsMap = new Dictionary<string, Expression<Func<RequestTransaction, object>>>()
            {
                ["requestType"] = v => v.TransactionType,
                ["requestStatus"] = v => v.RequestStatus,
            };

            query = query.ApplySorting(queryObject, columnsMap);
            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObject);
            result.Items = await query.AsNoTracking().ToListAsync();
            return mapper.Map<QueryResult<RequestTransaction>, QueryResultDto<RequestTransactionListDto>>(result);
        }

    }
}
