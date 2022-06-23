using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class LegalMemoNoteRepository : RepositoryBase, ILegalMemoNoteRepository
    {
        public LegalMemoNoteRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider)
            : base(context, mapperConfig, userProvider)
        {
        }

        public async Task<QueryResultDto<LegalMemoNoteListItemDto>> GetAllAsync(LegalMemoNoteQueryObject queryObject)
        {
            var result = new QueryResult<LegalMemoNote>();

            var query = db.LegalMemoNotes
               .Include(a => a.CreatedByUser)
               //.Include(m => m.Role)
               .Include(b => b.LegalBoard)
               .Include(m => m.LegalMemo)
               .ThenInclude(m => m.LegalBoardMemos)
               .ThenInclude(bm => bm.LegalBoard)
               .ThenInclude(b => b.LegalBoardMembers)
               .OrderBy(n => n.ReviewNumber)
               .AsNoTracking()
               .AsQueryable();

            if (queryObject.LegalMemoId.HasValue)
                query = query.Where(s => s.LegalMemoId == queryObject.LegalMemoId);

            var columnsMap = new Dictionary<string, Expression<Func<LegalMemoNote, object>>>()
            {
                ["reviewNumber"] = v => v.ReviewNumber,
                ["createdOn"] = v => v.CreatedOn,
                ["createdBy"] = v => v.CreatedByUser.FirstName
            };

            query = query.ApplySorting(queryObject, columnsMap);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObject);

            result.Items = await query.ToListAsync();

            return mapper.Map<QueryResult<LegalMemoNote>, QueryResultDto<LegalMemoNoteListItemDto>>(result);
        }

        public async Task<LegalMemoNoteListItemDto> GetAsync(int id)
        {
            var legalMemoNote = await db.LegalMemoNotes
               .Include(u => u.CreatedByUser)
               .AsNoTracking()
               .FirstOrDefaultAsync(s => s.Id == id);

            return mapper.Map<LegalMemoNoteListItemDto>(legalMemoNote);
        }

        public async Task AddAsync(LegalMemoNoteDto legalMemoNoteDto)
        {
            var entityToAdd = mapper.Map<LegalMemoNote>(legalMemoNoteDto);

            entityToAdd.CreatedBy = CurrentUser.UserId;
            entityToAdd.CreatedOn = DateTime.Now;



            // Set ReviewNumber value from Dto
            entityToAdd.ReviewNumber = await GetCurrentReviewNumberAsync(entityToAdd.LegalMemoId);

            await db.LegalMemoNotes.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            mapper.Map(entityToAdd, legalMemoNoteDto);
        }

        public async Task EditAsync(LegalMemoNoteDto legalMemoNoteDto)
        {
            var entityToUpdate = await db.LegalMemoNotes
                .FirstOrDefaultAsync(s => s.Id == legalMemoNoteDto.Id);

            mapper.Map(legalMemoNoteDto, entityToUpdate);

            entityToUpdate.CreatedBy = CurrentUser.UserId;
            entityToUpdate.CreatedOn = DateTime.Now;

            await db.SaveChangesAsync();

            mapper.Map(entityToUpdate, legalMemoNoteDto);
        }

        public async Task RemoveAsync(int id)
        {
            var entityToDelete = await db.LegalMemoNotes.FindAsync(id);

            entityToDelete.CreatedBy = CurrentUser.UserId;
            entityToDelete.CreatedOn = DateTime.Now;
            entityToDelete.IsDeleted = true;

            await db.SaveChangesAsync();
        }

        public async Task<int> GetCurrentReviewNumberAsync(int legalMemoId)
        {
            int reviewNumber = 0;

            int ckeckReviewNumber = await db.LegalMemoNotes
                    .Where(m => m.LegalMemoId == legalMemoId && !m.IsClosed && !m.IsDeleted).CountAsync();

            if (ckeckReviewNumber > 0)
            {
                //same review number
                reviewNumber = await GetNextReviewNumber(legalMemoId, false);
            }
            else
            {
                //new review number
                reviewNumber = await GetNextReviewNumber(legalMemoId, true) + 1;
            }

            return reviewNumber;
        }

        private async Task<int> GetNextReviewNumber(int legalMemoId, bool closed)
        {
            var legalMemoNote = await db.LegalMemoNotes
                   .Where(m => m.LegalMemoId == legalMemoId && m.IsClosed == closed && !m.IsDeleted)
                   .OrderByDescending(h => h.ReviewNumber)
                   .FirstOrDefaultAsync();

            if (legalMemoNote != null)
                return legalMemoNote.ReviewNumber;
            else
                return 0;
        }

        public async Task CloseCurrentNotesAsync(int legalMemoId)
        {
            var entityToUpdate = await db.LegalMemoNotes
                .Where(s => s.LegalMemoId == legalMemoId && !s.IsClosed).ToListAsync();

            //update
            foreach (var row in entityToUpdate)
            {
                row.IsClosed = true;
            }

            await db.SaveChangesAsync();
        }
    }
}
