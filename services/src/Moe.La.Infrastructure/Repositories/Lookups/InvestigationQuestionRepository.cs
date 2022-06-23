using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
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
    public class InvestigationQuestionRepository : RepositoryBase, IInvestigationQuestionRepository
    {
        private readonly IDistributedCache _cache;
        private const string CacheKey = "Lookup_Investigation_Questions";

        public InvestigationQuestionRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider, IDistributedCache cache)
            : base(context, mapperConfig, userProvider)
        {
            _cache = cache;
        }

        public async Task<QueryResultDto<InvestigationQuestionListItemDto>> GetAllAsync(InvestigationQuestionQueryObject queryObject)
        {
            QueryResult<InvestigationQuestion> result = new();
            IQueryable<InvestigationQuestion> query = null;

            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<InvestigationQuestion>>(CacheKey) ?? new List<InvestigationQuestion>();

            if (listValueInCache.Any())
            {
                // Set query data from cache 
                query = listValueInCache.OrderBy(n => n.Question).AsQueryable();
            }
            else
            {
                var entityDbSet = db.InvestigationQuestions;

                // Set query data from database
                query = entityDbSet.OrderBy(n => n.Question).AsQueryable();

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            if (!string.IsNullOrEmpty(queryObject.Question))
            {
                query.Where(q => q.Question == queryObject.Question);
            }

            var columnsMap = new Dictionary<string, Expression<Func<InvestigationQuestion, object>>>()
            {
                ["question"] = v => v.Question,
                ["status"] = v => v.Status,
            };

            query = query.ApplySorting(queryObject, columnsMap);
            result.TotalItems = query.Count();
            query = query.ApplyPaging(queryObject);
            result.Items = query.ToList();

            return mapper.Map<QueryResult<InvestigationQuestion>, QueryResultDto<InvestigationQuestionListItemDto>>(result);
        }

        public async Task<InvestigationQuestionListItemDto> GetAsync(int id)
        {
            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<InvestigationQuestion>>(CacheKey) ?? new List<InvestigationQuestion>();

            InvestigationQuestion investigationQuestion = listValueInCache.FirstOrDefault(m => m.Id == id);

            if (investigationQuestion is null)
            {
                var entityDbSet = db.InvestigationQuestions;

                // Get data from database
                investigationQuestion = await entityDbSet.FirstOrDefaultAsync(m => m.Id == id);

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            return mapper.Map<InvestigationQuestion, InvestigationQuestionListItemDto>(investigationQuestion);
        }

        public async Task<InvestigationQuestionDto> AddAsync(InvestigationQuestionDto investigationQuestionDto)
        {
            var entityDbSet = db.InvestigationQuestions;

            var entityToAdd = mapper.Map<InvestigationQuestion>(investigationQuestionDto);
            entityToAdd.Id = (await entityDbSet.IgnoreQueryFilters().MaxAsync(c => (int?)c.Id) ?? 0) + 1;
            entityToAdd.CreatedOn = DateTime.Now;
            entityToAdd.Status = InvestigationQuestionStatuses.Undefined;
            await entityDbSet.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());

            return mapper.Map(entityToAdd, investigationQuestionDto);
        }

        public async Task EditAsync(InvestigationQuestionDto investigationQuestionDto)
        {
            var entityDbSet = db.InvestigationQuestions;

            var entityToUpdate = await entityDbSet.FindAsync(investigationQuestionDto.Id);
            mapper.Map(investigationQuestionDto, entityToUpdate);
            entityToUpdate.Question = investigationQuestionDto.Question;
            entityToUpdate.Status = investigationQuestionDto.Status;
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());

            mapper.Map(entityToUpdate, investigationQuestionDto);
        }

        public async Task<InvestigationQuestionDto> ChangeQuestionStatusAsync(InvestigationQuestionChangeStatusDto questionChangeStatusDto)
        {
            var entityToUpdate = await db.InvestigationQuestions.FirstOrDefaultAsync(c => c.Id == questionChangeStatusDto.Id);
            entityToUpdate.Status = questionChangeStatusDto.Status;
            await db.SaveChangesAsync();
            return mapper.Map<InvestigationQuestion, InvestigationQuestionDto>(entityToUpdate);
        }

        public async Task RemoveAsync(int id)
        {
            var entityDbSet = db.InvestigationQuestions;

            var entityToDelete = await entityDbSet.FindAsync(id);
            entityDbSet.Remove(entityToDelete);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
        }

        public async Task<bool> IsNameExistsAsync(string question)
        {
            return await db.InvestigationQuestions.AnyAsync(s => s.Question == question);
        }
    }
}
