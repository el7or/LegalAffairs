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

    public class InvestigationRecordRepository : RepositoryBase, IInvestigationRecordRepository
    {
        public InvestigationRecordRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider)
            : base(context, mapperConfig, userProvider)
        {
        }

        public async Task<QueryResultDto<InvestigationRecordListItemDto>> GetAllAsync(InvestiationRecordQueryObject queryObject)
        {
            var result = new QueryResult<InvestigationRecord>();

            var query = db.InvestigationRecords
                .Include(r => r.CreatedByUser)
                .AsQueryable();

            if (queryObject.InvestigationId != null)
            {
                query = query.Where(r => r.InvestigationId == queryObject.InvestigationId);
            }

            if (!string.IsNullOrEmpty(queryObject.SearchText))
            {
                query = query.Where(m =>
                m.RecordNumber.Contains(queryObject.SearchText)
                );
            }
            var columnsMap = new Dictionary<string, Expression<Func<InvestigationRecord, object>>>()
            {
                ["recordNumber"] = v => v.RecordNumber,
                ["startDate"] = v => v.StartDate,
                ["EndDate"] = v => v.EndDate
            };

            query = query.ApplySorting(queryObject, columnsMap);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObject);

            result.Items = await query.AsNoTracking().ToListAsync();

            return mapper.Map<QueryResult<InvestigationRecord>, QueryResultDto<InvestigationRecordListItemDto>>(result);
        }

        public async Task<InvestigationRecordDetailsDto> GetAsync(int id)
        {
            var InvestigationRecord = await db.InvestigationRecords
                .Include(m => m.InvestigationRecordInvestigators)
                    .ThenInclude(mm => mm.Investigator)
                .Include(m => m.InvestigationRecordParties)
                     .ThenInclude(mm => mm.InvestigationRecordPartyType)
                .Include(m => m.InvestigationRecordParties)
                     .ThenInclude(mm => mm.InvestigationPartyPenalties)
                .Include(m => m.InvestigationRecordParties)
                     .ThenInclude(mm => mm.Evaluations)
                .Include(m => m.InvestigationRecordParties)
                     .ThenInclude(mm => mm.EducationalLevels)
                .Include(m => m.InvestigationRecordQuestions)
                    .ThenInclude(mm => mm.Question)
                .Include(m => m.InvestigationRecordQuestions)
                    .ThenInclude(mm => mm.AssignedTo)
                .Include(m => m.Attendants)
                    .ThenInclude(mm => mm.RepresentativeOf)
                .Include(m => m.Attachments)
                    .ThenInclude(mm => mm.Attachment)
                    .ThenInclude(mm => mm.AttachmentType)
                .Include(m => m.Attachments)
                    .ThenInclude(mm => mm.Attachment)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            return mapper.Map<InvestigationRecordDetailsDto>(InvestigationRecord);
        }

        public async Task AddAsync(InvestigationRecordDto investigationRecordDto)
        {
            var entityToAdd = mapper.Map<InvestigationRecord>(investigationRecordDto);
            entityToAdd.CreatedOn = DateTime.Now;
            entityToAdd.CreatedBy = CurrentUser.UserId;

            await db.InvestigationRecords.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            // get investigation record id to use it in attachment
            investigationRecordDto.Id = entityToAdd.Id;
        }

        public async Task<bool> checkPartyExist(int investigationRecordPartyId, int investigationRecordId)
        {
            return await db.InvestigationRecordParties.AnyAsync(p => p.Id == investigationRecordPartyId && p.InvestigationRecordId == investigationRecordId);
        }
        public async Task EditAsync(InvestigationRecordDto investigationRecordDto)
        {

            //foreach (var item in investigationRecordDto.InvestigationRecordParties)
            //{
            //    var party = commandDb.InvestigationRecordParties.FirstOrDefault(p => p.Id == item.Id);
            //    if(party!=null)
            //    {

            //        investigationRecordDto.InvestigationRecordParties.Remove(item);
            //    }
            //}          
            var entityToUpdate = await db.InvestigationRecords
                .Include(m => m.InvestigationRecordInvestigators)
                .Include(m => m.InvestigationRecordParties)
                     .ThenInclude(mm => mm.InvestigationRecordPartyType)
                .Include(m => m.InvestigationRecordParties)
                     .ThenInclude(mm => mm.InvestigationPartyPenalties)
                .Include(m => m.InvestigationRecordParties)
                     .ThenInclude(mm => mm.Evaluations)
                .Include(m => m.InvestigationRecordParties)
                     .ThenInclude(mm => mm.EducationalLevels)
                .Include(m => m.InvestigationRecordQuestions)
                .Include(m => m.InvestigationRecordQuestions)
                .Include(m => m.Attendants)
                .Include(m => m.Attachments)
                .FirstOrDefaultAsync(s => s.Id == investigationRecordDto.Id);

            mapper.Map(investigationRecordDto, entityToUpdate);
            await db.SaveChangesAsync();
            mapper.Map(entityToUpdate, investigationRecordDto);
        }

        public async Task RemoveAsync(int id)
        {
            var entityToDelete = await db.InvestigationRecords.FindAsync(id);
            entityToDelete.IsDeleted = true;
            await db.SaveChangesAsync();
        }


    }
}

