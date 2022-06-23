using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Infrastructure.DbContexts;
using System;
using System.Threading.Tasks;

namespace Moe.La.Infrastructure.Repositories
{
    public class HearingLegalMemoRepository : RepositoryBase, IHearingLegalMemoRepository
    {
        public HearingLegalMemoRepository(LaDbContext context, IMapper mapper, IUserProvider userProvider)
            : base(context, mapper, userProvider)
        {
        }


        public async Task<HearingLegalMemoDto> AddAsync(int RequestId)
        {
            var AddingMemoHearingRequest = await db.HearingLegalMemoReviewRequests
                .FirstOrDefaultAsync(r => r.Id == RequestId);
            var entityToAdd = new HearingLegalMemo
            {
                HearingId = AddingMemoHearingRequest.HearingId,
                LegalMemoId = AddingMemoHearingRequest.LegalMemoId,
                CreatedBy = CurrentUser.UserId,
                CreatedOn = DateTime.Now
            };

            await db.HearingLegalMemos.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            return mapper.Map<HearingLegalMemoDto>(entityToAdd);
        }

        public async Task<HearingLegalMemoDetailsDto> GetByMemoAsync(int legalMemoId)
        {
            var hearingLegalMemo = await db.HearingLegalMemos
                 .Include(h => h.Hearing)
                 .Include(l => l.LegalMemo)
                  .ThenInclude(u => u.CreatedByUser)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.LegalMemoId == legalMemoId);

            return mapper.Map<HearingLegalMemoDetailsDto>(hearingLegalMemo);
        }
        public async Task<bool> HearingHasLegalMomo(int hearingId)
        {
            var hasLegalMomo = await db.HearingLegalMemos
                 .AnyAsync(n => n.HearingId == hearingId);
            return hasLegalMomo;
        }


    }
}
