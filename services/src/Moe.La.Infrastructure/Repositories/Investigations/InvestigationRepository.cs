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

    public class InvestigationRepository : RepositoryBase, IInvestigationRepository
    {
        public InvestigationRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider)
            : base(context, mapperConfig, userProvider)
        {
        }

        public async Task<QueryResultDto<InvestigationListItemDto>> GetAllAsync(InvestigationQueryObject queryObject)
        {
            var result = new QueryResult<Investigation>();

            var query = db.Investigations
                .Include(i => i.Investigator)
                .AsQueryable();

            if (!string.IsNullOrEmpty(queryObject.SearchText))
            {
                query = query.Where(c => c.InvestigationNumber.ToString().Contains(queryObject.SearchText)
                                      || c.Subject.Contains(queryObject.SearchText));
            }

            var columnsMap = new Dictionary<string, Expression<Func<Investigation, object>>>()
            {
                ["investigationNumber"] = v => v.InvestigationNumber,
                ["startDate"] = v => v.StartDate,
                ["subject"] = v => v.Subject,
                ["investigator"] = v => v.Investigator.FirstName,
                ["investigationStatus"] = v => v.InvestigationStatus,
                ["isHasCriminalSide"] = v => v.IsHasCriminalSide
            };

            query = query.ApplySorting(queryObject, columnsMap);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObject);

            result.Items = await query.AsNoTracking().ToListAsync();

            return mapper.Map<QueryResult<Investigation>, QueryResultDto<InvestigationListItemDto>>(result);
        }

        public async Task<InvestigationDetailsDto> GetAsync(int id)
        {
            var Investigation = await db.Investigations
                .Include(m => m.InvestigationRecords)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            return mapper.Map<InvestigationDetailsDto>(Investigation);
        }

        public async Task AddAsync(InvestigationDto investigationDto)
        {
            var entityToAdd = mapper.Map<Investigation>(investigationDto);
            entityToAdd.CreatedOn = DateTime.Now;
            entityToAdd.CreatedBy = CurrentUser.UserId;

            await db.Investigations.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            mapper.Map(entityToAdd, investigationDto);
        }

        public async Task EditAsync(InvestigationDto investigationDto)
        {
            var entityToUpdate = await db.Investigations.FindAsync(investigationDto.Id);
            mapper.Map(investigationDto, entityToUpdate);
            await db.SaveChangesAsync();
            mapper.Map(entityToUpdate, investigationDto);
        }

        public async Task RemoveAsync(int id)
        {
            var entityToDelete = await db.Investigations.FindAsync(id);
            entityToDelete.IsDeleted = true;
            await db.SaveChangesAsync();
        }
    }
}

