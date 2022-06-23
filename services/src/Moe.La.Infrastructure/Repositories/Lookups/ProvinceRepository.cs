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
    public class ProvinceRepository : RepositoryBase, IProvinceRepository
    {
        private readonly IDistributedCache _cache;
        private const string CacheKey = "Lookup_Provinces";

        public ProvinceRepository(LaDbContext context, IMapper mapper, IUserProvider userProvider, IDistributedCache cache)
            : base(context, mapper, userProvider)
        {
            _cache = cache;
        }

        public async Task<QueryResultDto<ProvinceListItemDto>> GetAllAsync(ProvinceQueryObject queryObject)
        {
            QueryResult<Province> result = new();
            IQueryable<Province> query = null;

            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<Province>>(CacheKey) ?? new List<Province>();

            if (listValueInCache.Any())
            {
                // Set query data from cache 
                query = listValueInCache.OrderBy(n => n.Name).AsQueryable();
            }
            else
            {
                var entityDbSet = db.Provinces;

                // Set query data from database
                query = entityDbSet.OrderBy(n => n.Name).AsQueryable();

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            if (queryObject.WithCities != null)
                query = query.Include(m => m.Cities);

            var columnsMap = new Dictionary<string, Expression<Func<Province, object>>>()
            {
                ["name"] = v => v.Name
            };

            query = query.ApplySorting(queryObject, columnsMap);
            result.TotalItems = query.Count();
            query = query.ApplyPaging(queryObject);
            result.Items = query.ToList();

            return mapper.Map<QueryResult<Province>, QueryResultDto<ProvinceListItemDto>>(result);
        }

        public async Task<ProvinceListItemDto> GetAsync(int id)
        {
            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<Province>>(CacheKey) ?? new List<Province>();

            Province province = listValueInCache.FirstOrDefault(m => m.Id == id);

            if (province is null)
            {
                var entityDbSet = db.Provinces
                    .Include(m => m.Cities);

                // Get data from database
                province = await entityDbSet.FirstOrDefaultAsync(m => m.Id == id);

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            return mapper.Map<ProvinceListItemDto>(province);
        }

        public async Task AddAsync(ProvinceDto provinceDto)
        {
            var entityDbSet = db.Provinces;

            var entityToAdd = mapper.Map<Province>(provinceDto);
            entityToAdd.Id = (await entityDbSet.IgnoreQueryFilters().MaxAsync(c => (int?)c.Id) ?? 0) + 1;
            entityToAdd.CreatedOn = DateTime.Now;
            await entityDbSet.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());

            mapper.Map(entityToAdd, provinceDto);
        }

        public async Task EditAsync(ProvinceDto provinceDto)
        {
            var entityDbSet = db.Provinces;

            var entityToUpdate = await entityDbSet.FindAsync(provinceDto.Id);
            mapper.Map(provinceDto, entityToUpdate);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());

            mapper.Map(entityToUpdate, provinceDto);
        }

        public async Task RemoveAsync(int id)
        {
            var entityDbSet = db.Provinces;

            var entityToDelete = await entityDbSet.FindAsync(id);
            entityDbSet.Remove(entityToDelete);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
        }

        public async Task<bool> IsNameExistsAsync(string name)
        {
            return await db.Provinces.AnyAsync(c => c.Name == name);
        }
    }
}
