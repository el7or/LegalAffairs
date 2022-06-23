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
    public class DepartmentRepository : RepositoryBase, IDepartmentRepository
    {
        private readonly IDistributedCache _cache;
        private const string CacheKey = "Lookup_Departments";

        public DepartmentRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider, IDistributedCache cache)
            : base(context, mapperConfig, userProvider)
        {
            _cache = cache;
        }

        public async Task<QueryResultDto<DepartmentListItemDto>> GetAllAsync(QueryObject queryObject)
        {
            QueryResult<Department> result = new();
            IQueryable<Department> query = null;

            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<Department>>(CacheKey) ?? new List<Department>();

            if (listValueInCache.Any())
            {
                // Set query data from cache 
                query = listValueInCache.OrderBy(n => n.Name).AsQueryable();
            }
            else
            {
                var entityDbSet = db.Departments;

                // Set query data from database
                query = entityDbSet.OrderBy(n => n.Name).AsQueryable();

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            var columnsMap = new Dictionary<string, Expression<Func<Department, object>>>()
            {
                ["name"] = v => v.Name,
                ["order"] = v => v.Order
            };

            query = query.ApplySorting(queryObject, columnsMap);
            result.TotalItems = query.Count();
            query = query.ApplyPaging(queryObject);
            result.Items = query.ToList();

            return mapper.Map<QueryResult<Department>, QueryResultDto<DepartmentListItemDto>>(result);
        }

        public async Task<DepartmentListItemDto> GetAsync(int id)
        {
            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<Department>>(CacheKey) ?? new List<Department>();

            Department department = listValueInCache.FirstOrDefault(m => m.Id == id);

            if (department is null)
            {
                var entityDbSet = db.Departments;

                // Get data from database
                department = await entityDbSet.FirstOrDefaultAsync(m => m.Id == id);

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            return mapper.Map<DepartmentListItemDto>(department);
        }

        public async Task AddAsync(DepartmentDto departmentDto)
        {
            var entityDbSet = db.Departments;

            var entityToAdd = mapper.Map<Department>(departmentDto);
            entityToAdd.Id = (await entityDbSet.IgnoreQueryFilters().MaxAsync(c => (int?)c.Id) ?? 0) + 1;
            entityToAdd.Order = 3;  // priority of department 
            entityToAdd.CreatedOn = DateTime.Now;
            await entityDbSet.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());

            mapper.Map(entityToAdd, departmentDto);
        }

        public async Task EditAsync(DepartmentDto departmentDto)
        {
            var entityDbSet = db.Departments;

            var entityToUpdate = await entityDbSet.FindAsync(departmentDto.Id);
            mapper.Map(departmentDto, entityToUpdate);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());

            mapper.Map(entityToUpdate, departmentDto);
        }

        public async Task RemoveAsync(int id, Guid userId)
        {
            var entityDbSet = db.Departments;

            var entityToDelete = await entityDbSet.FindAsync(id);
            entityDbSet.Remove(entityToDelete);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
        }

        public async Task<bool> IsNameExistsAsync(string name)
        {
            return await db.Departments.AnyAsync(c => c.Name == name);
        }
    }
}
