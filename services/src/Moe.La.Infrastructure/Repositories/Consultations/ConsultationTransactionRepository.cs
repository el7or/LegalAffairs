using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moe.La.Core.Dtos;
using Moe.La.Core.Dtos.Consultations;
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
    public class ConsultationTransactionRepository : RepositoryBase, IConsultationTransactionRepository
    {
        public ConsultationTransactionRepository(IMapper mapper, LaDbContext context, IUserProvider userProvider)
            : base(context, mapper, userProvider)
        {

        }

        public async Task AddAsync(ConsultationTransactionDto transactionDto)
        {
            var entityToAdd = mapper.Map<ConsultationTransaction>(transactionDto);

            entityToAdd.CreatedOn = DateTime.Now;
            entityToAdd.CreatedBy = CurrentUser.UserId;

            await db.ConsultationTransactions.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            mapper.Map<ConsultationTransactionDto>(entityToAdd);
        }

        public async Task<QueryResultDto<ConsultationTransactionListDto>> GetAllAsync(ConsultationTransactionQueryObject queryObject)
        {
            var result = new QueryResult<ConsultationTransaction>();
            var query = db.ConsultationTransactions
                .Where(t => t.ConsultationId == queryObject.ConsultationId)
                .AsQueryable();

            var columnsMap = new Dictionary<string, Expression<Func<ConsultationTransaction, object>>>()
            {
                ["ConsultationType"] = v => v.TransactionType,
            };

            query = query.ApplySorting(queryObject, columnsMap);
            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObject);
            result.Items = await query.AsNoTracking().ToListAsync();
            return mapper.Map<QueryResult<ConsultationTransaction>, QueryResultDto<ConsultationTransactionListDto>>(result);
        }

    }
}
