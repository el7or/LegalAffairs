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
    public class GovernmentOrganizationRepository : RepositoryBase, IGovernmentOrganizationRepository
    {
        private readonly IDistributedCache _cache;
        private const string CacheKey = "Lookup_Government_Organizations";

        public GovernmentOrganizationRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider, IDistributedCache cache)
            : base(context, mapperConfig, userProvider)
        {
            _cache = cache;
        }

        public async Task<QueryResultDto<GovernmentOrganizationListItemDto>> GetAllAsync(QueryObject queryObject)
        {
            QueryResult<GovernmentOrganization> result = new();
            IQueryable<GovernmentOrganization> query = null;

            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<GovernmentOrganization>>(CacheKey) ?? new List<GovernmentOrganization>();

            if (listValueInCache.Any())
            {
                // Set query data from cache 
                query = listValueInCache.OrderBy(n => n.Name).AsQueryable();
            }
            else
            {
                var entityDbSet = db.GovernmentOrganizations;

                // Set query data from database
                query = entityDbSet.OrderBy(n => n.Name).AsQueryable();

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }
            var columnsMap = new Dictionary<string, Expression<Func<GovernmentOrganization, object>>>()
            {
                ["name"] = v => v.Name
            };

            query = query.ApplySorting(queryObject, columnsMap);

            result.TotalItems = query.Count();

            query = query.ApplyPaging(queryObject);

            result.Items = query.ToList();

            return Mapper.Map<QueryResult<GovernmentOrganization>, QueryResultDto<GovernmentOrganizationListItemDto>>(result);
        }

        public async Task<GovernmentOrganizationListItemDto> GetAsync(int id)
        {
            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<GovernmentOrganization>>(CacheKey) ?? new List<GovernmentOrganization>();

            GovernmentOrganization governmentOrganization = listValueInCache.FirstOrDefault(m => m.Id == id);

            if (governmentOrganization is null)
            {
                var entityDbSet = db.GovernmentOrganizations;

                // Get data from database
                governmentOrganization = await entityDbSet.FirstOrDefaultAsync(m => m.Id == id);

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            return mapper.Map<GovernmentOrganizationListItemDto>(governmentOrganization);
        }

        public async Task<GovernmentOrganizationDto> AddAsync(GovernmentOrganizationDto GovernmentOrganizationDto)
        {
            var entityDbSet = db.GovernmentOrganizations;

            var entityToAdd = mapper.Map<GovernmentOrganization>(GovernmentOrganizationDto);
            entityToAdd.Id = (await entityDbSet.IgnoreQueryFilters().MaxAsync(c => (int?)c.Id) ?? 0) + 1;
            entityToAdd.CreatedOn = DateTime.Now;
            await entityDbSet.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());

            return mapper.Map(entityToAdd, GovernmentOrganizationDto);
        }

        public async Task EditAsync(GovernmentOrganizationDto GovernmentOrganizationDto)
        {
            var entityDbSet = db.GovernmentOrganizations;

            var entityToUpdate = await entityDbSet.FindAsync(GovernmentOrganizationDto.Id);
            mapper.Map(GovernmentOrganizationDto, entityToUpdate);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());

            mapper.Map(entityToUpdate, GovernmentOrganizationDto);
        }

        public async Task RemoveAsync(int id)
        {
            var entityDbSet = db.GovernmentOrganizations;

            var entityToDelete = await entityDbSet.FindAsync(id);
            entityDbSet.Remove(entityToDelete);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
        }

        public async Task<bool> IsNameExistsAsync(string name)
        {
            return await db.GovernmentOrganizations.AnyAsync(c => c.Name == name);
        }
    }
}
