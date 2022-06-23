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
    public class ChangeResearcherRequestRepository : RepositoryBase, IChangeResearcherRequestRepository
    {
        public ChangeResearcherRequestRepository(LaDbContext context, IMapper mapper, IUserProvider userProvider)
            : base(context, mapper, userProvider)
        {
        }

        public async Task<ChangeResearcherRequestListItemDto> GetAsync(int id)
        {
            var entity = await db.ChangeResearcherRequests
                .Include(m => m.Request.CreatedByUser)
                .Include(m => m.CurrentResearcher)
                .Include(m => m.NewResearcher)
                .Include(r => r.Case)
                .Include(m => m.Request)
                    .ThenInclude(r => r.RequestTransactions)
                        .ThenInclude(r => r.CreatedByUser)
                            .ThenInclude(u => u.UserRoles)
                                .ThenInclude(r => r.Role)

                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();

            return mapper.Map<ChangeResearcherRequestListItemDto>(entity);
        }

        public async Task<ChangeResearcherRequestDetailsDto> AddAsync(ChangeResearcherRequestDto changeResearcherRequestDto)
        {
            // Current logged in researcher information for a given case.
            if (changeResearcherRequestDto.CurrentResearcherId == null)
                changeResearcherRequestDto.CurrentResearcherId = CurrentUser.UserId;

            var caseResearcher = await db.CaseResearchers.Include(c => c.Case)
                .Where(m => m.CaseId == changeResearcherRequestDto.CaseId && m.ResearcherId == changeResearcherRequestDto.CurrentResearcherId)
                .FirstOrDefaultAsync();
             
            var request = new ChangeResearcherRequest
            {
                Request = new Request
                {
                    RequestType = RequestTypes.RequestResearcherChange,
                    RequestStatus = RequestStatuses.New,
                    CreatedBy = CurrentUser.UserId,
                    CreatedOn = DateTime.Now, 
                    Note = changeResearcherRequestDto.Note,
                    SendingType = SendingTypes.Role,
                    ReceiverRoleId = ApplicationRolesConstants.DepartmentManager.Code,
                    ReceiverBranchId = caseResearcher.Case.BranchId,
                    ReceiverDepartmentId = 1,
                },
                CaseId = caseResearcher.CaseId,
                CurrentResearcherId = caseResearcher.ResearcherId,
                LegalMemoId = changeResearcherRequestDto.LegalMemoId
            };

            await db.ChangeResearcherRequests.AddAsync(request);
            await db.SaveChangesAsync();

            return mapper.Map<ChangeResearcherRequestDetailsDto>(request);
        }

        public async Task EditAsync(ChangeResearcherRequestDto changeResearcherRequestDto)
        {
            var entityToUpdate = await db.ChangeResearcherRequests.FindAsync(changeResearcherRequestDto.Id);
            mapper.Map(changeResearcherRequestDto, entityToUpdate);
            await db.SaveChangesAsync();
            mapper.Map(entityToUpdate, changeResearcherRequestDto);
        }

        public async Task<ChangeResearcherRequestListItemDto> ReplyChangeResearcherRequestAsync(
            ReplyChangeResearcherRequestDto replyChangeResearcherRequestDto,
            RequestStatuses status)
        {
            var entityToUpdate = await db.ChangeResearcherRequests
                .Include(m => m.Request)
                .ThenInclude(r => r.CreatedByUser)
                .Include(m => m.CurrentResearcher)
                .Where(m => m.Id == replyChangeResearcherRequestDto.Id)
                .FirstOrDefaultAsync();

            entityToUpdate.NewResearcherId = status == RequestStatuses.Rejected ? null : replyChangeResearcherRequestDto.NewResearcherId;
            entityToUpdate.ReplyNote = replyChangeResearcherRequestDto.ReplyNote;
            entityToUpdate.ReplyDate = DateTime.Now;
            entityToUpdate.Request.RequestStatus = status;

            await db.SaveChangesAsync();

            return mapper.Map<ChangeResearcherRequestListItemDto>(entityToUpdate);
        }
    }

}
