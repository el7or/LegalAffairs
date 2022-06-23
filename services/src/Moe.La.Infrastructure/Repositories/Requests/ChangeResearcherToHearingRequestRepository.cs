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
using System.Linq;
using System.Threading.Tasks;

namespace Moe.La.Infrastructure.Repositories
{
    public class ChangeResearcherToHearingRequestRepository : RepositoryBase, IChangeResearcherToHearingRequestRepository
    {
        public ChangeResearcherToHearingRequestRepository(LaDbContext context, IMapper mapper, IUserProvider userProvider)
            : base(context, mapper, userProvider)
        {
        }

        public async Task<ChangeResearcherToHearingRequestListItemDto> GetAsync(int id)
        {
            var entity = await db.ChangeResearcherToHearingRequests
                .Include(m => m.Request.CreatedByUser)
                .Include(m => m.CurrentResearcher)
                .Include(m => m.NewResearcher)
                .Include(c => c.Hearing)
                   .ThenInclude(h => h.Case)
                .Include(m => m.Request)
                    .ThenInclude(r => r.RequestTransactions)
                        .ThenInclude(r => r.CreatedByUser)
                            .ThenInclude(u => u.UserRoles)
                                .ThenInclude(r => r.Role)
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();

            return mapper.Map<ChangeResearcherToHearingRequestListItemDto>(entity);
        }

        public async Task<ChangeResearcherToHearingRequestDetailsDto> AddAsync(ChangeResearcherToHearingRequestDto changeResearcherToHearingRequestDto)
        {
            var request = new ChangeResearcherToHearingRequest
            {
                Request = new Request
                {
                    RequestType = RequestTypes.RequestResearcherChangeToHearing,
                    RequestStatus = RequestStatuses.New,
                    CreatedBy = CurrentUser.UserId,
                    CreatedOn = DateTime.Now,
                    SendingType = SendingTypes.Role,
                    ReceiverRoleId = ApplicationRolesConstants.DepartmentManager.Code,
                    ReceiverBranchId = CurrentUser.BranchId,
                    ReceiverDepartmentId = (int)Departments.Litigation,
                    Note = changeResearcherToHearingRequestDto.Note
                },
                CurrentResearcherId = CurrentUser.UserId,
                NewResearcherId = changeResearcherToHearingRequestDto.NewResearcherId,
                HearingId = changeResearcherToHearingRequestDto.HearingId
            };

            await db.ChangeResearcherToHearingRequests.AddAsync(request);
            await db.SaveChangesAsync();

            return mapper.Map<ChangeResearcherToHearingRequestDetailsDto>(request);
        }

        public async Task<ChangeResearcherToHearingRequestListItemDto> ReplyChangeResearcherToHearingRequestAsync(
            ReplyChangeResearcherToHearingRequestDto replyChangeResearcherToHearingRequestDto,
            RequestStatuses status)
        {
            var entityToUpdate = await db.ChangeResearcherToHearingRequests
                .Include(m => m.Request)
                .ThenInclude(r => r.CreatedByUser)
                .Include(m => m.CurrentResearcher)
                .Where(m => m.Id == replyChangeResearcherToHearingRequestDto.Id)
                .FirstOrDefaultAsync();

            entityToUpdate.ReplyNote = replyChangeResearcherToHearingRequestDto.ReplyNote;
            entityToUpdate.ReplyDate = DateTime.Now;
            entityToUpdate.Request.RequestStatus = status;

            await db.SaveChangesAsync();

            return mapper.Map<ChangeResearcherToHearingRequestListItemDto>(entityToUpdate);
        }

        public async Task<bool> IsMoreRequestsForSameHearing(ChangeResearcherToHearingRequestDto changeResearcherToHearingRequestDto)
        {
            return await db.ChangeResearcherToHearingRequests
                .Include(r => r.Request)
                .AnyAsync(c => c.HearingId == changeResearcherToHearingRequestDto.HearingId && c.Request.RequestStatus == RequestStatuses.New);
        }

        public Task CanceledChangeResearcherToHearingRequests()
        {
            var newRequests = db.ChangeResearcherToHearingRequests
                .Include(r => r.Request)
                .Include(h => h.Hearing)
               .Where(c => c.Request.RequestStatus == RequestStatuses.New && c.Hearing.Status != HearingStatuses.Scheduled)
               .ToList();

            foreach (var changeResearcherToHearingRequest in newRequests)
            {
                changeResearcherToHearingRequest.Request.UpdatedBy = CurrentUser.UserId;
                changeResearcherToHearingRequest.Request.UpdatedOn = DateTime.Now;
                changeResearcherToHearingRequest.Request.RequestStatus = RequestStatuses.Canceled;

                var requestTransaction = new RequestTransaction()
                {
                    CreatedOn = DateTime.Now,
                    CreatedBy = CurrentUser.UserId,
                    RequestId = changeResearcherToHearingRequest.Id,
                    RequestStatus = RequestStatuses.Canceled,
                    TransactionType = RequestTransactionTypes.Canceled,
                    Description = "عدم قبول الطلب قبل انتهاء الجلسة"
                };

                changeResearcherToHearingRequest.Request.RequestTransactions.Add(requestTransaction);
            }

            db.SaveChanges();

            return Task.CompletedTask;
        }
    }
}
