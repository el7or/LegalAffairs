using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class AddingLegalMemoToHearingRequestRepository : RepositoryBase, IAddingLegalMemoToHearingRequestRepository
    {

        public AddingLegalMemoToHearingRequestRepository(LaDbContext commandDb, IMapper mapperConfig, IUserProvider userProvider)
            : base(commandDb, mapperConfig, userProvider)
        {
        }

        public async Task<AddingLegalMemoToHearingRequestDto> AddAsync(AddingLegalMemoToHearingRequestDto hearingLegalMemo)
        {
            var entityToAdd = mapper.Map<AddingLegalMemoToHearingRequest>(hearingLegalMemo);
            var researcherConsultant = await db.ResearcherConsultants
                .FirstOrDefaultAsync(m => m.ResearcherId == CurrentUser.UserId && m.IsActive);

            var hearing = await db.Hearings
                .FirstOrDefaultAsync(m => m.Id == hearingLegalMemo.HearingId);

            entityToAdd.Request = new Request
            {
                CreatedBy = CurrentUser.UserId,
                CreatedOn = DateTime.Now,
                RequestStatus = RequestStatuses.New,
                RequestType = RequestTypes.RequestAddHearingMemo,
                ReceiverId = researcherConsultant.ConsultantId,
                SendingType = SendingTypes.User
            };
            entityToAdd.CaseId = hearing.CaseId;
            await db.HearingLegalMemoReviewRequests.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            return mapper.Map<AddingLegalMemoToHearingRequestDto>(entityToAdd);
        }

        public async Task<AddingLegalMemoToHearingRequestDetailsDto> GetAsync(int Id)
        {
            var entity = await db.HearingLegalMemoReviewRequests
                .Include(h => h.Hearing.Court)
                .Include(h => h.Hearing)
                    .ThenInclude(h => h.Case)
                      .ThenInclude(c => c.SecondSubCategory)
                        .ThenInclude(cc => cc.FirstSubCategory)
                          .ThenInclude(cc => cc.MainCategory)
                .Include(h => h.LegalMemo)
                    .ThenInclude(l => l.SecondSubCategory)
                .Include(h => h.LegalMemo)
                    .ThenInclude(l => l.CreatedByUser)
                .Include(m => m.Request)
                  .ThenInclude(m => m.RequestTransactions)
                    .ThenInclude(mm => mm.CreatedByUser)
                     .ThenInclude(ur => ur.UserRoles)
                      .ThenInclude(r => r.Role)
                .Include(h => h.Request)
                .Include(r => r.Case)
                .Where(m => m.Id == Id).FirstOrDefaultAsync();

            return mapper.Map<AddingLegalMemoToHearingRequestDetailsDto>(entity);
        }

        public async Task<AddingLegalMemoToHearingRequestDto> ReplyAddingMemoHearingRequestAsync(ReplyAddingLegalMemoToHearingRequestDto replyAddingLegalMemoToHearingRequest)
        {
            var entityToUpdate = await db.HearingLegalMemoReviewRequests
                  .Include(m => m.Request)
                  .Where(m => m.Id == replyAddingLegalMemoToHearingRequest.Id)
                  .FirstOrDefaultAsync();
            entityToUpdate.ReplyNote = replyAddingLegalMemoToHearingRequest.ReplyNote;
            entityToUpdate.ReplyDate = DateTime.Now;
            entityToUpdate.Request.UpdatedBy = CurrentUser.UserId;
            entityToUpdate.Request.UpdatedOn = DateTime.Now;

            entityToUpdate.Request.RequestStatus = replyAddingLegalMemoToHearingRequest.RequestStatus;

            await db.SaveChangesAsync();

            return mapper.Map<AddingLegalMemoToHearingRequestDto>(entityToUpdate);
        }
    }
}
