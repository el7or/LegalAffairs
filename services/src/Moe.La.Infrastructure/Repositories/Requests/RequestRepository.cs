using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moe.La.Core.Constants;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
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
    public class RequestRepository : RepositoryBase, IRequestRepository
    {
        public RequestRepository(LaDbContext commandDb, IMapper mapperConfig, IUserProvider userProvider)
            : base(commandDb, mapperConfig, userProvider)
        {
        }

        public async Task<QueryResultDto<RequestListItemDto>> GetAllAsync(RequestQueryObject queryObject)
        {
            var result = new QueryResult<Request>();

            //check current user
            var currentUser = db.Users.Include(u => u.UserRoles).Where(u => u.Id == CurrentUser.UserId)
                .FirstOrDefault();

            //check current user roles
            var currentUserRoles = currentUser.UserRoles.Select(r => r.RoleId).ToList();

            // get userRoleManagdDepartments
            var departmentManagedByUser = await db.AppUserRoleDepartmets
                .Where(d => d.UserId == CurrentUser.UserId && d.RoleId == ApplicationRolesConstants.DepartmentManager.Code).Select(r => r.DepartmentId).ToListAsync();

            //return requests according to sendingtype role & request creator
            var query = db.Requests
                .Where(r => (
                 (r.CreatedBy == CurrentUser.UserId)

                 || r.ReceiverId == CurrentUser.UserId && r.RequestStatus != RequestStatuses.Draft)

                //not used in any request
                || (r.SendingType == SendingTypes.Department && currentUser.BranchId == r.ReceiverBranchId)
                ///

                || ((r.SendingType == SendingTypes.Role && currentUserRoles.Contains(r.ReceiverRoleId.Value) && r.RequestStatus != RequestStatuses.Draft)

                && ((currentUserRoles.Contains(ApplicationRolesConstants.DepartmentManager.Code) && r.ReceiverBranchId == currentUser.BranchId && departmentManagedByUser.Contains((int)r.ReceiverDepartmentId))

                || (currentUserRoles.Contains(ApplicationRolesConstants.BranchManager.Code) && r.ReceiverBranchId == currentUser.BranchId)

                 )
                 || (currentUserRoles.Contains(ApplicationRolesConstants.AdministrativeCommunicationSpecialist.Code) && (r.RequestStatus == RequestStatuses.Approved || r.RequestStatus == RequestStatuses.Exported) && r.IsExportable)

                || ((r.RequestType == RequestTypes.RequestSupportingDocuments || r.RequestType == RequestTypes.RequestAttachedLetter) && currentUserRoles.Contains(ApplicationRolesConstants.DepartmentManager.Code) && r.ReceiverBranchId == currentUser.BranchId && departmentManagedByUser.Contains((int)r.ReceiverDepartmentId) && (r.RequestStatus == RequestStatuses.AcceptedFromConsultant || r.RequestStatus == RequestStatuses.AcceptedFromLitigationManager))

                || ((r.RequestType == RequestTypes.RequestSupportingDocuments || r.RequestType == RequestTypes.RequestAttachedLetter) && currentUserRoles.Contains(ApplicationRolesConstants.GeneralSupervisor.Code) && (r.RequestStatus == RequestStatuses.AcceptedFromLitigationManager || r.RequestStatus == RequestStatuses.Approved))

                || (r.RequestType == RequestTypes.ConsultationSupportingDocument && currentUserRoles.Contains(ApplicationRolesConstants.GeneralSupervisor.Code)) && r.RequestStatus != RequestStatuses.Draft

                || (r.RequestType == RequestTypes.RequestExportCaseJudgment && currentUserRoles.Contains(ApplicationRolesConstants.GeneralSupervisor.Code)) && r.RequestStatus != RequestStatuses.Draft))
                .Include(u => u.CreatedByUser)
                .Include(u => u.RequestTransactions)
                .AsQueryable();



            var columnsMap = new Dictionary<string, Expression<Func<Request, object>>>()
            {
                ["id"] = v => v.Id,
                ["requestType"] = v => v.RequestType,
                ["requestStatus"] = v => v.RequestStatus,
                ["lastTransactionDate"] = v => v.CreatedOn,
                ["updatedOn"] = v => v.UpdatedOn,
                //  ["caseId"] = v => v.CaseId,
                ["createdByFullName"] = v => v.CreatedByUser.FirstName,
            };

            query = query.ApplySorting(queryObject, columnsMap);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObject);

            result.Items = await query.AsNoTracking().ToListAsync();

            return mapper.Map<QueryResult<Request>, QueryResultDto<RequestListItemDto>>(result);
        }

        public async Task<RequestListItemDto> GetAsync(int id)
        {
            var entity = await db
                .Requests
                //.Include(m => m.Case)
                .Include(u => u.CreatedByUser)
                .Where(m => m.Id == id).FirstOrDefaultAsync();

            return mapper.Map<RequestListItemDto>(entity);
        }

        public async Task ChangeRequestStatus(int requestId, RequestStatuses requestStatus)
        {
            var entityToUpdate = await db.Requests.FindAsync(requestId);

            entityToUpdate.RequestStatus = requestStatus;
            entityToUpdate.UpdatedBy = CurrentUser.UserId;
            entityToUpdate.UpdatedOn = DateTime.Now;

            await db.SaveChangesAsync();
        }

        public async Task<RequestForPrintDto> GetForPrintAsync(int id)
        {
            var entity = await db.Requests
                 //.Include(mm => mm.Case)
                 //  .ThenInclude(c => c.CaseRule)
                 .Where(m => m.Id == id).FirstOrDefaultAsync();

            var model = mapper.Map<RequestForPrintDto>(entity);

            return model;
        }
        public async Task UpdateStatusAsync(int id, RequestStatuses status)
        {
            var entity = await db.Requests
                 .Where(m => m.Id == id).FirstOrDefaultAsync();


            entity.RequestStatus = status;
            await db.SaveChangesAsync();
        }
    }
}
