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
    public class FirstSubCategoryRepository : RepositoryBase, IFirstSubCategoryRepository
    {
        private readonly IDistributedCache _cache;
        private const string CacheKey = "Lookup_First_Sub_Category";

        public FirstSubCategoryRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider, IDistributedCache cache)
            : base(context, mapperConfig, userProvider)
        {
            _cache = cache;
        }

        public async Task<QueryResultDto<FirstSubCategoryListItemDto>> GetAllAsync(FirstSubCategoriesQueryObject queryObject)
        {
            QueryResult<FirstSubCategory> result = new();
            IQueryable<FirstSubCategory> query = null;

            // Get data list from cache
            var listValueInCache = new List<FirstSubCategory>();
            //await _cache.GetRecordAsync<List<FirstSubCategory>>(CacheKey) ?? new List<FirstSubCategory>();

            if (listValueInCache.Any())
            {
                // Set query data from cache 
                query = listValueInCache.OrderBy(n => n.Name).AsQueryable();
            }
            else
            {
                var entityDbSet = db.FirstSubCategories.Include(s => s.MainCategory);

                // Set query data from database
                query = entityDbSet.OrderBy(n => n.Name).AsQueryable();

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await query.ToListAsync());
            }

            if (queryObject.MainCategoryId.HasValue)
                query = query.Where(s => s.MainCategoryId == queryObject.MainCategoryId);

            var columnsMap = new Dictionary<string, Expression<Func<FirstSubCategory, object>>>()
            {
                ["name"] = v => v.Name,
                ["mainCategory"] = v => v.MainCategory.Name
            };

            query = query.ApplySorting(queryObject, columnsMap);
            result.TotalItems = query.Count();
            query = query.ApplyPaging(queryObject);
            result.Items = query.ToList();

            return mapper.Map<QueryResult<FirstSubCategory>, QueryResultDto<FirstSubCategoryListItemDto>>(result);
        }

        public async Task<FirstSubCategoryListItemDto> GetAsync(int id)
        {
            // Get data list from cache
            var listValueInCache = new List<FirstSubCategory>();
            //await _cache.GetRecordAsync<List<FirstSubCategory>>(CacheKey) ?? new List<FirstSubCategory>();

            FirstSubCategory firstSubCategory = listValueInCache.FirstOrDefault(m => m.Id == id);

            if (firstSubCategory is null)
            {
                var entityDbSet = db.FirstSubCategories;

                // Get data from database
                firstSubCategory = await entityDbSet.FirstOrDefaultAsync(m => m.Id == id);

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.Include(x => x.MainCategory).ToListAsync());
            }

            return mapper.Map<FirstSubCategoryListItemDto>(firstSubCategory);
        }

        public async Task<FirstSubCategoryListItemDto> GetByNameAsync(string name)
        {
            // Get data list from cache
            var listValueInCache = new List<FirstSubCategory>();
            //await _cache.GetRecordAsync<List<FirstSubCategory>>(CacheKey) ?? new List<FirstSubCategory>();

            FirstSubCategory firstSubCategory = listValueInCache.FirstOrDefault(m => m.Name == name);

            if (firstSubCategory is null)
            {
                var entityDbSet = db.FirstSubCategories;

                // Get data from database
                firstSubCategory = await entityDbSet.Include(s => s.MainCategory)
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(m => m.Name == name);

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.Include(x => x.MainCategory).ToListAsync());
            }

            return mapper.Map<FirstSubCategoryListItemDto>(firstSubCategory);
        }

        public async Task<FirstSubCategoryDto> AddAsync(FirstSubCategoryDto firstSubCategoryDto)
        {
            var entityDbSet = db.FirstSubCategories;

            var entityToAdd = mapper.Map<FirstSubCategory>(firstSubCategoryDto);
            entityToAdd.Id = (await entityDbSet.IgnoreQueryFilters().MaxAsync(c => (int?)c.Id) ?? 0) + 1;
            entityToAdd.CreatedOn = DateTime.Now;
            await entityDbSet.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.Include(x => x.MainCategory).ToListAsync());

            return mapper.Map(entityToAdd, firstSubCategoryDto);
        }

        public async Task EditAsync(FirstSubCategoryDto firstSubCategoryDto)
        {
            var entityDbSet = db.FirstSubCategories;

            var entityToUpdate = await entityDbSet.FindAsync(firstSubCategoryDto.Id);
            mapper.Map(firstSubCategoryDto, entityToUpdate);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.Include(x => x.MainCategory).ToListAsync());

            mapper.Map(entityToUpdate, firstSubCategoryDto);
        }

        public async Task RemoveAsync(int id)
        {
            var entityDbSet = db.FirstSubCategories;

            var entityToDelete = await entityDbSet.FindAsync(id);
            entityDbSet.Remove(entityToDelete);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.Include(x => x.MainCategory).ToListAsync());
        }

        public async Task<bool> IsNameExistsAsync(FirstSubCategoryDto firstSubCategoryDto)
        {
            return await db.FirstSubCategories
                .AnyAsync(c => c.Name == firstSubCategoryDto.Name);
            //.AnyAsync(c => c.Id == firstSubCategoryDto.Id && c.Name == firstSubCategoryDto.Name && c.MainCategoryId == firstSubCategoryDto.MainCategoryId);
        }
    }
}
