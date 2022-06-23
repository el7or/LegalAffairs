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
    public class MinistrySectorRepository : RepositoryBase, IMinistrySectorRepository
    {
        private readonly IDistributedCache _cache;
        private const string CacheKey = "Lookup_Ministry_Sectors";

        public MinistrySectorRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider, IDistributedCache cache)
            : base(context, mapperConfig, userProvider)
        {
            _cache = cache;
        }

        public async Task<QueryResultDto<MinistrySectorListItemDto>> GetAllAsync(QueryObject queryObject)
        {
            QueryResult<MinistrySector> result = new();
            IQueryable<MinistrySector> query = null;

            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<MinistrySector>>(CacheKey) ?? new List<MinistrySector>();

            if (listValueInCache.Any())
            {
                // Set query data from cache 
                query = listValueInCache.OrderBy(n => n.Name).AsQueryable();
            }
            else
            {
                var entityDbSet = db.MinistrySectors;

                // Set query data from database
                query = entityDbSet.OrderBy(n => n.Name).AsQueryable();

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            var columnsMap = new Dictionary<string, Expression<Func<MinistrySector, object>>>()
            {
                ["name"] = v => v.Name
            };

            query = query.ApplySorting(queryObject, columnsMap);
            result.TotalItems = query.Count();
            query = query.ApplyPaging(queryObject);
            result.Items = query.ToList();

            return mapper.Map<QueryResult<MinistrySector>, QueryResultDto<MinistrySectorListItemDto>>(result);
        }

        public async Task<MinistrySectorListItemDto> GetAsync(int id)
        {
            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<MinistrySector>>(CacheKey) ?? new List<MinistrySector>();

            MinistrySector ministrySector = listValueInCache.FirstOrDefault(m => m.Id == id);

            if (ministrySector is null)
            {
                var entityDbSet = db.MinistrySectors;

                // Get data from database
                ministrySector = await entityDbSet.FirstOrDefaultAsync(m => m.Id == id);

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            return mapper.Map<MinistrySectorListItemDto>(ministrySector);
        }

        public async Task AddAsync(MinistrySectorDto ministrySectorDto)
        {
            var entityDbSet = db.MinistrySectors;

            var entityToAdd = mapper.Map<MinistrySector>(ministrySectorDto);
            entityToAdd.Id = (await entityDbSet.IgnoreQueryFilters().MaxAsync(c => (int?)c.Id) ?? 0) + 1;
            entityToAdd.CreatedOn = DateTime.Now;
            await entityDbSet.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());

            mapper.Map(entityToAdd, ministrySectorDto);
        }

        public async Task EditAsync(MinistrySectorDto ministrySectorDto)
        {
            var entityDbSet = db.MinistrySectors;

            var entityToUpdate = await entityDbSet.FindAsync(ministrySectorDto.Id);
            mapper.Map(ministrySectorDto, entityToUpdate);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());

            mapper.Map(entityToUpdate, ministrySectorDto);
        }

        public async Task RemoveAsync(int id, Guid userId)
        {
            var entityDbSet = db.MinistrySectors;

            var entityToDelete = await entityDbSet.FindAsync(id);
            entityDbSet.Remove(entityToDelete);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
        }

        public async Task<int> GetSectorIdAsync(string Name)
        {
            var Sector = await db.MinistrySectors.FirstOrDefaultAsync(d => d.Name == Name);
            return Sector != null ? Sector.Id : 0;
        }

        public async Task<bool> IsNameExistsAsync(string name)
        {
            return await db.MinistrySectors.AnyAsync(c => c.Name == name);

        }
    }
}
