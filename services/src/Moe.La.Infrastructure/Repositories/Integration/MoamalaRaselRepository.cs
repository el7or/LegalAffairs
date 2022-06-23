using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moe.La.Core.Constants;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Enums.Integration;
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
    public class MoamalaRaselRepository : RepositoryBase, IMoamalaRaselRepository
    {
        public MoamalaRaselRepository(LaDbContext command, IMapper mapper, IUserProvider userProvider)
            : base(command, mapper, userProvider)
        {

        }

        public async Task<QueryResultDto<MoamalaRaselListItemDto>> GetAllAsync(MoamalatRaselQueryObject queryObject)
        {

            var result = new QueryResult<MoamalaRasel>();

            var query = db.MoamalatRasel
                .AsNoTracking().Where(m => !m.IsDeleted && m.Status != MoamalaRaselStatuses.Received)
                .AsQueryable();

            query = query.Where(p =>
                ((p.Status == MoamalaRaselStatuses.Imported) &&
                // return all for general supervisor
                (CurrentUser.Roles.Contains(ApplicationRolesConstants.GeneralSupervisor.Name) ||

                // return all for general Distributor with confidential permission
                (CurrentUser.Roles.Contains(ApplicationRolesConstants.Distributor.Name) && CurrentUser.Permissions.Contains(ApplicationRoleClaimsConstants.ConfidentialMoamla.ClaimValue) && p.ItemPrivacy == ConfidentialDegrees.Confidential) ||

                // return all for general Distributor except confidential moamala
                (CurrentUser.Roles.Contains(ApplicationRolesConstants.Distributor.Name) && p.ItemPrivacy != ConfidentialDegrees.Confidential)))
                );


            if (queryObject.Status.HasValue)
            {
                if (queryObject.Status != 0) // we set all value to 0
                {
                    query = query.Where(m => m.Status == (MoamalaRaselStatuses)queryObject.Status);
                }
            }

            if (queryObject.ItemPrivacy.HasValue)
            {
                if (queryObject.ItemPrivacy != 0) // we set all value to 0
                {
                    query = query.Where(m => m.ItemPrivacy == queryObject.ItemPrivacy);
                }
            }

            if (!string.IsNullOrEmpty(queryObject.SearchText))
            {
                query = query.Where(m =>
                    m.Subject.Contains(queryObject.SearchText) ||
                    m.Comments.Contains(queryObject.SearchText)
                );
            }

            if (queryObject.CreatedOnFrom.HasValue)
            {
                query = query.Where(p => p.CreatedOn.Date >= queryObject.CreatedOnFrom.Value.Date);
            }

            if (queryObject.CreatedOnTo.HasValue)
            {
                query = query.Where(p => p.CreatedOn.Date <= queryObject.CreatedOnTo.Value.Date);
            }

            var columnsMap = new Dictionary<string, Expression<Func<MoamalaRasel, object>>>()
            {
                ["id"] = v => v.Id,
                ["unifiedNo"] = v => v.UnifiedNumber,
                ["moamalaNumber"] = v => v.ItemNumber,
                ["createdOn"] = v => v.CreatedOn,
                ["subject"] = v => v.Subject,
                ["comments"] = v => v.Comments,
                ["confidentialDegree"] = v => v.ItemPrivacyName,
                ["status"] = v => v.Status,
            };

            query = query.ApplySorting(queryObject, columnsMap);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObject);

            result.Items = await query.AsNoTracking().ToListAsync();

            return mapper.Map<QueryResult<MoamalaRasel>, QueryResultDto<MoamalaRaselListItemDto>>(result);
        }

        public async Task<MoamalaRaselDto> GetAsync(int id)
        {
            var _case = await db.MoamalatRasel.FindAsync(id);

            return mapper.Map<MoamalaRasel, MoamalaRaselDto>(_case);
        }

        public async Task AddAsync(MoamalaRaselDto moamalaRaselDto)
        {
            var entityToAdd = mapper.Map<MoamalaRasel>(moamalaRaselDto);

            entityToAdd.CreatedOn = DateTime.Now;

            entityToAdd.Status = MoamalaRaselStatuses.Imported;

            await db.MoamalatRasel.AddAsync(entityToAdd);

            await db.SaveChangesAsync();

            mapper.Map<MoamalaRaselDto>(entityToAdd);
        }
        public async Task<MoamalaRaselDto> ReceiveMoamalaAsync(int id)
        {
            var entityToEdit = await db.MoamalatRasel.FindAsync(id);

            entityToEdit.UpdatedBy = CurrentUser.UserId;
            entityToEdit.UpdatedOn = DateTime.Now;
            entityToEdit.Status = MoamalaRaselStatuses.Received;

            await db.SaveChangesAsync();

            return mapper.Map<MoamalaRaselDto>(entityToEdit);
        }

        public async Task RemoveAsync(int id)
        {
            var entityToDelete = await db.MoamalatRasel.FindAsync(id);

            entityToDelete.UpdatedBy = CurrentUser.UserId;
            entityToDelete.UpdatedOn = DateTime.Now;
            entityToDelete.IsDeleted = true;

            await db.SaveChangesAsync();
        }
    }
}
