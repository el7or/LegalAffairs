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
    public class InvestigationRecordPartyTypeRepository : RepositoryBase, IInvestigationRecordPartyTypeRepository
    {
        private readonly IDistributedCache _cache;
        private const string CacheKey = "Lookup_Investigation_Record_Party_Types";

        public InvestigationRecordPartyTypeRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider, IDistributedCache cache)
            : base(context, mapperConfig, userProvider)
        {
            _cache = cache;
        }

        public async Task<QueryResultDto<InvestigationRecordPartyTypeListItemDto>> GetAllAsync(QueryObject queryObject)
        {
            QueryResult<InvestigationRecordPartyType> result = new();
            IQueryable<InvestigationRecordPartyType> query = null;

            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<InvestigationRecordPartyType>>(CacheKey) ?? new List<InvestigationRecordPartyType>();

            if (listValueInCache.Any())
            {
                // Set query data from cache 
                query = listValueInCache.OrderBy(n => n.Name).AsQueryable();
            }
            else
            {
                var entityDbSet = db.InvestigationRecordPartyTypes;

                // Set query data from database
                query = entityDbSet.OrderBy(n => n.Name).AsQueryable();

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            var columnsMap = new Dictionary<string, Expression<Func<InvestigationRecordPartyType, object>>>()
            {
                ["name"] = v => v.Name
            };

            query = query.ApplySorting(queryObject, columnsMap);
            result.TotalItems = query.Count();
            query = query.ApplyPaging(queryObject);
            result.Items = query.ToList();

            return mapper.Map<QueryResult<InvestigationRecordPartyType>, QueryResultDto<InvestigationRecordPartyTypeListItemDto>>(result);
        }

        public async Task<InvestigationRecordPartyTypeListItemDto> GetAsync(int id)
        {
            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<InvestigationRecordPartyType>>(CacheKey) ?? new List<InvestigationRecordPartyType>();

            InvestigationRecordPartyType investigationRecordPartyType = listValueInCache.FirstOrDefault(m => m.Id == id);

            if (investigationRecordPartyType is null)
            {
                var entityDbSet = db.InvestigationRecordPartyTypes;

                // Get data from database
                investigationRecordPartyType = await entityDbSet.FirstOrDefaultAsync(m => m.Id == id);

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            return mapper.Map<InvestigationRecordPartyType, InvestigationRecordPartyTypeListItemDto>(investigationRecordPartyType);
        }

        public async Task AddAsync(InvestigationRecordPartyTypeDto investigationRecordPartyTypeDto)
        {
            var entityDbSet = db.InvestigationRecordPartyTypes;

            var entityToAdd = mapper.Map<InvestigationRecordPartyType>(investigationRecordPartyTypeDto);
            entityToAdd.Id = (await entityDbSet.IgnoreQueryFilters().MaxAsync(c => (int?)c.Id) ?? 0) + 1;
            entityToAdd.CreatedOn = DateTime.Now;
            await entityDbSet.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());

            mapper.Map(entityToAdd, investigationRecordPartyTypeDto);
        }

        public async Task EditAsync(InvestigationRecordPartyTypeDto investigationRecordPartyTypeDto)
        {
            var entityDbSet = db.InvestigationRecordPartyTypes;

            var entityToUpdate = await entityDbSet.FindAsync(investigationRecordPartyTypeDto.Id);
            mapper.Map(investigationRecordPartyTypeDto, entityToUpdate);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());

            mapper.Map(entityToUpdate, investigationRecordPartyTypeDto);
        }

        public async Task RemoveAsync(int id)
        {
            var entityDbSet = db.InvestigationRecordPartyTypes;

            var entityToDelete = await entityDbSet.FindAsync(id);
            entityDbSet.Remove(entityToDelete);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
        }

        public async Task<bool> IsNameExistsAsync(string name)
        {
            return await db.InvestigationRecordPartyTypes.AnyAsync(c => c.Name == name);
        }
    }
}
