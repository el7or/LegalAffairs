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
    public class AttachmentTypeRepository : RepositoryBase, IAttachmentTypeRepository
    {
        private readonly IDistributedCache _cache;
        private const string CacheKey = "Lookup_Attachment_Types";

        public AttachmentTypeRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider, IDistributedCache cache)
            : base(context, mapperConfig, userProvider)
        {
            _cache = cache;
        }

        public async Task<QueryResultDto<AttachmentTypeListItemDto>> GetAllAsync(AttachmentQueryObject queryObject)
        {
            QueryResult<AttachmentType> result = new();
            IQueryable<AttachmentType> query = null;

            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<AttachmentType>>(CacheKey) ?? new List<AttachmentType>();

            if (listValueInCache.Any())
            {
                // Set query data from cache 
                query = listValueInCache.OrderBy(n => n.Id).AsQueryable();
            }
            else
            {
                var entityDbSet = db.AttachmentsTypes;

                // Set query data from database
                query = entityDbSet.OrderBy(n => n.Id).AsQueryable();

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            if (queryObject.GroupName != 0)
            {
                query = query.Where(p => p.GroupName == queryObject.GroupName);
            }
            var columnsMap = new Dictionary<string, Expression<Func<AttachmentType, object>>>()
            {
                ["id"] = v => v.Id,
                ["name"] = v => v.Name
            };

            query = query.ApplySorting(queryObject, columnsMap);
            result.TotalItems = query.Count();
            query = query.ApplyPaging(queryObject);
            result.Items = query.ToList();

            return mapper.Map<QueryResult<AttachmentType>, QueryResultDto<AttachmentTypeListItemDto>>(result);
        }

        public async Task<AttachmentTypeDetailsDto> GetAsync(int id)
        {
            // Get data list from cache
            var listValueInCache = await _cache.GetRecordAsync<List<AttachmentType>>(CacheKey) ?? new List<AttachmentType>();

            AttachmentType attachmentType = listValueInCache.FirstOrDefault(m => m.Id == id);

            if (attachmentType is null)
            {
                var entityDbSet = db.AttachmentsTypes;

                // Get data from database
                attachmentType = await entityDbSet.FirstOrDefaultAsync(m => m.Id == id);

                // Update data list cache values from database
                await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
            }

            return mapper.Map<AttachmentTypeDetailsDto>(attachmentType);
        }

        public async Task AddAsync(AttachmentTypeDto attachmentTypeDto)
        {
            var entityDbSet = db.AttachmentsTypes;

            var entityToAdd = mapper.Map<AttachmentType>(attachmentTypeDto);
            entityToAdd.Id = (await entityDbSet.IgnoreQueryFilters().MaxAsync(c => (int?)c.Id) ?? 0) + 1;
            entityToAdd.CreatedOn = DateTime.Now;
            await entityDbSet.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());

            mapper.Map(entityToAdd, attachmentTypeDto);
        }

        public async Task EditAsync(AttachmentTypeDto attachmentTypeDto)
        {
            var entityDbSet = db.AttachmentsTypes;

            var entityToUpdate = await entityDbSet.FindAsync(attachmentTypeDto.Id);
            mapper.Map(attachmentTypeDto, entityToUpdate);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());

            mapper.Map(entityToUpdate, attachmentTypeDto);
        }

        public async Task RemoveAsync(int id)
        {
            var entityDbSet = db.AttachmentsTypes;

            var entityToDelete = await entityDbSet.FindAsync(id);
            entityDbSet.Remove(entityToDelete);
            await db.SaveChangesAsync();

            // Update data list cache values from database
            await _cache.SetRecordAsync(CacheKey, await entityDbSet.ToListAsync());
        }

        public async Task<bool> IsNameExistsAsync(AttachmentTypeDto attachmentTypeDto)
        {
            return await db.AttachmentsTypes
                .AnyAsync(c => c.Id != attachmentTypeDto.Id && c.Name == attachmentTypeDto.Name && (int)c.GroupName == attachmentTypeDto.GroupName.Value);
        }
    }
}
