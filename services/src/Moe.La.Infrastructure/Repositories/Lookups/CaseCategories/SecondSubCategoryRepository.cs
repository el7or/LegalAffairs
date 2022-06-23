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
    public class SecondSubCategoryRepository : RepositoryBase, ISecondSubCategoryRepository
    {
        private readonly IDistributedCache _cache;
        private const string CacheKey = "Lookup_Second_Sub_Categories";

        public SecondSubCategoryRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider, IDistributedCache cache)
            : base(context, mapperConfig, userProvider)
        {
            _cache = cache;
        }

        public async Task<QueryResultDto<SecondSubCategoryListItemDto>> GetAllAsync(SecondSubCategoryQueryObject queryObject)
        {
            QueryResult<SecondSubCategory> result = new();
            IQueryable<SecondSubCategory> query = null;

            // Get data list from cache
            var listValueInCache = new List<SecondSubCategory>();
            //await _cache.GetRecordAsync<List<SecondSubCategory>>(CacheKey) ?? new List<SecondSubCategory>();

            if (listValueInCache.Any())
            {
                // Set query data from cache 
                query = listValueInCache.OrderBy(n => n.Name).AsQueryable();
            }
            else
            {
                var entityDbSet = db.SecondSubCategories.Include(c => c.FirstSubCategory).ThenInclude(m => m.MainCategory);

                // Set query data from database
                query = entityDbSet.OrderBy(n => n.Name).AsQueryable();

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            if (queryObject.FirstSubCategoryId.HasValue)
            {
                query = query.Where(c => c.FirstSubCategoryId == queryObject.FirstSubCategoryId);
            }

            if (queryObject.IsActive.HasValue)
            {
                query = query.Where(c => c.IsActive == queryObject.IsActive);
            }

            var columnsMap = new Dictionary<string, Expression<Func<SecondSubCategory, object>>>()
            {
                ["name"] = v => v.Name,
                ["caseSource"] = v => v.FirstSubCategory.MainCategory.CaseSource,
                ["mainCategory"] = v => v.FirstSubCategory.MainCategory.Name,
                ["firstSubCategory"] = v => v.FirstSubCategory.Name,
            };

            query = query.ApplySorting(queryObject, columnsMap);
            result.TotalItems = query.Count();
            query = query.ApplyPaging(queryObject);
            result.Items = query.ToList();

            return Mapper.Map<QueryResult<SecondSubCategory>, QueryResultDto<SecondSubCategoryListItemDto>>(result);
        }

        public async Task<SecondSubCategoryDto> GetAsync(int id)
        {

            // Get data list from cache
            var listValueInCache = new List<SecondSubCategory>();
            //await _cache.GetRecordAsync<List<SecondSubCategory>>(CacheKey) ?? new List<SecondSubCategory>();

            SecondSubCategory secondSubCategory = listValueInCache.FirstOrDefault(m => m.Id == id);

            if (secondSubCategory is null)
            {
                var entityDbSet = db.SecondSubCategories.Include(d => d.FirstSubCategory).ThenInclude(m => m.MainCategory);

                // Get data from database
                secondSubCategory = await entityDbSet.FirstOrDefaultAsync(m => m.Id == id);

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            return mapper.Map<SecondSubCategoryDto>(secondSubCategory);
        }

        public async Task<SecondSubCategoryDto> AddAsync(SecondSubCategoryDto secondSubCategoryDto)
        {
            var entityDbSet = db.SecondSubCategories;

            var entityToAdd = mapper.Map<SecondSubCategory>(secondSubCategoryDto);
            entityToAdd.Id = (await entityDbSet.IgnoreQueryFilters().MaxAsync(c => (int?)c.Id) ?? 0) + 1;
            entityToAdd.CreatedOn = DateTime.Now;
            await entityDbSet.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.Include(d => d.FirstSubCategory).ToListAsync());

            return mapper.Map(entityToAdd, secondSubCategoryDto);
        }

        public async Task EditAsync(SecondSubCategoryDto secondSubCategoryDto)
        {
            var entityDbSet = db.SecondSubCategories;

            var entityToUpdate = await entityDbSet.FindAsync(secondSubCategoryDto.Id);
            mapper.Map(secondSubCategoryDto, entityToUpdate);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.Include(d => d.FirstSubCategory).ToListAsync());

            mapper.Map(entityToUpdate, secondSubCategoryDto);
        }

        public async Task RemoveAsync(int id)
        {
            var entityDbSet = db.SecondSubCategories;

            var entityToDelete = await entityDbSet.FindAsync(id);
            entityDbSet.Remove(entityToDelete);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.Include(d => d.FirstSubCategory).ToListAsync());
        }

        public async Task<bool> IsNameExistsAsync(SecondSubCategoryDto secondSubCategoryDto)
        {
            return await db.SecondSubCategories
                            .AnyAsync(c => c.Name == secondSubCategoryDto.Name);
            //.AnyAsync(c => c.Id != secondSubCategoryDto.Id && c.Name == secondSubCategoryDto.Name && c.FirstSubCategoryId == secondSubCategoryDto.FirstSubCategory.Id);
        }

        public async Task ChangeCategoryActivity(int secondSubCategoryId, bool IsActive)
        {
            var entityToUpdate = await db.SecondSubCategories.FindAsync(secondSubCategoryId);
            entityToUpdate.IsActive = IsActive;

            await db.SaveChangesAsync();
        }


    }
}
