using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

    public class HearingUpdateRepository : RepositoryBase, IHearingUpdateRepository
    {
        public HearingUpdateRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider)
            : base(context, mapperConfig, userProvider)
        {
        }

        public async Task<QueryResultDto<HearingUpdateListItemDto>> GetAllAsync(HearingUpdateQueryObject queryObject)
        {
            var result = new QueryResult<HearingUpdate>();

            var query = db.HearingUpdates
                .Include(c => c.Attachments)
                  .ThenInclude(c => c.Attachment)
                .Include(u => u.CreatedByUser)
                .OrderBy(n => n.Text)
                .AsNoTracking()
                .AsQueryable()
                .Where(x => x.HearingId == queryObject.HearingId);


            if (!string.IsNullOrEmpty(queryObject.SearchText))
            {
                query = query.Where(m => m.Text.Contains(queryObject.SearchText));
            }

            if (queryObject.CreatedBy != null)
                query = query.Where(c => c.CreatedBy == queryObject.CreatedBy);

            if (!string.IsNullOrEmpty(queryObject.UpdateDate))
            {
                query = query.Where(p => p.UpdateDate >= DateTime.Parse(queryObject.UpdateDate).Date);
            }

            var columnsMap = new Dictionary<string, Expression<Func<HearingUpdate, object>>>()
            {
                ["text"] = v => v.Text,
                ["updatedDate"] = v => v.UpdateDate,
                ["createdByUser"] = v => v.CreatedByUser.FirstName,
            };

            query = query.ApplySorting(queryObject, columnsMap);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObject);

            result.Items = await query.ToListAsync();

            return mapper.Map<QueryResult<HearingUpdate>, QueryResultDto<HearingUpdateListItemDto>>(result);
        }

        public async Task<HearingUpdateDetailsDto> GetAsync(int id)
        {
            var hearing = await db.HearingUpdates
                .Include(m => m.CreatedByUser)
                .Include(m => m.Attachments)
                   .ThenInclude(m => m.Attachment)
                .Include(m => m.Attachments)
                   .ThenInclude(m => m.Attachment)
                   .ThenInclude(m => m.AttachmentType)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            return mapper.Map<HearingUpdateDetailsDto>(hearing);
        }


        public async Task AddAsync(HearingUpdateDto hearingUpdateDto)
        {
            var entityToAdd = mapper.Map<HearingUpdate>(hearingUpdateDto);

            entityToAdd.CreatedBy = CurrentUser.UserId;
            entityToAdd.CreatedOn = DateTime.Now;
            entityToAdd.UpdateDate = DateTime.Now;

            await db.HearingUpdates.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            hearingUpdateDto.Id = entityToAdd.Id;
        }

        public async Task EditAsync(HearingUpdateDto hearingUpdateDto)
        {
            var entityToUpdate = await db.HearingUpdates.Include(h => h.Attachments).FirstOrDefaultAsync(s => s.Id == hearingUpdateDto.Id);

            mapper.Map(hearingUpdateDto, entityToUpdate);
            entityToUpdate.Text = hearingUpdateDto.Text;

            await db.SaveChangesAsync();

            //mapper.Map(entityToUpdate, hearingUpdateDto);
        }


    }
}
