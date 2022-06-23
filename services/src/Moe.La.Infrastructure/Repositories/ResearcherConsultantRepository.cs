using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moe.La.Common;
using Moe.La.Core.Constants;
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
    public class ResearcherConsultantRepository : RepositoryBase, IResearcherConsultantRepository
    {
        public ResearcherConsultantRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider)
            : base(context, mapperConfig, userProvider)
        {
        }

        public async Task AddAsync(ResearcherConsultantDto researcherConsultantDto)
        {

            //check if record in database exists with same researcher and consultant connected so we will edit this record
            var entityInDb = db.ResearcherConsultants
                .Where(r => r.ResearcherId == researcherConsultantDto.ResearcherId && r.ConsultantId == researcherConsultantDto.ConsultantId)
                .FirstOrDefault();

            if (entityInDb != null)
            {
                entityInDb.StartDate = DateTime.Now;
                entityInDb.EndDate = null;
                entityInDb.IsActive = true;
                await db.SaveChangesAsync();
                mapper.Map(entityInDb, researcherConsultantDto);
            }
            else
            {
                var entityToAdd = mapper.Map<ResearcherConsultant>(researcherConsultantDto);

                entityToAdd.CreatedBy = CurrentUser.UserId;
                entityToAdd.CreatedOn = DateTime.Now;
                entityToAdd.StartDate = DateTime.Now;
                entityToAdd.EndDate = null;
                entityToAdd.IsActive = true;

                db.ResearcherConsultants.Add(entityToAdd);
                await db.SaveChangesAsync();
                mapper.Map(entityToAdd, researcherConsultantDto);
            }
        }
        public async Task AddToHistoryAsync(ResearcherConsultantDto researcherConsultantDto)
        {
            // previous active relation to be saved in history and then be deactivated
            var activeEntity = db.ResearcherConsultants
                .Where(r => r.ResearcherId == researcherConsultantDto.ResearcherId && r.IsActive == true)
                .FirstOrDefault();
            if (activeEntity != null)
            {
                activeEntity.IsActive = false;
                activeEntity.EndDate = DateTime.Now;
                db.ResearcherConsultants.Update(activeEntity);

                // add previous active relation to history
                var activeEntityHistoryToAdd = mapper.Map<ResearcherConsultantHistory>(activeEntity);
                activeEntityHistoryToAdd.CreatedBy = CurrentUser.UserId;
                activeEntityHistoryToAdd.CreatedOn = DateTime.Now;
                activeEntityHistoryToAdd.EndDate = DateTime.Now;
                activeEntityHistoryToAdd.IsActive = true;
                db.ResearcherConsultantsHistory.Add(activeEntityHistoryToAdd);
                await db.SaveChangesAsync();
            }
        }
        public async Task<QueryResultDto<ResearcherConsultantListItemDto>> GetAllAsync(ResearcherQueryObject queryObject)
        {
            var result = new QueryResult<ResearcherConsultantListItemDto>();

            var query = (from u in db.Users
                         join rc in db.ResearcherConsultants.Where(r => r.IsActive == true
                         //&& r.Consultant.Enabled == true
                         //&& r.Consultant.UserRoles.Any(m => m.RoleId == ApplicationRolesConstants.LegalConsultant.Code
                         //)
                         ) on u.Id equals rc.ResearcherId into mm
                         from nn in mm.DefaultIfEmpty()
                         where u.UserRoles.Any(m => m.RoleId == ApplicationRolesConstants.LegalResearcher.Code) && u.Id != ApplicationConstants.SystemAdministratorId
                         select new ResearcherConsultantListItemDto
                         {
                             Id = nn.Id,
                             Consultant = nn.Consultant.FirstName + " " + nn.Consultant.LastName,
                             ConsultantId = nn.ConsultantId,
                             StartDate = nn.StartDate,
                             StartDateHigri = DateTimeHelper.GetHigriDate(nn.StartDate),
                             Researcher = u.FirstName + " " + u.LastName,
                             Enabled = u.Enabled,
                             ResearcherId = u.Id,
                             ResearcherDepartmentId = u.BranchId,
                             ResearcherDepartment = u.Branch.Name,
                             ConsultantDepartment = nn.Consultant.Branch.Name
                         });



            // if the user does not have admin role => return researchers in the same department only
            if (!queryObject.AllDepartments)
            {
                var currentUser = await db.Users.Where(c => c.Id == CurrentUser.UserId).FirstOrDefaultAsync();
                query = query.Where(c => c.ResearcherDepartmentId == currentUser.BranchId);
            }

            if (queryObject.HasConsultant)
                query = query.Where(s => s.ConsultantId != null);

            if (queryObject.ResearcherId != null)
                query = query.Where(s => s.ResearcherId == queryObject.ResearcherId);

            if (queryObject.ConsultantId != null)
                query = query.Where(s => s.ConsultantId == queryObject.ConsultantId);

            if (queryObject.IsSameGeneralManagement == true)
            {
                query = query.Where(p => p.ResearcherDepartmentId == CurrentUser.BranchId);
            }
            var columnsMap = new Dictionary<string, Expression<Func<ResearcherConsultantListItemDto, object>>>()
            {
                ["researcher"] = v => v.Researcher,
                ["consultant"] = v => v.Consultant,
                ["startDate"] = v => v.StartDate,
            };

            query = query.ApplySorting(queryObject, columnsMap);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObject);

            result.Items = await query.ToListAsync();

            return mapper.Map<QueryResult<ResearcherConsultantListItemDto>, QueryResultDto<ResearcherConsultantListItemDto>>(result);

        }

        public async Task<bool> CheckCurrentResearcherHasConsultantAsync()
        {
            var researcherConsultant = await db.ResearcherConsultants.Include(r => r.Consultant)
                .FirstOrDefaultAsync(r => r.ResearcherId == CurrentUser.UserId && r.IsActive == true && r.Consultant.Enabled == true);

            return researcherConsultant != null;
        }
        public async Task<ResearcherConsultantDto> GetAsync(int id)
        {
            var researcherConsultant = await db.ResearcherConsultants
                .FirstOrDefaultAsync(r => r.Id == id);

            return mapper.Map<ResearcherConsultantDto>(researcherConsultant);
        }
        public async Task<ResearcherConsultantDto> GetConsultantAsync(Guid researcherId)
        {
            var researcherConsultant = await db.ResearcherConsultants
                .FirstOrDefaultAsync(r => r.ResearcherId == researcherId);

            return mapper.Map<ResearcherConsultantDto>(researcherConsultant);
        }
    }
}
