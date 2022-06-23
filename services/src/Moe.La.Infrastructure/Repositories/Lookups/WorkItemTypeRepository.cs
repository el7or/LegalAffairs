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
    public class WorkItemTypeRepository : RepositoryBase, IWorkItemTypeRepository
    {
        private readonly IDistributedCache _cache;
        private const string CacheKey = "Lookup_Work_Item_Types";

        public WorkItemTypeRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider, IDistributedCache cache)
            : base(context, mapperConfig, userProvider)
        {
            _cache = cache;
        }

        public async Task<QueryResultDto<WorkItemTypeListItemDto>> GetAllAsync(WorkItemTypeQueryObject queryObject)
        {
            QueryResult<WorkItemType> result = new();
            IQueryable<WorkItemType> query = null;

            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<WorkItemType>>(CacheKey) ?? new List<WorkItemType>();

            if (listValueInCache.Any())
            {
                // Set query data from cache 
                query = listValueInCache.OrderBy(n => n.Name).AsQueryable();
            }
            else
            {
                var entityDbSet = db.WorkItemTypes.Include(t => t.Department);

                // Set query data from database
                query = entityDbSet.OrderBy(n => n.Name).AsQueryable();

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            if (queryObject.DepartmentId.HasValue)
            {
                query = query.Where(w => w.DepartmentId == queryObject.DepartmentId);
            }

            var columnsMap = new Dictionary<string, Expression<Func<WorkItemType, object>>>()
            {
                ["name"] = v => v.Name,
                ["id"] = v => v.Id
            };

            query = query.ApplySorting(queryObject, columnsMap);
            result.TotalItems = query.Count();
            query = query.ApplyPaging(queryObject);
            result.Items = query.ToList();

            var Roles = await db.Roles.ToListAsync();

            List<WorkItemTypeListItemDto> workItems = new List<WorkItemTypeListItemDto>();

            foreach (var item in result.Items)
            {
                var roles = item.RolesIds.Split(new char[] { ',' });

                var workItem = new WorkItemTypeListItemDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Department = new KeyValuePairsDto<int> { Id = item.Department.Id, Name = item.Department.Name },
                    CreatedOn = item.CreatedOn
                };
                foreach (var role in roles)
                {
                    workItem.Roles += Roles.Where(r => r.Id.ToString() == role.Trim()).Select(r => r.NameAr).FirstOrDefault() + " , ";
                }
                workItem.Roles = workItem.Roles.Remove(workItem.Roles.LastIndexOf(','), 1);
                workItems.Add(workItem);
            }
            return new QueryResultDto<WorkItemTypeListItemDto> { Items = workItems, TotalItems = result.TotalItems };

        }

        public async Task<WorkItemTypeDto> GetAsync(int id)
        {
            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<WorkItemType>>(CacheKey) ?? new List<WorkItemType>();

            WorkItemType workItemType = listValueInCache.FirstOrDefault(m => m.Id == id);

            if (workItemType is null)
            {
                var entityDbSet = db.WorkItemTypes.Include(t => t.Department);

                // Get data from database
                workItemType = await entityDbSet.FirstOrDefaultAsync(m => m.Id == id);

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            return mapper.Map<WorkItemTypeDto>(workItemType);
        }

        public async Task<int> GetByNameAsync(string name)
        {
            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<WorkItemType>>(CacheKey) ?? new List<WorkItemType>();

            WorkItemType workItemType = listValueInCache.FirstOrDefault(s => s.Name == name);

            if (workItemType is null)
            {
                var entityDbSet = db.WorkItemTypes.Include(t => t.Department);

                // Get data from database
                workItemType = await entityDbSet.FirstOrDefaultAsync(s => s.Name == name);

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            return mapper.Map<int>(workItemType?.Id);
        }

        public async Task EditAsync(WorkItemTypeDto workItemTypeDto)
        {
            var entityDbSet = db.WorkItemTypes;

            var entityToUpdate = await entityDbSet.FindAsync(workItemTypeDto.Id);
            mapper.Map(workItemTypeDto, entityToUpdate);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.Include(t => t.Department).ToListAsync());

            mapper.Map(entityToUpdate, workItemTypeDto);
        }

        public async Task RemoveAsync(int id)
        {
            var entityDbSet = db.WorkItemTypes;

            var entityToDelete = await entityDbSet.FindAsync(id);
            entityDbSet.Remove(entityToDelete);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.Include(t => t.Department).ToListAsync());
        }

        public async Task<bool> IsNameExistsAsync(WorkItemTypeDto workItemTypeDto)
        {
            return await db.WorkItemTypes.AnyAsync(c => c.Id != workItemTypeDto.Id && c.Name == workItemTypeDto.Name && c.RolesIds == workItemTypeDto.RolesIds);
        }
    }
}
