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
    public class MinistryDepartmentRepository : RepositoryBase, IMinistryDepartmentRepository
    {
        private readonly IDistributedCache _cache;
        private const string CacheKey = "Lookup_Ministry_Departments";

        public MinistryDepartmentRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider, IDistributedCache cache)
            : base(context, mapperConfig, userProvider)
        {
            _cache = cache;
        }

        public async Task<QueryResultDto<MinistryDepartmentListItemDto>> GetAllAsync(MinistryDepartmentQueryObject queryObject)
        {
            QueryResult<MinistryDepartment> result = new();
            IQueryable<MinistryDepartment> query = null;

            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<MinistryDepartment>>(CacheKey) ?? new List<MinistryDepartment>();

            if (listValueInCache.Any())
            {
                // Set query data from cache 
                query = listValueInCache.OrderBy(n => n.Name).AsQueryable();
            }
            else
            {
                var entityDbSet = db.MinistryDepartments.Include(d => d.MinistrySector);

                // Set query data from database
                query = entityDbSet.OrderBy(n => n.Name).AsQueryable();

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            var columnsMap = new Dictionary<string, Expression<Func<MinistryDepartment, object>>>()
            {
                ["name"] = v => v.Name
            };

            query = query.ApplySorting(queryObject, columnsMap);
            result.TotalItems = query.Count();
            query = query.ApplyPaging(queryObject);
            result.Items = query.ToList();

            return mapper.Map<QueryResult<MinistryDepartment>, QueryResultDto<MinistryDepartmentListItemDto>>(result);
        }

        public async Task<MinistryDepartmentListItemDto> GetAsync(int id)
        {
            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<MinistryDepartment>>(CacheKey) ?? new List<MinistryDepartment>();

            MinistryDepartment ministryDepartment = listValueInCache.FirstOrDefault(m => m.Id == id);

            if (ministryDepartment is null)
            {
                var entityDbSet = db.MinistryDepartments;

                // Get data from database
                ministryDepartment = await entityDbSet.FirstOrDefaultAsync(m => m.Id == id);

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            return mapper.Map<MinistryDepartmentListItemDto>(ministryDepartment);
        }

        public async Task AddAsync(MinistryDepartmentDto ministryDepartmentDto)
        {
            var entityDbSet = db.MinistryDepartments;

            var entityToAdd = mapper.Map<MinistryDepartment>(ministryDepartmentDto);
            entityToAdd.Id = (await entityDbSet.IgnoreQueryFilters().MaxAsync(c => (int?)c.Id) ?? 0) + 1;
            entityToAdd.CreatedOn = DateTime.Now;
            await entityDbSet.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.Include(d => d.MinistrySector).ToListAsync());

            mapper.Map(entityToAdd, ministryDepartmentDto);
        }

        public async Task EditAsync(MinistryDepartmentDto ministryDepartmentDto)
        {
            var entityDbSet = db.MinistryDepartments;

            var entityToUpdate = await entityDbSet.FindAsync(ministryDepartmentDto.Id);
            mapper.Map(ministryDepartmentDto, entityToUpdate);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());

            mapper.Map(entityToUpdate, ministryDepartmentDto);
        }

        public async Task RemoveAsync(int id, Guid userId)
        {
            var entityDbSet = db.MinistryDepartments;

            var entityToDelete = await entityDbSet.FindAsync(id);
            entityDbSet.Remove(entityToDelete);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
        }

        public async Task<int> GetDepartmentIdAsync(string Name)
        {
            var department = await db.MinistryDepartments.FirstOrDefaultAsync(d => d.Name == Name);
            return department != null ? department.Id : 0;
        }

        public async Task<bool> IsNameExistsAsync(MinistryDepartmentDto ministryDepartmentDto)
        {
            return await db.MinistryDepartments
                .AnyAsync(c => c.Id != ministryDepartmentDto.Id && c.Name == ministryDepartmentDto.Name);

        }
    }
}
