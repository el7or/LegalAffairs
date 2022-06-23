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
    public class MainCategoryRepository : RepositoryBase, IMainCategoryRepository
    {
        private readonly IDistributedCache _cache;
        private const string CacheKey = "Lookup_Main_Categories";

        public MainCategoryRepository(LaDbContext context, IMapper mapper, IUserProvider userProvider, IDistributedCache cache)
            : base(context, mapper, userProvider)
        {
            _cache = cache;
        }

        public async Task<QueryResultDto<MainCategoryListItemDto>> GetAllAsync(MainCategoryQueryObject queryObject)
        {
            QueryResult<MainCategory> result = new();
            IQueryable<MainCategory> query = null;

            // Get data list from cache
            var listValueInCache = new List<MainCategory>();
            //await _cache.GetRecordAsync<List<MainCategory>>(CacheKey) ?? new List<MainCategory>();

            if (listValueInCache.Any())
            {
                // Set query data from cache 
                query = listValueInCache.OrderBy(n => n.Name).AsQueryable();
            }
            else
            {
                var entityDbSet = db.MainCategories.Where(m => (int)m.CaseSource == (int)queryObject.CaseSource);

                // Set query data from database
                query = entityDbSet.OrderBy(n => n.Name).AsQueryable();

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            if (queryObject.WithFirstSubCategories != null)
                query = query.Include(m => m.FirstSubCategories);

            var columnsMap = new Dictionary<string, Expression<Func<MainCategory, object>>>()
            {
                ["name"] = v => v.Name
            };

            query = query.ApplySorting(queryObject, columnsMap);
            result.TotalItems = query.Count();
            query = query.ApplyPaging(queryObject);
            result.Items = query.ToList();

            return mapper.Map<QueryResult<MainCategory>, QueryResultDto<MainCategoryListItemDto>>(result);
        }

        public async Task<MainCategoryListItemDto> GetAsync(int id)
        {
            // Get data list from cache
            var listValueInCache = new List<MainCategory>();
            //await _cache.GetRecordAsync<List<MainCategory>>(CacheKey) ?? new List<MainCategory>();

            MainCategory mainCategory = listValueInCache.FirstOrDefault(m => m.Id == id);

            if (mainCategory is null)
            {
                var entityDbSet = db.MainCategories
                    .Include(m => m.FirstSubCategories);

                // Get data from database
                mainCategory = await entityDbSet.FirstOrDefaultAsync(m => m.Id == id);

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            return mapper.Map<MainCategoryListItemDto>(mainCategory);
        }

        public async Task<MainCategoryDto> AddAsync(MainCategoryDto mainCategoryDto)
        {
            var entityDbSet = db.MainCategories;

            var entityToAdd = mapper.Map<MainCategory>(mainCategoryDto);
            entityToAdd.Id = (await entityDbSet.IgnoreQueryFilters().MaxAsync(c => (int?)c.Id) ?? 0) + 1;
            entityToAdd.CreatedOn = DateTime.Now;
            await entityDbSet.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());

            return mapper.Map(entityToAdd, mainCategoryDto);
        }

        public async Task EditAsync(MainCategoryDto mainCategoryDto)
        {
            var entityDbSet = db.MainCategories;

            var entityToUpdate = await entityDbSet.FindAsync(mainCategoryDto.Id);
            mapper.Map(mainCategoryDto, entityToUpdate);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());

            mapper.Map(entityToUpdate, mainCategoryDto);
        }

        public async Task RemoveAsync(int id)
        {
            var entityDbSet = db.MainCategories;

            var entityToDelete = await entityDbSet.FindAsync(id);
            entityDbSet.Remove(entityToDelete);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
        }

        public async Task<bool> IsNameExistsAsync(MainCategoryDto mainCategoryDto)
        {
            return await db.MainCategories
                .AnyAsync(c => c.Name == mainCategoryDto.Name);
            //.AnyAsync(c => c.Id != mainCategoryDto.Id && c.Name == mainCategoryDto.Name);

        }
    }
}
