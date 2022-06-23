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
    public class LetterTemplateRepository : RepositoryBase, ILetterTemplateRepository
    {
        private readonly IDistributedCache _cache;
        private const string CacheKey = "Lookup_LetterTemplates";

        public LetterTemplateRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider, IDistributedCache cache)
            : base(context, mapperConfig, userProvider)
        {
            _cache = cache;
        }

        public async Task<QueryResultDto<LetterTemplateDto>> GetAllAsync(TemplateQueryObject queryObject)
        {
            QueryResult<LetterTemplate> result = new();
            IQueryable<LetterTemplate> query = null;

            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<LetterTemplate>>(CacheKey) ?? new List<LetterTemplate>();

            if (listValueInCache.Any())
            {
                // Set query data from cache 
                query = listValueInCache.OrderBy(n => n.Name).AsQueryable();
            }
            else
            {
                var entityDbSet = db.LetterTemplates;

                // Set query data from database
                query = entityDbSet.AsQueryable();

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            if (!string.IsNullOrEmpty(queryObject.Name))
            {
                query = query.Where(c => c.Name == queryObject.Name);
            }

            if (queryObject.Type.HasValue)
            {
                query = query.Where(c => c.Type == queryObject.Type);
            }

            var columnsMap = new Dictionary<string, Expression<Func<LetterTemplate, object>>>()
            {
                ["name"] = v => v.Name
            };

            query = query.ApplySorting(queryObject, columnsMap);

            result.TotalItems = query.Count();

            query = query.ApplyPaging(queryObject);

            result.Items = query.AsNoTracking().ToList();

            return mapper.Map<QueryResult<LetterTemplate>, QueryResultDto<LetterTemplateDto>>(result);
        }

        public async Task<LetterTemplateDto> GetAsync(int id)
        {
            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<LetterTemplate>>(CacheKey) ?? new List<LetterTemplate>();

            LetterTemplate template = listValueInCache.FirstOrDefault(m => m.Id == id);

            if (template is null)
            {
                var entityDbSet = db.LetterTemplates;

                // Get data from database
                template = await entityDbSet.FirstOrDefaultAsync(m => m.Id == id);

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            return mapper.Map<LetterTemplateDto>(template);
        }

        public async Task AddAsync(LetterTemplateDto tampleteDto)
        {
            var entityDbSet = db.LetterTemplates;

            var entityToAdd = mapper.Map<LetterTemplate>(tampleteDto);
            entityToAdd.Id = (await entityDbSet.IgnoreQueryFilters().MaxAsync(c => (int?)c.Id) ?? 0) + 1;
            entityToAdd.CreatedOn = DateTime.Now;
            await entityDbSet.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());

            mapper.Map(entityToAdd, tampleteDto);
        }

        public async Task EditAsync(LetterTemplateDto tampleteDto)
        {
            var entityDbSet = db.LetterTemplates;

            var entityToUpdate = await entityDbSet.FindAsync(tampleteDto.Id);
            mapper.Map(tampleteDto, entityToUpdate);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());

            mapper.Map(entityToUpdate, tampleteDto);
        }

        public async Task RemoveAsync(int id)
        {
            var entityDbSet = db.LetterTemplates;

            var entityToDelete = await entityDbSet.FindAsync(id);
            entityDbSet.Remove(entityToDelete);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
        }
    }
}
