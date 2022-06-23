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
    public class CourtRepository : RepositoryBase, ICourtRepository
    {
        private readonly IDistributedCache _cache;
        private const string CacheKey = "Lookup_Courts";

        public CourtRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider, IDistributedCache cache)
            : base(context, mapperConfig, userProvider)
        {
            _cache = cache;
        }

        public async Task<QueryResultDto<CourtListItemDto>> GetAllAsync(CourtQueryObject queryObject)
        {
            QueryResult<Court> result = new();
            IQueryable<Court> query = null;

            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<Court>>(CacheKey) ?? new List<Court>();

            if (listValueInCache.Any())
            {
                // Set query data from cache 
                query = listValueInCache.OrderBy(n => n.Name).AsQueryable();
            }
            else
            {
                var entityDbSet = db.Courts;

                // Set query data from database
                query = entityDbSet.AsQueryable();

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            if (queryObject.LitigationType.HasValue)
            {
                query = query.Where(c => c.LitigationType == queryObject.LitigationType);
            }

            if (queryObject.CourtCategory.HasValue)
            {
                query = query.Where(c => c.CourtCategory == queryObject.CourtCategory);
            }
            var columnsMap = new Dictionary<string, Expression<Func<Court, object>>>()
            {
                ["name"] = v => v.Name,
                ["type"] = v => v.LitigationType,
                ["courtCategory"] = v => v.CourtCategory,
            };

            query = query.ApplySorting(queryObject, columnsMap);
            result.TotalItems = query.Count();
            query = query.ApplyPaging(queryObject);
            result.Items = query.ToList();

            return mapper.Map<QueryResult<Court>, QueryResultDto<CourtListItemDto>>(result);
        }

        public async Task<CourtListItemDto> GetAsync(int id)
        {
            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<Court>>(CacheKey) ?? new List<Court>();

            Court court = listValueInCache.FirstOrDefault(m => m.Id == id);

            if (court is null)
            {
                var entityDbSet = db.Courts;

                // Get data from database
                court = await entityDbSet.FirstOrDefaultAsync(m => m.Id == id);

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            return mapper.Map<CourtListItemDto>(court);
        }

        public async Task AddAsync(CourtDto courtDto)
        {
            var entityDbSet = db.Courts;

            var entityToAdd = mapper.Map<Court>(courtDto);
            entityToAdd.Id = (await entityDbSet.IgnoreQueryFilters().MaxAsync(c => (int?)c.Id) ?? 0) + 1;
            entityToAdd.CreatedOn = DateTime.Now;
            await entityDbSet.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());

            mapper.Map(entityToAdd, courtDto);
        }

        public async Task EditAsync(CourtDto courtDto)
        {
            var entityDbSet = db.Courts;

            var entityToUpdate = await entityDbSet.FindAsync(courtDto.Id);
            mapper.Map(courtDto, entityToUpdate);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());

            mapper.Map(entityToUpdate, courtDto);
        }

        public async Task RemoveAsync(int id)
        {
            var entityDbSet = db.Courts;

            var entityToDelete = await entityDbSet.FindAsync(id);
            entityDbSet.Remove(entityToDelete);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
        }

        public async Task<bool> IsNameExistsAsync(CourtDto courtDto)
        {
            return await db.Courts
                .AnyAsync(c => c.Id != courtDto.Id && c.Name == courtDto.Name && c.LitigationType == courtDto.LitigationType && c.CourtCategory == courtDto.CourtCategory);
        }
    }
}
