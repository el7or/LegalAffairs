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
    public class AddingObjectionLegalMemoToCaseRequestRepository : RepositoryBase, IAddingObjectionLegalMemoToCaseRequestRepository
    {
        public AddingObjectionLegalMemoToCaseRequestRepository(LaDbContext commandDb, IMapper mapperConfig, IUserProvider userProvider)
            : base(commandDb, mapperConfig, userProvider)
        {
        }

        public async Task<AddingObjectionLegalMemoToCaseRequestDto> AddAsync(AddingObjectionLegalMemoToCaseRequestDto objectionLegalMemo)
        {
            var entityToAdd = mapper.Map<AddingObjectionLegalMemoToCaseRequest>(objectionLegalMemo);
            var researcherConsultant = await db.ResearcherConsultants
                .FirstOrDefaultAsync(m => m.ResearcherId == CurrentUser.UserId && m.IsActive);


            entityToAdd.Request = new Request
            {
                CreatedBy = CurrentUser.UserId,
                CreatedOn = DateTime.Now,
                RequestStatus = RequestStatuses.New,
                RequestType = RequestTypes.ObjectionLegalMemoRequest,
                ReceiverId = researcherConsultant.ConsultantId,
                SendingType = SendingTypes.User
            };
            entityToAdd.CaseId = objectionLegalMemo.CaseId;
            entityToAdd.LegalMemoId = objectionLegalMemo.LegalMemoId;

            await db.AddingObjectionLegalMemoToCaseRequests.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            return mapper.Map<AddingObjectionLegalMemoToCaseRequestDto>(entityToAdd);
        }

        public async Task<AddingObjectionLegalMemoToCaseRequestDto> GetAsync(int Id)
        {
            var entity = await db.AddingObjectionLegalMemoToCaseRequests
                .Include(h => h.Case)
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
                .Where(m => m.Id == Id).FirstOrDefaultAsync();

            return mapper.Map<AddingObjectionLegalMemoToCaseRequestDto>(entity);
        }

        public async Task<AddingObjectionLegalMemoToCaseRequestDto> GetByCaseAsync(int caseId)
        {
            var entity = await db.AddingObjectionLegalMemoToCaseRequests
                .Include(h => h.Case)
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
                .Where(m => m.CaseId == caseId).ToListAsync();

            return mapper.Map<AddingObjectionLegalMemoToCaseRequestDto>(entity.LastOrDefault());
        }
        public async Task<bool> CheckCaseObjectionRequestAcceptedAsync(int caseId)
        {
            return await db.AddingObjectionLegalMemoToCaseRequests
                .Include(h => h.Request)
                .AnyAsync(m => m.CaseId == caseId && m.Request.RequestStatus == RequestStatuses.Accepted);
        }

        public async Task<AddingObjectionLegalMemoToCaseRequestDto> ReplyObjectionLegalMemoRequestAsync(ReplyObjectionLegalMemoRequestDto replyObjectionLegalMemoRequestDto)
        {
            var entityToUpdate = await db.AddingObjectionLegalMemoToCaseRequests
                  .Include(m => m.Request)
                    .ThenInclude(c => c.CreatedByUser)
                  .Where(m => m.Id == replyObjectionLegalMemoRequestDto.Id)
                  .FirstOrDefaultAsync();

            entityToUpdate.ReplyNote = replyObjectionLegalMemoRequestDto.ReplyNote;
            entityToUpdate.ReplyDate = DateTime.Now;

            entityToUpdate.Request.RequestStatus = replyObjectionLegalMemoRequestDto.RequestStatus;

            entityToUpdate.Request.ReceiverId = null;

            await db.SaveChangesAsync();

            //replyObjectionLegalMemoRequestDto.ResearcherId = entityToUpdate.Request.CreatedBy;

            return mapper.Map<AddingObjectionLegalMemoToCaseRequestDto>(entityToUpdate);
        }
    }
}
