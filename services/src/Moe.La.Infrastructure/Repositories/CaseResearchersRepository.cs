using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moe.La.Core.Constants;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Infrastructure.DbContexts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Moe.La.Infrastructure.Repositories
{
    public class CaseResearchersRepository : RepositoryBase, ICaseResearchersRepository
    {
        public CaseResearchersRepository(LaDbContext context, IMapper mapper, IUserProvider userProvider)
            : base(context, mapper, userProvider)
        {
        }

        public async Task<CaseResearchersDto> GetByCaseAsync(int caseId)
        {

            var caseResearchers = await db.CaseResearchers
                .Include(c => c.CreatedByUser)
                .ThenInclude(u => u.UserRoles)
                .ThenInclude(u => u.Role)
               .AsNoTracking()
               .FirstOrDefaultAsync(s => s.CaseId == caseId);

            return mapper.Map<CaseResearchersDto>(caseResearchers);
        }

        public async Task<CaseResearchersDto> GetByCaseAsync(int caseId, Guid? ResearcherId = null)
        {
            if (ResearcherId == null)
                ResearcherId = CurrentUser.UserId;

            var caseResearchers = await db.CaseResearchers
               .AsNoTracking()
               .FirstOrDefaultAsync(s => s.ResearcherId == ResearcherId && s.CaseId == caseId);

            return mapper.Map<CaseResearchersDto>(caseResearchers);
        }

        public async Task AddResearcher(CaseResearchersDto caseResearchersDto)
        {
            var entityToAdd = mapper.Map<CaseResearcher>(caseResearchersDto);
            entityToAdd.CreatedOn = DateTime.Now;
            entityToAdd.CreatedBy = CurrentUser.UserId;
            await db.CaseResearchers.AddAsync(entityToAdd);

            // check if any hearing in the case without assignTo
            var caseHearings = db.Hearings.Where(h => h.CaseId == caseResearchersDto.CaseId && h.AssignedTo == null);

            if (caseHearings.Count() > 0)
            {
                foreach (var hearing in caseHearings)
                {
                    hearing.AssignedToId = caseResearchersDto.ResearcherId;
                    hearing.UpdatedBy = CurrentUser.UserId;
                    hearing.UpdatedOn = DateTime.Now;
                }
            }

            await db.SaveChangesAsync();
            mapper.Map(entityToAdd, caseResearchersDto);
        }

        public async Task AddIntegratedResearcher(CaseResearchersDto caseResearchersDto)
        {
            var entityToAdd = mapper.Map<CaseResearcher>(caseResearchersDto);
            entityToAdd.CreatedOn = DateTime.Now;
            entityToAdd.CreatedBy = ApplicationConstants.SystemAdministratorId;
            await db.CaseResearchers.AddAsync(entityToAdd);

            // check if any hearing in the case without assignTo
            var caseHearings = db.Hearings.Where(h => h.CaseId == caseResearchersDto.CaseId && h.AssignedTo == null).AsQueryable();
            if (caseHearings.Count() > 0)
            {
                foreach (var hearing in caseHearings)
                {
                    hearing.AssignedToId = caseResearchersDto.ResearcherId;
                    hearing.UpdatedBy = ApplicationConstants.SystemAdministratorId;
                    hearing.UpdatedOn = DateTime.Now;
                }
            }

            await db.SaveChangesAsync();
            mapper.Map(entityToAdd, caseResearchersDto);
        }

        public async Task RemoveAsync(int id)
        {
            var entityToRemove = await db.CaseResearchers.FindAsync(id);
            entityToRemove.IsDeleted = true;
            await db.SaveChangesAsync();
        }
    }
}
