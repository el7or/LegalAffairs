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
    public class DistrictRepository : RepositoryBase, IDistrictRepository
    {
        private readonly IDistributedCache _cache;
        private const string CacheKey = "Lookup_Districts";

        public DistrictRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider, IDistributedCache cache)
            : base(context, mapperConfig, userProvider)
        {
            _cache = cache;
        }

        public async Task<QueryResultDto<DistrictListItemDto>> GetAllAsync(DistrictQueryObject queryObject)
        {
            QueryResult<District> result = new();
            IQueryable<District> query = null;

            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<District>>(CacheKey) ?? new List<District>();

            if (listValueInCache.Any())
            {
                // Set query data from cache 
                query = listValueInCache.OrderBy(n => n.Name).AsQueryable();
            }
            else
            {
                var entityDbSet = db.Districts.Include(c => c.City);

                // Set query data from database
                query = entityDbSet.OrderBy(n => n.Name).AsQueryable();

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            if (queryObject.CityId.HasValue)
            {
                query = query.Where(c => c.CityId == queryObject.CityId);
            }

            var columnsMap = new Dictionary<string, Expression<Func<District, object>>>()
            {
                ["name"] = v => v.Name
            };

            query = query.ApplySorting(queryObject, columnsMap);
            result.TotalItems = query.Count();
            query = query.ApplyPaging(queryObject);
            result.Items = query.ToList();

            return Mapper.Map<QueryResult<District>, QueryResultDto<DistrictListItemDto>>(result);
        }

        public async Task<DistrictDto> GetAsync(int id)
        {

            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<District>>(CacheKey) ?? new List<District>();

            District district = listValueInCache.FirstOrDefault(m => m.Id == id);

            if (district is null)
            {
                var entityDbSet = db.Districts.Include(d => d.City);

                // Get data from database
                district = await entityDbSet.FirstOrDefaultAsync(m => m.Id == id);

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            return mapper.Map<DistrictDto>(district);
        }

        public async Task<DistrictDto> AddAsync(DistrictDto districtDto)
        {
            var entityDbSet = db.Districts;

            var entityToAdd = mapper.Map<District>(districtDto);
            entityToAdd.Id = (await entityDbSet.IgnoreQueryFilters().MaxAsync(c => (int?)c.Id) ?? 0) + 1;
            entityToAdd.CreatedOn = DateTime.Now;
            await entityDbSet.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.Include(d => d.City).ToListAsync());

            return mapper.Map(entityToAdd, districtDto);
        }

        public async Task EditAsync(DistrictDto districtDto)
        {
            var entityDbSet = db.Districts;

            var entityToUpdate = await entityDbSet.FindAsync(districtDto.Id);
            mapper.Map(districtDto, entityToUpdate);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.Include(d => d.City).ToListAsync());

            mapper.Map(entityToUpdate, districtDto);
        }

        public async Task RemoveAsync(int id)
        {
            var entityDbSet = db.Districts;

            var entityToDelete = await entityDbSet.FindAsync(id);
            entityDbSet.Remove(entityToDelete);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.Include(d => d.City).ToListAsync());
        }

        public async Task<bool> IsNameExistsAsync(DistrictDto districtDto)
        {
            return await db.Districts
                            .AnyAsync(c => c.Id != districtDto.Id && c.Name == districtDto.Name && c.CityId == districtDto.CityId);
        }
    }
}
