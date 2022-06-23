using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moe.La.Core.Constants;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moe.La.Infrastructure.Repositories
{
    public class ObjectionPermitRequestRepository : RepositoryBase, IObjectionPermitRequestRepository
    {
        public ObjectionPermitRequestRepository(LaDbContext db, IMapper mapperConfig, IUserProvider userProvider)
            : base(db, mapperConfig, userProvider)
        {
        }

        public async Task<ObjectionPermitRequestListItemDto> GetAsync(int id)
        {
            var entity = await db.ObjectionPermitRequests
                .Include(m => m.Request)
                  .ThenInclude(m => m.RequestTransactions)
                    .ThenInclude(mm => mm.CreatedByUser)
                     .ThenInclude(ur => ur.UserRoles)
                      .ThenInclude(r => r.Role)
                .Include(m => m.Request)
                  .ThenInclude(mm => mm.CreatedByUser)
                .Include(m => m.Request)
                .Include(mm => mm.Case)
                  .ThenInclude(mm => mm.CaseRule)

                .Where(m => m.Id == id).FirstOrDefaultAsync();

            return mapper.Map<ObjectionPermitRequestListItemDto>(entity);
        }

        public async Task<ObjectionPermitRequestDetailsDto> GetByCaseAsync(int caseId)
        {
            var entity = await db.ObjectionPermitRequests
                    .Include(m => m.Request)
                    .ThenInclude(mm => mm.CreatedByUser)
                    .Include(m => m.Request)
                    .Include(mm => mm.Case)
                    .Include(m => m.Request)
                    .ThenInclude(m => m.RequestTransactions)
                    .Where(m => m.CaseId == caseId).ToListAsync();

            return mapper.Map<ObjectionPermitRequestDetailsDto>(entity.LastOrDefault());
        }

        public async Task<ObjectionPermitRequestDetailsDto> AddAsync(ObjectionPermitRequestDto ObjectionPermitRequestDto)
        {
            var request = new ObjectionPermitRequest()
            {
                Request = new Request()
                {
                    SendingType = SendingTypes.Role,
                    RequestType = RequestTypes.ObjectionPermitRequest,
                    RequestStatus = ObjectionPermitRequestDto.RequestStatus,
                    ReceiverBranchId = CurrentUser.BranchId,
                    ReceiverDepartmentId = (int)Departments.Litigation,
                    ReceiverRoleId = ApplicationRolesConstants.DepartmentManager.Code,
                    IsExportable = false,
                    CreatedBy = CurrentUser.UserId,
                    CreatedOn = DateTime.Now
                },
                CaseId = ObjectionPermitRequestDto.CaseId,
                Note = ObjectionPermitRequestDto.Note,
                SuggestedOpinon = ObjectionPermitRequestDto.SuggestedOpinon
            };

            await db.ObjectionPermitRequests.AddAsync(request);
            await db.SaveChangesAsync();

            return mapper.Map<ObjectionPermitRequestDetailsDto>(request);
        }

        public async Task<ObjectionPermitRequestDto> ReplyObjectionPermitRequestAsync(ReplyObjectionPermitRequestDto replyObjectionPermitRequestDto)
        {
            var entityToUpdate = await db.ObjectionPermitRequests
                  .Include(m => m.Request)
                  .Include(m => m.Case)
                      .ThenInclude(m => m.CaseRule)
                  .Where(m => m.Id == replyObjectionPermitRequestDto.Id)
                  .FirstOrDefaultAsync();

            entityToUpdate.ReplyNote = replyObjectionPermitRequestDto.ReplyNote;
            entityToUpdate.ReplyDate = DateTime.Now;

            entityToUpdate.Request.RequestStatus = replyObjectionPermitRequestDto.RequestStatus;

            entityToUpdate.Request.ReceiverBranchId = null;
            entityToUpdate.Request.ReceiverId = null;
            entityToUpdate.Request.ReceiverDepartmentId = null;
            entityToUpdate.Request.ReceiverRoleId = null;

            await db.SaveChangesAsync();

            replyObjectionPermitRequestDto.ResearcherId = entityToUpdate.Request.CreatedBy;

            return mapper.Map<ObjectionPermitRequestDto>(entityToUpdate);
        }

        public Task<List<ObjectionPermitRequest>> GetExpiredObjections()
        {
            var _objectionPermitRequests = db.ObjectionPermitRequests
               .Include(r => r.Request)
                .ThenInclude(u => u.CreatedByUser)
               .Include(c => c.Case)
               .Where(r => r.Request.RequestStatus != RequestStatuses.Abandoned
                && r.Case.ReceivingJudgmentDate.Value.AddDays(30) <= DateTime.Now)
               .ToListAsync();

            return _objectionPermitRequests;
        }

        public Task UpdateExpiredObjectionRequest(ObjectionPermitRequest _objectionPermitRequest)
        {
            _objectionPermitRequest.Request.RequestStatus = RequestStatuses.Abandoned;
            _objectionPermitRequest.Request.UpdatedBy = CurrentUser.UserId;
            _objectionPermitRequest.Request.UpdatedOn = DateTime.Now;
            db.SaveChanges();

            return Task.CompletedTask;
        }

        public async Task<bool> CheckCaseObjectionPermitRequestAcceptedAsync(int caseId)
        {
            return await db.ObjectionPermitRequests
                .Include(m => m.Request)
                .AnyAsync(m => m.CaseId == caseId && m.Request.RequestStatus == RequestStatuses.AcceptedFromLitigationManager && m.SuggestedOpinon == SuggestedOpinon.ObjectionAction);

        }

    }
}
