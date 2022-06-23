using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
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

    public class JobTitleRepository : RepositoryBase, IJobTitleRepository
    {
        private readonly IDistributedCache _cache;
        private const string CacheKey = "Lookup_Job_Titles";

        public JobTitleRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider, IDistributedCache cache)
            : base(context, mapperConfig, userProvider)
        {
            _cache = cache;
        }

        public async Task<QueryResultDto<JobTitleListItemDto>> GetAllAsync(QueryObject queryObject)
        {
            QueryResult<JobTitle> result = new();
            IQueryable<JobTitle> query = null;

            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<JobTitle>>(CacheKey) ?? new List<JobTitle>();

            if (listValueInCache.Any())
            {
                // Set query data from cache 
                query = listValueInCache.OrderBy(n => n.Name).AsQueryable();
            }
            else
            {
                var entityDbSet = db.JobTitles;

                // Set query data from database
                query = entityDbSet.OrderBy(n => n.Name).AsQueryable();

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            var columnsMap = new Dictionary<string, Expression<Func<JobTitle, object>>>()
            {
                ["name"] = v => v.Name
            };
            query = query.ApplySorting(queryObject, columnsMap);
            result.TotalItems = query.Count();
            query = query.ApplyPaging(queryObject);
            result.Items = query.ToList();

            return mapper.Map<QueryResult<JobTitle>, QueryResultDto<JobTitleListItemDto>>(result);
        }

        public async Task<JobTitleListItemDto> GetAsync(int id)
        {
            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<JobTitle>>(CacheKey) ?? new List<JobTitle>();

            JobTitle jobTitle = listValueInCache.FirstOrDefault(m => m.Id == id);

            if (jobTitle is null)
            {
                var entityDbSet = db.JobTitles;

                // Get data from database
                jobTitle = await entityDbSet.FirstOrDefaultAsync(m => m.Id == id);

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            return mapper.Map<JobTitleListItemDto>(jobTitle);
        }

        public async Task AddAsync(JobTitleDto jobTitleDto)
        {
            var entityDbSet = db.JobTitles;

            var entityToAdd = mapper.Map<JobTitle>(jobTitleDto);
            entityToAdd.Id = (await entityDbSet.IgnoreQueryFilters().MaxAsync(c => (int?)c.Id) ?? 0) + 1;
            entityToAdd.CreatedOn = DateTime.Now;
            await entityDbSet.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());

            mapper.Map(entityToAdd, jobTitleDto);
        }

        public async Task EditAsync(JobTitleDto jobTitleDto)
        {
            var entityDbSet = db.JobTitles;

            var entityToUpdate = await entityDbSet.FindAsync(jobTitleDto.Id);
            mapper.Map(jobTitleDto, entityToUpdate);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());

            mapper.Map(entityToUpdate, jobTitleDto);
        }

        public async Task RemoveAsync(int id)
        {
            var entityDbSet = db.JobTitles;

            var entityToDelete = await entityDbSet.FindAsync(id);
            entityDbSet.Remove(entityToDelete);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
        }

        public async Task<bool> IsNameExistsAsync(string name)
        {
            return await db.JobTitles.AnyAsync(c => c.Name == name);
        }
    }
}
