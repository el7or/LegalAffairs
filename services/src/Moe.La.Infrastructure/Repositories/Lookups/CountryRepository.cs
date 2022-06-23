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
    public class CountryRepository : RepositoryBase, ICountryRepository
    {
        private readonly IDistributedCache _cache;
        private const string CacheKey = "Lookup_Countries";

        public CountryRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider, IDistributedCache cache)
            : base(context, mapperConfig, userProvider)
        {
            _cache = cache;
        }

        public async Task<QueryResultDto<CountryListItemDto>> GetAllAsync(QueryObject queryObject)
        {
            QueryResult<Country> result = new();
            IQueryable<Country> query = null;

            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<Country>>(CacheKey) ?? new List<Country>();

            if (listValueInCache.Any())
            {
                // Set query data from cache 
                query = listValueInCache.OrderBy(n => n.NameAr).AsQueryable();
            }
            else
            {
                var entityDbSet = db.Countries;

                // Set query data from database
                query = entityDbSet.AsQueryable();

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            var columnsMap = new Dictionary<string, Expression<Func<Country, object>>>()
            {
                ["nameAr"] = v => v.NameAr,
                ["nameEn"] = v => v.NameEn
            };

            query = query.ApplySorting(queryObject, columnsMap);

            result.TotalItems = query.Count();

            query = query.ApplyPaging(queryObject);

            result.Items = query.AsNoTracking().ToList();

            return mapper.Map<QueryResult<Country>, QueryResultDto<CountryListItemDto>>(result);
        }

        public async Task<CountryDetailsDto> GetAsync(int id)
        {
            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<Country>>(CacheKey) ?? new List<Country>();

            Country country = listValueInCache.FirstOrDefault(m => m.Id == id);

            if (country is null)
            {
                var entityDbSet = db.Countries;

                // Get data from database
                country = await entityDbSet.FirstOrDefaultAsync(m => m.Id == id);

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            return mapper.Map<CountryDetailsDto>(country);
        }

        public async Task AddAsync(CountryDto CountryDto)
        {
            var entityDbSet = db.Countries;

            var entityToAdd = mapper.Map<Country>(CountryDto);
            entityToAdd.Id = (await entityDbSet.IgnoreQueryFilters().MaxAsync(c => (int?)c.Id) ?? 0) + 1;
            entityToAdd.CreatedOn = DateTime.Now;
            await entityDbSet.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());

            mapper.Map(entityToAdd, CountryDto);
        }

        public async Task EditAsync(CountryDto CountryDto)
        {
            var entityDbSet = db.Countries;

            var entityToUpdate = await entityDbSet.FindAsync(CountryDto.Id);
            mapper.Map(CountryDto, entityToUpdate);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());

            mapper.Map(entityToUpdate, CountryDto);
        }

        public async Task RemoveAsync(int id)
        {
            var entityDbSet = db.Countries;

            var entityToDelete = await entityDbSet.FindAsync(id);
            entityDbSet.Remove(entityToDelete);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
        }
        public async Task<bool> IsNameExistsAsync(CountryDto countryDto)
        {
            return await db.Countries.AnyAsync(c => c.Id != countryDto.Id && c.NameAr == countryDto.NameAr);
        }
    }
}
