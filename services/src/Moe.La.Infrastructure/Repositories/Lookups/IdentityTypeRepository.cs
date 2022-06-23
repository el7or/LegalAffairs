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
    public class IdentityTypeRepository : RepositoryBase, IIdentityTypeRepository
    {
        private readonly IDistributedCache _cache;
        private const string CacheKey = "Lookup_Identity_Types";

        public IdentityTypeRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider, IDistributedCache cache)
            : base(context, mapperConfig, userProvider)
        {
            _cache = cache;
        }

        public async Task<QueryResultDto<IdentityTypeListItemDto>> GetAllAsync(QueryObject queryObject)
        {
            QueryResult<IdentityType> result = new();
            IQueryable<IdentityType> query = null;

            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<IdentityType>>(CacheKey) ?? new List<IdentityType>();

            if (listValueInCache.Any())
            {
                // Set query data from cache 
                query = listValueInCache.OrderBy(n => n.Name).AsQueryable();
            }
            else
            {
                var entityDbSet = db.IdentityTypes;

                // Set query data from database
                query = entityDbSet.OrderBy(n => n.Name).AsQueryable();

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            var columnsMap = new Dictionary<string, Expression<Func<IdentityType, object>>>()
            {
                ["name"] = v => v.Name
            };

            query = query.ApplySorting(queryObject, columnsMap);
            result.TotalItems = query.Count();
            query = query.ApplyPaging(queryObject);
            result.Items = query.ToList();

            return mapper.Map<QueryResult<IdentityType>, QueryResultDto<IdentityTypeListItemDto>>(result);
        }

        public async Task<IdentityTypeListItemDto> GetAsync(int id)
        {
            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<IdentityType>>(CacheKey) ?? new List<IdentityType>();

            IdentityType identityType = listValueInCache.FirstOrDefault(m => m.Id == id);

            if (identityType is null)
            {
                var entityDbSet = db.IdentityTypes;

                // Get data from database
                identityType = await entityDbSet.FirstOrDefaultAsync(m => m.Id == id);

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            return mapper.Map<IdentityType, IdentityTypeListItemDto>(identityType);
        }

        public async Task AddAsync(IdentityTypeDto identityTypeDto)
        {
            var entityDbSet = db.IdentityTypes;

            var entityToAdd = mapper.Map<IdentityType>(identityTypeDto);
            entityToAdd.Id = (await entityDbSet.IgnoreQueryFilters().MaxAsync(c => (int?)c.Id) ?? 0) + 1;
            entityToAdd.CreatedOn = DateTime.Now;
            await entityDbSet.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());

            mapper.Map(entityToAdd, identityTypeDto);
        }

        public async Task EditAsync(IdentityTypeDto identityTypeDto)
        {
            var entityDbSet = db.IdentityTypes;

            var entityToUpdate = await entityDbSet.FindAsync(identityTypeDto.Id);
            mapper.Map(identityTypeDto, entityToUpdate);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());

            mapper.Map(entityToUpdate, identityTypeDto);
        }

        public async Task RemoveAsync(int id)
        {
            var entityDbSet = db.IdentityTypes;

            var entityToDelete = await entityDbSet.FindAsync(id);
            entityDbSet.Remove(entityToDelete);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
        }

        public async Task<bool> IsNameExistsAsync(string name)
        {
            return await db.IdentityTypes.AnyAsync(c => c.Name == name);
        }
    }
}
