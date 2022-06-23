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
    public class SubWorkItemTypeRepository : RepositoryBase, ISubWorkItemTypeRepository
    {
        private readonly IDistributedCache _cache;
        private const string CacheKey = "Lookup_Sub_Work_Item_Types";

        public SubWorkItemTypeRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider, IDistributedCache cache)
            : base(context, mapperConfig, userProvider)
        {
            _cache = cache;
        }

        public async Task<QueryResultDto<SubWorkItemTypeListItemDto>> GetAllAsync(SubWorkItemTypeQueryObject queryObject)
        {
            QueryResult<SubWorkItemType> result = new();
            IQueryable<SubWorkItemType> query = null;

            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<SubWorkItemType>>(CacheKey) ?? new List<SubWorkItemType>();

            if (listValueInCache.Any())
            {
                // Set query data from cache 
                query = listValueInCache.OrderBy(n => n.Name).AsQueryable();
            }
            else
            {
                var entityDbSet = db.SubWorkItemTypes.Include(t => t.WorkItemType);

                // Set query data from database
                query = entityDbSet.OrderBy(n => n.Name).AsQueryable();

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            if (queryObject.WorkItemTypeId != null)
                query = query.Where(s => s.WorkItemTypeId == queryObject.WorkItemTypeId);

            var columnsMap = new Dictionary<string, Expression<Func<SubWorkItemType, object>>>()
            {
                ["name"] = v => v.Name,
                ["id"] = v => v.Id
            };

            query = query.ApplySorting(queryObject, columnsMap);
            result.TotalItems = query.Count();
            query = query.ApplyPaging(queryObject);
            result.Items = query.ToList();

            return mapper.Map<QueryResult<SubWorkItemType>, QueryResultDto<SubWorkItemTypeListItemDto>>(result);
        }

        public async Task<SubWorkItemTypeDto> GetAsync(int id)
        {
            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<SubWorkItemType>>(CacheKey) ?? new List<SubWorkItemType>();

            SubWorkItemType subWorkItemType = listValueInCache.FirstOrDefault(m => m.Id == id);

            if (subWorkItemType is null)
            {
                var entityDbSet = db.SubWorkItemTypes.Include(t => t.WorkItemType);

                // Get data from database
                subWorkItemType = await entityDbSet.FirstOrDefaultAsync(m => m.Id == id);

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            return mapper.Map<SubWorkItemTypeDto>(subWorkItemType);
        }

        public async Task AddAsync(SubWorkItemTypeDto subWorkItemTypeDto)
        {
            var entityDbSet = db.SubWorkItemTypes;

            var entityToAdd = mapper.Map<SubWorkItemType>(subWorkItemTypeDto);
            entityToAdd.Id = (await entityDbSet.IgnoreQueryFilters().MaxAsync(c => (int?)c.Id) ?? 0) + 1;
            entityToAdd.CreatedOn = DateTime.Now;

            await entityDbSet.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.Include(t => t.WorkItemType).ToListAsync());

            mapper.Map(entityToAdd, subWorkItemTypeDto);
        }

        public async Task EditAsync(SubWorkItemTypeDto subWorkItemTypeDto)
        {
            var entityDbSet = db.SubWorkItemTypes;

            var entityToUpdate = await entityDbSet.FindAsync(subWorkItemTypeDto.Id);
            mapper.Map(subWorkItemTypeDto, entityToUpdate);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.Include(t => t.WorkItemType).ToListAsync());

            mapper.Map(entityToUpdate, subWorkItemTypeDto);
        }

        public async Task RemoveAsync(int id)
        {
            var entityDbSet = db.SubWorkItemTypes;

            var entityToDelete = await entityDbSet.FindAsync(id);
            entityDbSet.Remove(entityToDelete);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.Include(t => t.WorkItemType).ToListAsync());
        }

        public async Task<bool> IsNameExistsAsync(SubWorkItemTypeDto subWorkItemTypeDto)
        {
            return await db.SubWorkItemTypes.AnyAsync(c => c.Id != subWorkItemTypeDto.Id && c.Name == subWorkItemTypeDto.Name && c.WorkItemTypeId == subWorkItemTypeDto.WorkItemTypeId);
        }
    }
}
