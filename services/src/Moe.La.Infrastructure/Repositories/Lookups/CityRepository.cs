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
    public class CityRepository : RepositoryBase, ICityRepository
    {
        private readonly IDistributedCache _cache;
        private const string CacheKey = "Lookup_Cities";

        public CityRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider, IDistributedCache cache)
            : base(context, mapperConfig, userProvider)
        {
            _cache = cache;
        }

        public async Task<QueryResultDto<CityListItemDto>> GetAllAsync(CityQueryObject queryObject)
        {
            QueryResult<City> result = new();
            IQueryable<City> query = null;

            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<City>>(CacheKey) ?? new List<City>();

            if (listValueInCache.Any())
            {
                // Set query data from cache 
                query = listValueInCache.OrderBy(n => n.Name).AsQueryable();
            }
            else
            {
                var entityDbSet = db.Cities;

                // Set query data from database
                query = entityDbSet.Include(s => s.Province)
                                   .OrderBy(n => n.Name)
                                   .AsNoTracking()
                                   .AsQueryable();

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await query.ToListAsync());
            }

            if (queryObject.ProvinceId.HasValue)
                query = query.Where(s => s.ProvinceId == queryObject.ProvinceId);

            var columnsMap = new Dictionary<string, Expression<Func<City, object>>>()
            {
                ["name"] = v => v.Name,
                ["provinces"] = v => v.Province.Name
            };

            query = query.ApplySorting(queryObject, columnsMap);
            result.TotalItems = query.Count();
            query = query.ApplyPaging(queryObject);
            result.Items = query.ToList();

            return mapper.Map<QueryResult<City>, QueryResultDto<CityListItemDto>>(result);
        }

        public async Task<CityListItemDto> GetAsync(int id)
        {
            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<City>>(CacheKey) ?? new List<City>();

            City city = listValueInCache.FirstOrDefault(m => m.Id == id);

            if (city is null)
            {
                var entityDbSet = db.Cities;

                // Get data from database
                city = await entityDbSet.FirstOrDefaultAsync(m => m.Id == id);

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.Include(x => x.Province).ToListAsync());
            }

            return mapper.Map<CityListItemDto>(city);
        }

        public async Task<CityListItemDto> GetByNameAsync(string name)
        {
            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<City>>(CacheKey) ?? new List<City>();

            City city = listValueInCache.FirstOrDefault(m => m.Name == name);

            if (city is null)
            {
                var entityDbSet = db.Cities;

                // Get data from database
                city = await entityDbSet.Include(s => s.Province)
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(m => m.Name == name);

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.Include(x => x.Province).ToListAsync());
            }

            return mapper.Map<CityListItemDto>(city);
        }

        public async Task AddAsync(CityDto cityDto)
        {
            var entityDbSet = db.Cities;

            var entityToAdd = mapper.Map<City>(cityDto);
            entityToAdd.Id = (await entityDbSet.IgnoreQueryFilters().MaxAsync(c => (int?)c.Id) ?? 0) + 1;
            entityToAdd.CreatedOn = DateTime.Now;
            await entityDbSet.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.Include(x => x.Province).ToListAsync());

            mapper.Map(entityToAdd, cityDto);
        }

        public async Task EditAsync(CityDto cityDto)
        {
            var entityDbSet = db.Cities;

            var entityToUpdate = await entityDbSet.FindAsync(cityDto.Id);
            mapper.Map(cityDto, entityToUpdate);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.Include(x => x.Province).ToListAsync());

            mapper.Map(entityToUpdate, cityDto);
        }

        public async Task RemoveAsync(int id)
        {
            var entityDbSet = db.Cities;

            var entityToDelete = await entityDbSet.FindAsync(id);
            entityDbSet.Remove(entityToDelete);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.Include(x => x.Province).ToListAsync());
        }

        public async Task<bool> IsNameExistsAsync(CityDto cityDto)
        {
            return await db.Cities.AnyAsync(c => c.Id != cityDto.Id && c.Name == cityDto.Name && c.ProvinceId == cityDto.ProvinceId);
        }
    }
}
