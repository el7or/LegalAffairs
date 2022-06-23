using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IBoardMeetingRepository
    {
        Task<QueryResultDto<BoardMeetingListItemDto>> GetAllAsync(BoardMeetingQueryObject queryObject);
        Task<BoardMeetingDetailsDto> GetAsync(int id);
        Task<BoardMeetingDetailsDto> GetByBoardAndMemoAsync(int boardId, int memoId);
        Task AddAsync(BoardMeetingDto boardMeetingDto);
        Task EditAsync(BoardMeetingDto boardMeetingDto);
        Task<List<int>> GetCurrentMeetingMembers(int id);
        Task<bool> CheckIsCurrentUserMemberInMeeting(int id);

    }
}
