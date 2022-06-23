using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Entities.Litigation.BoardMeeting;
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
    public class BoardMeetingRepository : RepositoryBase, IBoardMeetingRepository
    {
        public BoardMeetingRepository(LaDbContext context, IMapper mapperConfig, IUserProvider userProvider)
            : base(context, mapperConfig, userProvider)
        {
        }

        #region board-meating
        public async Task<QueryResultDto<BoardMeetingListItemDto>> GetAllAsync(BoardMeetingQueryObject queryObject)
        {
            var result = new QueryResult<BoardMeeting>();

            var query = db.BoardMeetings
                .Include(b => b.Board)
                .Include(b => b.Memo)
                .Include(b => b.BoardMeetingMembers)
                .Where(b => b.BoardMeetingMembers.Any(m => m.BoardMember.UserId == CurrentUser.UserId))
                .AsNoTracking()
                .AsQueryable();

            var columnsMap = new Dictionary<string, Expression<Func<BoardMeeting, object>>>()
            {
                ["board"] = v => v.Board.Name,
                ["memo"] = v => v.Memo.Name,
                ["meetingDate"] = v => v.MeetingDate,
                ["meetingPlace"] = v => v.MeetingPlace
            };

            if (queryObject.MeetingDateFrom.HasValue)
            {
                query = query.Where(p => p.MeetingDate.Date >= queryObject.MeetingDateFrom.Value.Date);
            }

            if (queryObject.MeetingDateTo.HasValue)
            {
                query = query.Where(p => p.MeetingDate.Date <= queryObject.MeetingDateTo.Value.Date);
            }

            if (!string.IsNullOrEmpty(queryObject.MeetingPlace))
            {
                query = query.Where(p => p.MeetingPlace == queryObject.MeetingPlace);
            }

            if (!string.IsNullOrEmpty(queryObject.SearchText))
            {
                query = query.Where(c => c.Id.ToString().Contains(queryObject.SearchText)
                                      || c.MeetingDate.Date.ToString().Contains(queryObject.SearchText)
                                      || c.Memo.Name.ToLower().Contains(queryObject.SearchText)
                                      || c.Board.Name.ToLower().Contains(queryObject.SearchText)
                                      || c.MeetingPlace.Contains(queryObject.SearchText)
                                     );
            }

            query = query.ApplySorting(queryObject, columnsMap);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObject);

            result.Items = await query.ToListAsync();

            return mapper.Map<QueryResult<BoardMeeting>, QueryResultDto<BoardMeetingListItemDto>>(result);
        }

        public async Task<BoardMeetingDetailsDto> GetAsync(int id)
        {
            var boardMeeting = await db.BoardMeetings
                .Include(n => n.BoardMeetingMembers)
                 .ThenInclude(nn => nn.BoardMember)
                   .ThenInclude(nnn => nnn.User)
                .Include(n => n.Memo)
                .Include(n => n.Board)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);
            return mapper.Map<BoardMeetingDetailsDto>(boardMeeting);
        }

        public async Task<BoardMeetingDetailsDto> GetByBoardAndMemoAsync(int boardId, int memoId)
        {
            var boardMeeting = await db.BoardMeetings
                .Include(n => n.BoardMeetingMembers)
                 .ThenInclude(nn => nn.BoardMember)
                   .ThenInclude(nnn => nnn.User)
                .Include(n => n.Memo)
                .Include(n => n.Board)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.BoardId == boardId && b.MemoId == memoId);
            return mapper.Map<BoardMeetingDetailsDto>(boardMeeting);
        }

        public async Task AddAsync(BoardMeetingDto boardMeetingDto)
        {
            var entityToAdd = mapper.Map<BoardMeeting>(boardMeetingDto);

            entityToAdd.CreatedOn = DateTime.Now;
            entityToAdd.CreatedBy = CurrentUser.UserId;
            entityToAdd.BoardMeetingMembers.ToList().ForEach(m =>
            {
                m.CreatedOn = DateTime.Now;
                m.CreatedBy = CurrentUser.UserId;
            });

            await db.BoardMeetings.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            boardMeetingDto.Id = entityToAdd.Id;
        }

        public async Task EditAsync(BoardMeetingDto boardMeetingDto)
        {
            var entityToUpdate = await db.BoardMeetings
                .Where(b => b.Id == boardMeetingDto.Id)
                .Include(m => m.BoardMeetingMembers)
                .FirstOrDefaultAsync();

            //db.BoardMeetingMembers.RemoveRange(entityToUpdate.BoardMeetingMembers);
            //await db.SaveChangesAsync();

            mapper.Map(boardMeetingDto, entityToUpdate);

            entityToUpdate.UpdatedBy = CurrentUser.UserId;
            entityToUpdate.UpdatedOn = DateTime.Now;
            entityToUpdate.BoardMeetingMembers.ToList().ForEach(m =>
            {
                m.CreatedOn = DateTime.Now;
                m.CreatedBy = CurrentUser.UserId;
            });

            await db.SaveChangesAsync();
        }

        public async Task<List<int>> GetCurrentMeetingMembers(int id)
        {
            var meetingMembers = await db.BoardMeetingMembers
                .Where(b => b.BoardMeetingId == id).Select(b => b.BoardMemberId).ToListAsync();

            return meetingMembers;
        }

        public async Task<bool> CheckIsCurrentUserMemberInMeeting(int id)
        {

            return await db.BoardMeetingMembers
                .AnyAsync(b => b.BoardMeetingId == id && b.BoardMember.UserId == CurrentUser.UserId);
        }

        #endregion
    }
}
